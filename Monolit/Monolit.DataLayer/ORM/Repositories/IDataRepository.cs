using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ViageSoft.SystemServices.Collections;

namespace Monolit.DataLayer.ORM.Repositories
{
	public interface IDataRepository<TEntity> : IDataRepository
	{
		new TEntity FindSingle(Guid uid);
		new TEntity FindSingle(long docID);
		new TEntity FindSingle(object parameters);
		new TEntity FindFirst(object parameters);
		new IEnumerable<TEntity> FindFirst(int count, object parameters);
		new IEnumerable<TEntity> FindNext(int startIndex, int count, object parameters);
		new IEnumerable<TEntity> FindAll(object parameters);
		new IEnumerable<TEntity> FindAll();
		IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> filterExpression);
		new ResultSet<TEntity> FindPage(int pageNumber, int pageSize, object parameters);
		ResultSet<TEntity> FindPage(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> filterExpression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

		new TEntity Insert(object parameters);
		TEntity Insert(TEntity entity);
		new TEntity Update(object parameters);
		TEntity Update(TEntity entity);
		new TEntity Merge(object parameters);
		TEntity Merge(TEntity entity);
		new TEntity Delete(object parameters);
		TEntity Delete(TEntity entity);
		TEntity Delete(Guid uid, Expression<Func<TEntity, bool>> predicate = null);
	}

	public interface IDataRepository
	{
		object FindSingle(Guid uid);
		object FindSingle(long docID);
		object FindSingle(object parameters);
		object FindFirst(object parameters);
		IEnumerable FindFirst(int count, object parameters);
		IEnumerable FindNext(int startIndex, int count, object parameters);
		IEnumerable FindAll(object parameters);
		IEnumerable FindAll();
		ResultSet FindPage(int pageNumber, int pageSize, object parameters);

		object Insert(object parameters);
		object Update(object parameters);
		object Merge(object parameters);
		object Delete(object parameters);
	}
}