using System;

namespace Hides.Infrastructure.Persistence.Util
{
    public class ConfigOptions
    {
        public string ConnectionString { get; set; } = String.Empty;
        public bool IsDevelopment { get; set; } = false;
    }
}