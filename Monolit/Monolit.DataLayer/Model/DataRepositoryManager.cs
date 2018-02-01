using EntityFramework.DynamicFilters;
using ViageSoft.SystemServices.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Monolit.DataLayer.Model
{
	public interface IDataRepositoryManager
	{
		IDisposable DisableFilter(string filterName);
		void ApplyFilters(DbContext context);
	}

	public class DataRepositoryManager : IDataRepositoryManager
	{
		private interface IDisabledFilter : IDisposable
		{
			void ApplyFilter(DbContext context);
		}

		private class DisabledFilter : DisposableBase, IDisabledFilter
		{
			private readonly DataRepositoryManager _manager;
			private readonly string _filterName;

			public DisabledFilter(DataRepositoryManager manager, string filterName)
			{
				_manager = manager;
				_filterName = filterName;
			}

			protected override void InternalDispose()
			{
				_manager._disabledFilters.Remove(this);
			}

			public void ApplyFilter(DbContext context)
			{
				context.DisableFilter(_filterName);
			}
		}

		[ThreadStatic]
		private static IDataRepositoryManager __current;

		private readonly List<IDisabledFilter> _disabledFilters = new List<IDisabledFilter>();

		private DataRepositoryManager()
		{
		}

		public static IDataRepositoryManager Current => __current ?? (__current = new DataRepositoryManager());

		public IDisposable DisableFilter(string filterName)
		{
			IDisabledFilter result = new DisabledFilter(this, filterName);
			_disabledFilters.Add(result);
			return result;
		}

		public void ApplyFilters(DbContext context)
		{
			_disabledFilters.ForEach(filter => filter.ApplyFilter(context));
		}
	}

}