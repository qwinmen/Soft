using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Monolit.DataLayer.Accessors
{
	public interface IDataEntryPropertyAccessor
	{
		string Name { get; }
		object GetValue(object dataEntry);
		void SetValue(object dataEntry, object value);
	}

	public interface IDataEntryPropertyAccessor<in TDataEntry> : IDataEntryPropertyAccessor
	{
		object GetValue(TDataEntry dataEntry);
		void SetValue(TDataEntry dataEntry, object value);
		void CopyValue(TDataEntry source, TDataEntry destination);
	}

	public interface IAccessorBuilder<in TDataEntry>
	{
		Action<TDataEntry, object> CreateSetAccessor(PropertyInfo property);
		Func<TDataEntry, object> CreateGetAccessor(PropertyInfo property);
	}

	public interface IAccessorBuilder<in TDataEntry, T> : IAccessorBuilder<TDataEntry>
	{
		new Action<TDataEntry, T> CreateSetAccessor(PropertyInfo property);
		new Func<TDataEntry, T> CreateGetAccessor(PropertyInfo property);
	}

	public class AccessorBuilder<TDataEntry, T> : IAccessorBuilder<TDataEntry, T>
	{
		Func<TDataEntry, object> IAccessorBuilder<TDataEntry>.CreateGetAccessor(PropertyInfo property)
		{
			Func<TDataEntry, T> accessor = CreateGetAccessor(property);
			return entry => accessor(entry);
		}

		Action<TDataEntry, object> IAccessorBuilder<TDataEntry>.CreateSetAccessor(PropertyInfo property)
		{
			Action<TDataEntry, T> accessor = CreateSetAccessor(property);
			return (entry, value) =>
			{
				accessor(entry, (T)value);
			};
		}

		public Func<TDataEntry, T> CreateGetAccessor(PropertyInfo property)
		{
			ParameterExpression instanceParameter = Expression.Parameter(typeof(TDataEntry));
			MemberExpression memberExpression = Expression.Property(instanceParameter, property);
			return Expression.Lambda<Func<TDataEntry, T>>(memberExpression, instanceParameter).Compile();
		}

		public Action<TDataEntry, T> CreateSetAccessor(PropertyInfo property)
		{
			ParameterExpression instanceParameter = Expression.Parameter(typeof(TDataEntry));
			MemberExpression memberExpression = Expression.Property(instanceParameter, property);
			ParameterExpression valueParameter = Expression.Parameter(property.PropertyType);
			BinaryExpression assign = Expression.Assign(memberExpression, valueParameter);
			BlockExpression block = Expression.Block(/*new[] { instanceParameter, valueParameter }, */assign);
			Func<TDataEntry, T, T> func = Expression.Lambda<Func<TDataEntry, T, T>>(block, instanceParameter, valueParameter).Compile();
			return (dataEntry, value) => func(dataEntry, value);
		}
	}

	public class DataEntryPropertyAccessor<TDataEntry> : IDataEntryPropertyAccessor<TDataEntry>
	{
		private readonly Func<TDataEntry, object> _getAccessor;
		private readonly Action<TDataEntry, object> _setAccessor;

		public DataEntryPropertyAccessor(PropertyInfo property)
		{
			Name = property.Name;
			var builder =
				(IAccessorBuilder<TDataEntry>)Activator.CreateInstance(typeof(AccessorBuilder<,>).MakeGenericType(typeof(TDataEntry), property.PropertyType));
			_getAccessor = builder.CreateGetAccessor(property);
			_setAccessor = builder.CreateSetAccessor(property);
		}

		public string Name { get; private set; }

		object IDataEntryPropertyAccessor.GetValue(object dataEntry)
		{
			return GetValue((TDataEntry)dataEntry);
		}

		void IDataEntryPropertyAccessor.SetValue(object dataEntry, object value)
		{
			SetValue((TDataEntry)dataEntry, value);
		}

		public object GetValue(TDataEntry dataEntry)
		{
			return _getAccessor(dataEntry);
		}

		public void SetValue(TDataEntry dataEntry, object value)
		{
			_setAccessor(dataEntry, value);
		}

		public void CopyValue(TDataEntry source, TDataEntry destination)
		{
			SetValue(destination, GetValue(source));
		}
	}

	public interface IDataEntryAccessor
	{
		IList<IDataEntryPropertyAccessor> Properties { get; }
	}

	public interface IDataEntryAccessor<TDataEntry> : IDataEntryAccessor
	{
		new IList<IDataEntryPropertyAccessor<TDataEntry>> Properties { get; }
	}

	public class DataEntryAccessor<TDataEntry> : IDataEntryAccessor<TDataEntry>
	{
		private readonly IList<IDataEntryPropertyAccessor<TDataEntry>> _properties;

		public DataEntryAccessor()
		{
			_properties = typeof(TDataEntry).GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Select(p => new DataEntryPropertyAccessor<TDataEntry>(p))
				.Cast<IDataEntryPropertyAccessor<TDataEntry>>()
				.ToList();
		}

		IList<IDataEntryPropertyAccessor> IDataEntryAccessor.Properties => _properties.Cast<IDataEntryPropertyAccessor>().ToArray();

		public IList<IDataEntryPropertyAccessor<TDataEntry>> Properties => new ReadOnlyCollection<IDataEntryPropertyAccessor<TDataEntry>>(_properties);
	}

}