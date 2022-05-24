using ProjectX.Core.Commons.Abstraction;
using ProjectX.Core.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Data.EFCore.CoreEntity
{
	public class EntityHistory : BaseEntity<long>, ICreationTime
	{
		public string EntityName { get; set; }
		public string EntityId { get; set; }
		public string Detail { get; set; }
		public int? UserId { get; set; }
		public DateTime CreationTime { get; set; }
	}
}
