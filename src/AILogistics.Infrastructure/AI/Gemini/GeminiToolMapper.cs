using AILogistics.Application.AI.Abstractions;
using Google.GenAI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.AI.Gemini
{
    internal static class GeminiToolMapper
    {
        public static FunctionDeclaration Map(IAiTool tool)
        {
            var schemaJson = JsonSerializer.Serialize(tool.GetParametersSchema());

            var schema = JsonSerializer.Deserialize<Schema>(schemaJson)
                ?? throw new InvalidOperationException($"Could not map schema for AI tool '{tool.Name}'");

            return new FunctionDeclaration
            {
                Name = tool.Name,
                Description = tool.Description,
                Parameters = schema
            };
        }
    }
}
