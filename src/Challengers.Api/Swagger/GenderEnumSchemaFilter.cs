using Challengers.Domain.Enums;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Challengers.Api.Swagger;

public class GenderEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Gender))
        {
            schema.Description = $"1 = {GetMessage(Gender_Male)}, 2 = {GetMessage(Gender_Female)}";
        }
    }
}
