using Asp.Versioning.ApiExplorer;

namespace AILogistics.Api.Extensions
{
    public static class SwaggerApplicationExtensions
    {
        public static WebApplication UseSwaggerDocumentation(this WebApplication app) 
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                var provider = app.Services
                .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach(var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"AI Logistics API {description.GroupName.ToUpperInvariant()}"
                        );
                }
            });

            return app;
        }
    }
}
