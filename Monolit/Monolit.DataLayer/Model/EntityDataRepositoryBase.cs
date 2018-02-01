using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EntityFramework.DynamicFilters;
using LinqKit;
using Monolit.DataLayer.Accessors;
using Monolit.DataLayer.Expressions;
using Monolit.DataLayer.ORM.Repositories;
using Monolit.Interfaces;
using ViageSoft.SystemServices;
using ViageSoft.SystemServices.Collections;
using ViageSoft.SystemServices.Extensions;

namespace Monolit.DataLayer.Model
{
	public static class EntityDataRepositoryBase
	{
		private static readonly Dictionary<Type, object> __dataEntryAccessorMap = new Dictionary<Type, object>();
		private static readonly ReaderWriterLockWrapper __dataEntryAccessorMapLock = new ReaderWriterLockWrapper();

		public static IList<IDataEntryPropertyAccessor<TDataEntry>> GetAccessors<TDataEntry>()
		{
			return (IList<IDataEntryPropertyAccessor<TDataEntry>>)__dataEntryAccessorMapLock.ReadWrite(__dataEntryAccessorMap, typeof(TDataEntry), type =>
			{
				var skipProps = new List<string> { "ID", "DocID", "UID" };
				if (typeof(VersionedEntryDao).IsAssignableFrom(typeof(TDataEntry)))
					skipProps.Add("Revision");

				var dataEntryAccessor = new DataEntryAccessor<TDataEntry>();
				return dataEntryAccessor.Properties.Where(p => !skipProps.Contains(p.Name)).ToList();
			});
		}
	}

	public abstract class EntityDataRepositoryBase<TDataContext, TDataEntry> : IDataRepository<TDataEntry>
		where TDataContext : DbContext, new()
		where TDataEntry : class, IDataEntry, new()
	{
		protected EntityDataRepositoryBase()
		{
			Debug.Assert(UpdatableProperties.Count > 0);
		}

		private readonly Lazy<IList<IDataEntryPropertyAccessor<TDataEntry>>> _lazyUpdatableProperties =
			new Lazy<IList<IDataEntryPropertyAccessor<TDataEntry>>>(EntityDataRepositoryBase.GetAccessors<TDataEntry>);

		protected IList<IDataEntryPropertyAccessor<TDataEntry>> UpdatableProperties => _lazyUpdatableProperties.Value;

		private Func<IQueryable<TDataEntry>, IQueryable<TDataEntry>> _customizeQueryOptions;

		public TDataEntry FindSingle(Guid uid)
		{
			return FindRange(0, 2, entry => entry.UID == uid).SingleOrDefault();
		}

		object IDataRepository.FindSingle(Guid uid)
		{
			return FindSingle(uid);
		}

		public TDataEntry FindSingle(long docID)
		{
			return FindRange(0, 2, entry => entry.DocID == docID).SingleOrDefault();
		}

		object IDataRepository.FindSingle(long docID)
		{
			return FindSingle(docID);
		}

		public TDataEntry FindSingle(object parameters)
		{
			return FindRange(0, 2, GetExpression(parameters)).SingleOrDefault();
		}

		object IDataRepository.FindSingle(object parameters)
		{
			return FindSingle(parameters);
		}

		public TDataEntry FindFirst(object parameters)
		{
			return FindRange(0, 1, GetExpression(parameters)).FirstOrDefault();
		}

		object IDataRepository.FindFirst(object parameters)
		{
			return FindFirst(parameters);
		}

		public IEnumerable<TDataEntry> FindFirst(int count, object parameters)
		{
			return FindRange(0, count, GetExpression(parameters));
		}

		IEnumerable IDataRepository.FindFirst(int count, object parameters)
		{
			return FindFirst(count, parameters);
		}

		public IEnumerable<TDataEntry> FindNext(int startIndex, int count, object parameters)
		{
			return FindRange(startIndex, count, GetExpression(parameters));
		}

		IEnumerable IDataRepository.FindNext(int startIndex, int count, object parameters)
		{
			return FindNext(startIndex, count, parameters);
		}

		public IEnumerable<TDataEntry> FindAll(object parameters)
		{
			return FindRange(0, int.MaxValue, GetExpression(parameters));
		}

		IEnumerable IDataRepository.FindAll(object parameters)
		{
			return FindAll(parameters);
		}

		public IEnumerable<TDataEntry> FindAll()
		{
			return FindRange(0, int.MaxValue);
		}

		public IEnumerable<TDataEntry> FindAll(Expression<Func<TDataEntry, bool>> filterExpression)
		{
			return FindRange(0, int.MaxValue, filterExpression);
		}

		IEnumerable IDataRepository.FindAll()
		{
			return FindAll();
		}

		public ResultSet<TDataEntry> FindPage(int pageNumber, int pageSize, object parameters)
		{
			int startIndex = (pageNumber - 1) * pageSize;
			if (pageSize != int.MaxValue) pageSize++;
			return new ResultSet<TDataEntry>(FindRange(startIndex, pageSize, GetExpression(parameters)), startIndex, pageSize);
		}

		public ResultSet<TDataEntry> FindPage(int pageNumber, int pageSize, Expression<Func<TDataEntry, bool>> filterExpression, Func<IQueryable<TDataEntry>, IOrderedQueryable<TDataEntry>> orderBy = null)
		{
			int startIndex = (pageNumber - 1) * pageSize;
			if (pageSize != int.MaxValue) pageSize++;
			return new ResultSet<TDataEntry>(FindRange(startIndex, pageSize, filterExpression, orderBy), startIndex, pageSize);
		}

		ResultSet IDataRepository.FindPage(int pageNumber, int pageSize, object parameters)
		{
			return FindPage(pageNumber, pageSize, parameters);
		}

		public TDataEntry Insert(object parameters)
		{
			throw new NotImplementedException();
		}

		public TDataEntry Insert(TDataEntry entity)
		{
			return Perform(db => Insert(db, entity));
		}

		private TDataEntry Insert(TDataContext db, TDataEntry entity)
		{
			DbSet<TDataEntry> dbSet = db.Set<TDataEntry>();
			dbSet.Add(entity);
			db.SaveChanges();
			if (_customizeQueryOptions != null)
				entity = WithOptions(dbSet).Single(i => i.UID == entity.UID);
			else
				db.Entry(entity).Reload();
			return entity;
		}

		object IDataRepository.Insert(object parameters)
		{
			return Insert(parameters);
		}

		public TDataEntry Update(object parameters)
		{
			throw new NotImplementedException();
		}

		public TDataEntry Update(TDataEntry entity)
		{
			return Perform(db => Update(db, entity));
		}

		private TDataEntry Update(TDataContext db, TDataEntry entity)
		{
			DbSet<TDataEntry> dbSet = db.Set<TDataEntry>();
			db.Entry(entity).State = EntityState.Modified;
			db.SaveChanges();
			if (_customizeQueryOptions != null)
				entity = WithOptions(dbSet).Single(i => i.UID == entity.UID);
			else
				db.Entry(entity).Reload();
			return entity;
		}

		object IDataRepository.Update(object parameters)
		{
			return Update(parameters);
		}

		public TDataEntry Merge(object parameters)
		{
			throw new NotImplementedException();
		}

		public TDataEntry Merge(TDataEntry entity)
		{
			return Perform(db =>
			{
				DbSet<TDataEntry> dbSet = db.Set<TDataEntry>();
				TDataEntry currentEntry = dbSet.SingleOrDefault(i => i.UID == entity.UID);
				if (currentEntry == null)
					return Insert(db, entity);

				foreach (IDataEntryPropertyAccessor<TDataEntry> property in UpdatableProperties)
					property.CopyValue(entity, currentEntry);

				return Update(db, currentEntry);
			});
		}

		object IDataRepository.Merge(object parameters)
		{
			return Merge(parameters);
		}

		public TDataEntry Delete(object parameters)
		{
			throw new NotImplementedException();
		}

		public TDataEntry Delete(TDataEntry entity)
		{
			return Perform(db =>
			{
				db.DisableFilter(DataFilters.SoftDelete);
				DbSet<TDataEntry> dbSet = db.Set<TDataEntry>();
				if (db.Entry(entity).State == EntityState.Detached)
					dbSet.Attach(entity);
				dbSet.Remove(entity);
				db.SaveChanges();
				entity = WithOptions(dbSet).SingleOrDefault(i => i.UID == entity.UID) ?? entity;
				return entity;
			});
		}

		public TDataEntry Delete(Guid uid, Expression<Func<TDataEntry, bool>> predicate)
		{
			return Perform(db =>
			{
				db.DisableFilter(DataFilters.SoftDelete);
				predicate = predicate ?? (i => true);
				DbSet<TDataEntry> dbSet = db.Set<TDataEntry>();
				TDataEntry entry = dbSet.Where(predicate).SingleOrDefault(i => i.UID == uid);
				if (entry != null)
				{
					dbSet.Remove(entry);
					db.SaveChanges();
				}
				entry = WithOptions(dbSet).Where(predicate).SingleOrDefault(i => i.UID == uid) ?? entry;
				return entry;
			});
		}

		object IDataRepository.Delete(object parameters)
		{
			return Delete(parameters);
		}

		private TDataContext GetDataContext()
		{
			var dataContext = new TDataContext();
			DataRepositoryManager.Current.ApplyFilters(dataContext);
			return dataContext;
		}

		protected void Perform(Action<TDataContext> handler)
		{
			using (TDataContext dataContext = GetDataContext())
				handler(dataContext);
		}

		protected TResult Perform<TResult>(Func<TDataContext, TResult> handler)
		{
			using (TDataContext dataContext = GetDataContext())
				return handler(dataContext);
		}

		private Expression<Func<TDataEntry, bool>> GetExpression(object parameters)
		{
			ParameterExpression dataEntryParameter = Expression.Parameter(typeof(TDataEntry));

			Expression result = Expression.Constant(true);

			List<Tuple<string, MemberInfo, PropertyDescriptor>> tuples = typeof(TDataEntry).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty)
				.FullOuterJoin(TypeDescriptor.GetProperties(parameters).Cast<PropertyDescriptor>(), p => p.Name, p => p.Name, Tuple.Create)
				.Where(tuple => tuple.Item3 != null)
				.ToList();
			result = tuples
				.Aggregate(result, (expr, tuple) => Expression.AndAlso(expr, CreateExpression(dataEntryParameter, tuple.Item2, tuple.Item3.Name, tuple.Item3.GetValue(parameters))));
			return Expression.Lambda<Func<TDataEntry, bool>>(result, dataEntryParameter);
		}

		private Expression CreateExpression(ParameterExpression dataEntryParameter, MemberInfo memberInfo, string parameterName, object value)
		{
			if (memberInfo == null)
				throw new InvalidOperationException(string.Format("Field or property with name '{0}' doesn't defined for type '{1}'",
					parameterName, typeof(TDataEntry).FullName));

			var builder = value as IFilterExpressionBuilder ?? new FilterExpressionBuilder(FilterExpressionType.Equals, value);

			switch (memberInfo.MemberType)
			{
				case MemberTypes.Property:
					var propertyInfo = (PropertyInfo)memberInfo;
					value = CustomConverterHelper.Convert(builder.Value, propertyInfo.PropertyType);
					return builder.BuildExpression(Expression.Property(dataEntryParameter, propertyInfo), Expression.Constant(value, propertyInfo.PropertyType));
				case MemberTypes.Field:
					var fieldInfo = (FieldInfo)memberInfo;
					value = CustomConverterHelper.Convert(builder.Value, fieldInfo.FieldType);
					return builder.BuildExpression(Expression.Field(dataEntryParameter, fieldInfo), Expression.Constant(value, fieldInfo.FieldType));
				default:
					throw new NotSupportedException();
			}
		}

		private IEnumerable<TDataEntry> FindRange(int skipCount, int takeCount, Expression<Func<TDataEntry, bool>> @where = null,
			Func<IQueryable<TDataEntry>, IOrderedQueryable<TDataEntry>> orderBy = null)
		{
			return Perform(db =>
			{
				IQueryable<TDataEntry> dataSet = WithOptions(db.Set<TDataEntry>());
				if (where != null)
					dataSet = dataSet.Where(@where.Expand());
				if (orderBy != null)
					dataSet = orderBy(dataSet);
				else
					dataSet = dataSet.OrderBy(i => i.DocID);
				if (skipCount > 0)
					dataSet = dataSet.Skip(skipCount);
				if (takeCount != int.MaxValue)
					dataSet = dataSet.Take(takeCount);
				return dataSet.ToArray();
			});
		}

		private IQueryable<TDataEntry> WithOptions(IQueryable<TDataEntry> query)
		{
			return (_customizeQueryOptions ?? (q => q))(query);
		}

		internal void WithOptions(Func<IQueryable<TDataEntry>, IQueryable<TDataEntry>> customizeQueryOptions)
		{
			_customizeQueryOptions = customizeQueryOptions;
		}
	}

}