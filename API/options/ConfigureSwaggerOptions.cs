using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
// gives access to OpenApiInfo (title, version, description of a swagger doc).
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.options
{

    //This tells ASP.NET Core: "When SwaggerGenOptions is being built, call my Configure() method."
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        // the list of all discovered API versions in your app will be stored in this field.
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;
        
        //ASP.NET Core calls this automatically when building Swagger, passing in the SwaggerGenOptions to configure.
        public void Configure(SwaggerGenOptions options)
        {
            //each description represents one version (e.g., v1, v2) 
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                //the header metadata for each version's Swagger doc
                var info = new OpenApiInfo
                {
                    Title = $"WorkFlow API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = "An API to manage a project workflow.",
                };
                if (description.IsDeprecated)
                    info.Description += " This API version has been deprecated.";
                //register a Swagger doc for this API version, using the GroupName as the doc name (e.g., "v1", "v2").
                options.SwaggerDoc(description.GroupName, info);
            }
            //filter rule that controls which endpoints appear in which Swagger doc
            // this tells Swagger to only include API endpoints in a given doc if their GroupName matches the doc name (e.g., only include endpoints with GroupName "v1" in the "v1" doc).
            options.DocInclusionPredicate((docName, apiDesc) => apiDesc.GroupName == docName);
        }
    }
}
