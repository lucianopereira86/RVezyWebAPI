using Microsoft.Extensions.Configuration;
using Serilog;

namespace RVezy.Common.DI
{
    public class Logging
    {
        public static void Init(IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }
    }
}
