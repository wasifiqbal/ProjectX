using ProjectX.Core.Commons.Abstraction;
using ProjectX.Core.Commons.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProjectX.Core.Tenants
{
	public class Tenant : BaseEntity<int>, ICreationTime, IModificationTime, IDeletion
	{
		[Required, MaxLength(20), MinLength(3)]
		public string Code { get; set; } = string.Empty;
		[Required, MaxLength(256), MinLength(3)]
		public string Name { get; set; } = String.Empty;
		public bool IsActive { get; set; } = true;
		public DateTime CreationTime { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? DeletionTime { get; set; }
		public DateTime? ModificationTime { get; set; }
	}
}
