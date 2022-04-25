using System.ComponentModel.DataAnnotations;

namespace ProjectX.Core.Tenants.DTO
{
	public class UpdateTenantDto
	{
		[Range(1, int.MaxValue)]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Code { get; set; } = string.Empty;
	}
}
