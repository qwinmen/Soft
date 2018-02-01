using ViageSoft.SystemServices.Extensions;
using Monolit.DataLayer.Model.dbo.Objects;
using Object = Monolit.Interfaces.Models.Objects.Object;

namespace Monolit.BusinessLayer.Objects
{
	public static class ObjectHelpers
	{
		public static Object ToDto(this ObjectDao source)
		{
			return source.Safe(src => new Object
			{
				IsDeleted = src.IsDeleted,
				UID = src.UID,
				Revision = src.Revision,
				ID = src.ID,
				DocID = src.DocID,
				Name = src.Name,

			});
		}

		public static ObjectDao ToDao(this Object source)
		{
			ObjectDao objectDao = null;
			if (source != null)
			{
				objectDao = new ObjectDao
				{
					UID = source.UID,
					Name = source.Name,

				};
			}

			return objectDao;
		}
	}
}
