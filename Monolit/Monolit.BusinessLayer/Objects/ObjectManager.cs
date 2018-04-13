using System;
using System.Linq;
using Monolit.DataLayer.Model;
using Monolit.DataLayer.Model.dbo.Objects;
using Monolit.DataLayer.ORM.Repositories;
using Monolit.Interfaces.Common;
using Monolit.Interfaces.Contracts;
using ViageSoft.SystemServices.Contextual;

namespace Monolit.BusinessLayer.Objects
{
	public static class ObjectManager
	{
		private static IDataRepository<ObjectDao> ObjectDataRepository => GlobalContextManager.CurrentContext.Get<IDataRepository<ObjectDao>>();

		public static CommonOperationResultSet<Interfaces.Models.Objects.Object> GetNameObjects(long revision)
		{
			using (DataRepositoryManager.Current.DisableFilter(DataFilters.SoftDelete))
				return new CommonOperationResultSet<Interfaces.Models.Objects.Object>(
					ObjectDataRepository
						.FindAll(i => i.Revision > revision && !i.IsDeleted)
						.Select(i => i.ToDto()));
		}

		public static Interfaces.Models.Objects.Object GetNameObjectByUid(Guid objUid)
		{
			return ObjectDataRepository.FindAll(i => i.UID == objUid && !i.IsDeleted).FirstOrDefault().ToDto();
		}

		public static CommentOperationResult UpdateNameObjects(Interfaces.Models.Objects.Object objData)
		{
			Interfaces.Models.Objects.Object result;
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

		private static Interfaces.Models.Objects.Object Update(Interfaces.Models.Objects.Object entry)
		{
			var oldData = GetNameObjectByUid(entry.UID);
			var newData = new Interfaces.Models.Objects.Object()
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

		/// <summary>
		///     Выставит флаг IsDeleted
		/// </summary>
		/// <param name="objUID"></param>
		/// <returns></returns>
		public static CommentOperationResult Delete(Guid objUID)
		{
			ObjectDao result = ObjectDataRepository.Delete(objUID);
			return new CommentOperationResult(result == null ? ObjectServiceOperationStatus.Fail : ObjectServiceOperationStatus.Success, result.ToDto());
		}
	}
}
