using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreGrafana
{
    public class Startup
    {
        public static readonly Uri ApiBaseAddress = new Uri("http://localhost:5002/");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddMetrics();
            //services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMetricsAllEndpoints();
            app.UseMetricsAllMiddleware();

            RunRequestsToSample();
        }

        private static void RunRequestsToSample()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = ApiBaseAddress
            };

            var requestSamplesScheduler = new AppMetricsTaskScheduler(TimeSpan.FromMilliseconds(10), async () =>
            {

                var Tasks = new List<Task> { };

                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=1"));
                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=2"));
                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=3"));
                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=4"));
                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=5"));
                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=6"));
                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=7"));
                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=8"));
                Tasks.Add(httpClient.GetStringAsync("/WeatherForecast/hotfound?v=9"));

                // var uniform = httpClient.GetStringAsync("api/reservoirs/uniform");
                // var exponentiallyDecaying = httpClient.GetStringAsync("api/reservoirs/exponentially-decaying");
                // var exponentiallyDecayingLowWeight = httpClient.GetStringAsync("api/reservoirs/exponentially-decaying-low-weight");
                // var slidingWindow = httpClient.GetStringAsync("api/reservoirs/sliding-window");

                await Task.WhenAll(Tasks.ToArray());

                // await Task.WhenAll(uniform, exponentiallyDecaying, exponentiallyDecayingLowWeight, slidingWindow);
            });

            requestSamplesScheduler.Start();
        }
    }
}
