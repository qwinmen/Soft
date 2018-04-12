using System;
using System.Collections.Generic;
using System.Linq;
using Monolit.DataLayer.Model;
using Monolit.DataLayer.Model.dbo.Objects;
using Monolit.DataLayer.ORM.Repositories;
using Monolit.Interfaces.Common;
using Monolit.Interfaces.Contracts;
using ViageSoft.SystemServices.Contextual;
using Object = Monolit.Interfaces.Models.Objects.Object;

namespace Monolit.BusinessLayer.Objects
{
	public static class ObjectManager
	{
		private static IDataRepository<ObjectDao> ObjectDataRepository => GlobalContextManager.CurrentContext.Get<IDataRepository<ObjectDao>>();
		
		public static CommonOperationResultSet<Object> GetNameObjects(long revision)
		{
			using (DataRepositoryManager.Current.DisableFilter(DataFilters.SoftDelete))
				return new CommonOperationResultSet<Object>(
					ObjectDataRepository
						.FindAll(i => i.Revision > revision)
						.Select(i => i.ToDto()));
		}

		public static Object GetNameObjectByUid(Guid objUid)
		{
			return ObjectDataRepository.FindAll(i => i.UID == objUid).FirstOrDefault().ToDto();
		}

		public static CommentOperationResult UpdateNameObjects(Object objData)
		{
			Object result;
			if (Guid.Empty.Equals(objData.UID))
			{
				objData.UID = Guid.NewGuid();
				result = ObjectDataRepository.Insert(objData.ToDao()).ToDto();
			}
			else
			{
				result = Update(objData);
			}

			if (result == null)
				return new CommentOperationResult("Не удалось обновить информацию в таблице Objects.");

			return new CommentOperationResult(ObjectServiceOperationStatus.Success, result);
		}

		private static Object Update(Object entry)
		{
			var oldData = GetNameObjectByUid(entry.UID);
			var newData = new Object()
				{
					UID = oldData.UID,
					ID = oldData.ID,
					DocID = oldData.DocID,
					Revision = oldData.Revision,
					Name = entry.Name == oldData.Name ? oldData.Name : entry.Name,
					IsDeleted = entry.IsDeleted,
				};
			return ObjectDataRepository.Update(newData.ToDao()).ToDto();
		}
	}
}
