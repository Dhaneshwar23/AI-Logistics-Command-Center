using AILogistics.Application.AI;
using AILogistics.Application.AI.Abstractions;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.AI.Gemini
{
    public sealed class GeminiAgent : IAiAgent
    {
        private readonly GeminiOptions _options;
        private readonly Client _client;
        private readonly IEnumerable<IAiTool> _tools;

        public GeminiAgent(IOptions<GeminiOptions> options, IEnumerable<IAiTool> tools)
        {
            _options = options.Value;
            _tools = tools;
            _client = new Client(apiKey: _options.ApiKey);
        }

        public async Task<string> AskAsync(string message, CancellationToken cancellationToken = default)
        {
            const int maxIterations = 5;


            var functionDeclarations = _tools
                .Select(GeminiToolMapper.Map)
                .ToList();

            var config = new GenerateContentConfig
            {
                SystemInstruction = new Content
                {
                    Parts = new List<Part>
                    {
                        new Part
                        {
                            Text = AiSystemPrompt.Default
                        }
                    }
                },
                Tools = new List<Tool>
                {
                    new Tool
                    {
                        FunctionDeclarations = functionDeclarations,
                    }
                }
            };

            var contents = new List<Content>
            {
                new Content
                {
                    Role="user",
                    Parts = new List<Part>
                    {
                        new Part
                        {
                            Text = message
                        }
                    }
                }
            };

            for (var iteration = 0; iteration < maxIterations; iteration++)
            {
                var response = await _client.Models.GenerateContentAsync(
                    model: _options.Model,
                    contents: contents,
                    config: config,
                    cancellationToken: cancellationToken
                    );

                var modelContent = response.Candidates?
                    .FirstOrDefault()?
                    .Content;

                if (modelContent is not null)
                {
                    contents.Add(modelContent);
                }

                var functionCall = response.Candidates?
                    .FirstOrDefault()?
                    .Content?
                    .Parts?
                    .FirstOrDefault(p => p.FunctionCall != null)?
                    .FunctionCall;


                if (functionCall is null)
                {
                    return response.Text ?? string.Empty;
                }

                var tool = _tools.FirstOrDefault(t => string.Equals(t.Name, functionCall?.Name, StringComparison.OrdinalIgnoreCase));

                if (tool is null)
                {
                    throw new InvalidOperationException($"AI requested unknown tool '{functionCall?.Name}' ");
                }

                var arguments = functionCall?.Args ?? new Dictionary<string, object>();

                var toolResult = await tool.ExecuteAsync(arguments, cancellationToken);

                //var toolResultJson = JsonSerializer.Serialize(toolResult);

                var toolResultDictionary = new Dictionary<string, object>
                {
                    ["result"] = toolResult ?? new { }
                };

                //using var jsonDocument = JsonDocument.Parse(toolResultJson);

                var functionResponseContent = new Content
                {
                    Role = "user",
                    Parts = new List<Part>
                    {
                        new Part
                        {
                            FunctionResponse = new FunctionResponse
                            {
                                Name = functionCall?.Name,
                                Response = toolResultDictionary
                            }
                        }
                    }
                };

                contents.Add(functionResponseContent);

            }

            throw new InvalidOperationException("AI agent exceeeded the maximum number of tool iterations");
        }
    }
}
