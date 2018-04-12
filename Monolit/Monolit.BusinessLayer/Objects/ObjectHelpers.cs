using System;
using System.Linq;
using Monolit.DataLayer.Model.dbo.Objects;
using ViageSoft.SystemServices.Extensions;

namespace Monolit.BusinessLayer.Objects
{
	public static class ObjectHelpers
	{
		public static Interfaces.Models.Objects.Object ToDto(this ObjectDao source)
		{
			return source.Safe(src => new Interfaces.Models.Objects.Object
				{
					IsDeleted = src.IsDeleted,
					UID = src.UID,
					Revision = src.Revision,
					ID = src.ID,
					DocID = src.DocID,
					Name = src.Name,
				});
		}

		public static ObjectDao ToDao(this Interfaces.Models.Objects.Object source)
		{
			ObjectDao objectDao = null;
			if (source != null)
			{
				objectDao = new ObjectDao
					{
						UID = source.UID,
						Name = source.Name,
						IsDeleted = source.IsDeleted,
						ID = source.ID,
						DocID = source.DocID,
						Revision = source.Revision,
					};
			}

			return objectDao;
		}
	}
}
