using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiOne
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config => // [episode-9 15:30] let's name it "Bearer" so that we can link it to our authentication
                {
                    // config.MetadataAddress // the default value for this is /.well-known/openid-configuration

                    //============================================================================================
                    // we are going to tell our client API where to pass access token to validate
                    // the URL from our IdentityServer project -> Properties -> Debug
                    config.Authority = "https://localhost:44375/";

                    // Audience is set to the same name as the one in IdentityServer.Configuration.GetApis()
                    config.Audience = "ApiOne";

                    // [episode-9 18:00] Authority and Audience identifies the API that will validate our token
                    //============================================================================================
                });

            services.AddControllers(); // [episode-9 14:30] no views
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
