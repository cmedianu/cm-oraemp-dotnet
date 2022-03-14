namespace OraEmp.Infrastructure.Services
{
    public class ConfigOptions
    {
        public string ConnectionString { get; set; } = String.Empty;
        public bool IsDevelopment { get; set; } = false;
    }
}