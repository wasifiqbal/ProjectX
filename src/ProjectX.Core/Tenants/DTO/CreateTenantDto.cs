using System.ComponentModel.DataAnnotations;

namespace ProjectX.Core.Tenants.DTO
{
	public class CreateTenantDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Code { get; set; } = string.Empty;
	}
}
