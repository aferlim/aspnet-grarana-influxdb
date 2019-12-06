// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using App.Metrics;
// using App.Metrics.AspNetCore;
// using App.Metrics.Filtering;


using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Filtering;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CoreGrafana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // var filter = new MetricsFilter();
            // filter.WhereContext(c => c == MetricsRegistry.Context);

            return Host.CreateDefaultBuilder(args)
                        .ConfigureMetricsWithDefaults(builder =>
                            {
                                //builder.Filter.With(filter);
                                builder.Report.ToInfluxDb("http://localhost:8086", "metricasnetcore", TimeSpan.FromSeconds(1));
                            })
                            .UseMetrics()
                            .UseMetricsWebTracking()
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                            });
        }

    }
}
