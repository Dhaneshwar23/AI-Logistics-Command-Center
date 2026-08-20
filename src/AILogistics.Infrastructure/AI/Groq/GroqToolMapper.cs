using AILogistics.Application.AI.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.AI.Groq
{
    internal static class GroqToolMapper
    {
        public static object Map(IAiTool tool)
        {
            return new
            {
                type = "function",
                function = new
                {
                    name = tool.Name,
                    description = tool.Description,
                    parameters = tool.GetParametersSchema()
                }
            };
        }
    }
}
