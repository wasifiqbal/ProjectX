using System.ComponentModel.DataAnnotations;

namespace ProjectX.Core.Tenants
{
    public class Tenant
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20), MinLength(3)]
        public string Code { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(3)]
        public string Name { get; set; } = String.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public DateTime? ModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
