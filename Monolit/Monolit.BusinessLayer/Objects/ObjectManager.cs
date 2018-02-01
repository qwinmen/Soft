using System;
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

		public static CommonOperationResultSet<Object> GetObjects(long revision)
		{
			using (DataRepositoryManager.Current.DisableFilter(DataFilters.SoftDelete))
				return new CommonOperationResultSet<Object>(
					ObjectDataRepository
						.FindAll(i => i.Revision > revision)
						.Select(i => i.ToDto()));
		}

		public static Object GetObjectByUid(Guid objUid)
		{
			return ObjectDataRepository.FindAll(i => i.UID == objUid).FirstOrDefault().ToDto();
		}

		public static CommentOperationResult UpdateObjects(Object objData)
		{
			Object result;
			if (Guid.Empty.Equals(objData.UID))
			{
				objData.UID = Guid.NewGuid();
				result = ObjectDataRepository.Insert(objData.ToDao()).ToDto();
			}
			else
			{
				result = ObjectDataRepository.Update(objData.ToDao()).ToDto();
			}

			if (result == null)
				return new CommentOperationResult("Не удалось обновить информацию в таблице Objects.");

			return new CommentOperationResult(ObjectServiceOperationStatus.Success, null);
		}
	}
}
