using API.Controllers;
using Application.DTOs.Employees;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Swagger;

public sealed class RequestExamplesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(CreateEmployeeRequest) || context.Type == typeof(UpdateEmployeeRequest))
        {
            SetExample(schema, "name", new OpenApiString("John Doe"));
            SetExample(schema, "currentPosition", new OpenApiInteger(1));
            SetExample(schema, "salary", new OpenApiDouble(3000));
            SetExample(schema, "departmentId", new OpenApiInteger(1));

            SetEnum(schema, "currentPosition", new object[] { 1, 2, 3, 4 });
            SetEnum(schema, "departmentId", new object[] { 1, 2, 3 });
        }

        if (context.Type == typeof(AssignProjectRequest))
        {
            SetExample(schema, "projectIds", new OpenApiArray
            {
                new OpenApiInteger(1),
                new OpenApiInteger(2),
                new OpenApiInteger(3)
            });
        }

        if (context.Type == typeof(LoginRequest))
        {
            SetExample(schema, "username", new OpenApiString("admin"));
            SetExample(schema, "password", new OpenApiString("Admin123!"));
        }

        if (context.Type == typeof(RegisterRequest))
        {
            SetExample(schema, "username", new OpenApiString("newuser"));
            SetExample(schema, "password", new OpenApiString("User123!"));
            SetExample(schema, "role", new OpenApiString("User"));

            SetEnum(schema, "role", new object[] { "Admin", "User" });
        }
    }

    private static void SetExample(OpenApiSchema schema, string propertyName, IOpenApiAny example)
    {
        if (schema.Properties.TryGetValue(propertyName, out var propertySchema))
        {
            propertySchema.Example = example;
        }
    }

    private static void SetEnum(OpenApiSchema schema, string propertyName, IEnumerable<object> values)
    {
        if (!schema.Properties.TryGetValue(propertyName, out var propertySchema))
        {
            return;
        }

        propertySchema.Enum.Clear();

        foreach (var value in values)
        {
            propertySchema.Enum.Add(value switch
            {
                int intValue => new OpenApiInteger(intValue),
                string stringValue => new OpenApiString(stringValue),
                _ => new OpenApiString(value.ToString() ?? string.Empty)
            });
        }
    }
}
