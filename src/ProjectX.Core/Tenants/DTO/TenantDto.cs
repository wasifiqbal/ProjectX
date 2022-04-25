namespace ProjectX.Core.Tenants.DTO
{
	public class TenantDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Code { get; set; } = string.Empty;
		public DateTime CreationTime { get; set; } = DateTime.UtcNow;
	}
}
