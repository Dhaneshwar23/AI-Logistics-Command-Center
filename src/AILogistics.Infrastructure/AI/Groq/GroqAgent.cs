using AILogistics.Application.AI;
using AILogistics.Application.AI.Abstractions;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.AI.Groq
{
    public class GroqAgent : IAiAgent
    {
        private readonly HttpClient _httpClient;
        private readonly GroqOptions _options;
        private readonly IEnumerable<IAiTool> _tools;


        public GroqAgent(HttpClient httpClient, IOptions<GroqOptions> options, IEnumerable<IAiTool> tools)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _tools = tools;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                _options.ApiKey
                );
        }

        public async Task<string> AskAsync(string message, CancellationToken cancellationToken = default)
        {
            const int maxIterations = 5;

            var tools = _tools
                .Select(GroqToolMapper.Map)
                .ToList();

            var messages = new List<object>
            {
                new
                {
                    role = "system",
                    content = AiSystemPrompt.Default
                },
                new
                {
                    role = "user",
                    content = message
                }
            };

            for (var iteration = 0; iteration < maxIterations; iteration++)
            {
                var requestBody = new
                {
                    model = _options.Model,
                    messages = messages,
                    tools = tools,
                    tool_choice = "auto"
                };

                var json = JsonSerializer.Serialize(requestBody);

                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "chat/completions"
                    );

                request.Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                    );

                using var response = await _httpClient.SendAsync(request, cancellationToken);

                response.EnsureSuccessStatusCode();

                var responsejson = await response.Content.ReadAsStringAsync(cancellationToken);

                var groqResponse = JsonSerializer.Deserialize<GroqChatResponse>(
                    responsejson);

                var choice = groqResponse?
                    .Choices
                    .FirstOrDefault();

                var toolCall = choice?
                    .Message
                    .ToolCalls?
                    .FirstOrDefault();

                if (toolCall is null)
                {
                    return choice?.Message.Content ?? string.Empty;
                }

                var tool = _tools.FirstOrDefault(t => string.Equals(t.Name, toolCall.Function.Name, StringComparison.OrdinalIgnoreCase));

                if (tool is null)
                {
                    throw new InvalidOperationException($"AI requested unknown tool '{toolCall.Function.Name}' .");
                }

                var arguments =
                       JsonSerializer.Deserialize<Dictionary<string, object>>(toolCall.Function.Arguments) ??
                       new Dictionary<string, object>();

                var toolResult = await tool.ExecuteAsync(arguments, cancellationToken);

                messages.Add(new
                {
                    role = "assistant",
                    content = (string?)null,
                    tool_calls = new[]
                    {
                        new
                        {
                            id = toolCall.Id,
                            type= toolCall.Type,
                            function = new
                            {
                                name = toolCall.Function.Name,
                                arguments = toolCall.Function.Arguments,
                            }
                        }
                    }
                });

                var toolResultJson = JsonSerializer.Serialize(toolResult);

                messages.Add(new
                {
                    role = "tool",
                    tool_call_id = toolCall.Id,
                    content = toolResultJson
                });
            }

            throw new InvalidOperationException("AI agent exceeded the maximum number of tool iterations. ");

            //return responsejson;
        }

    }
}
