using System;
using AILogistics.Application.AI.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AILogistics.Infrastructure.AI.Groq;
using AILogistics.Infrastructure.AI.Gemini;
using Microsoft.Extensions.Logging;

namespace AILogistics.Infrastructure.AI
{
    public sealed class FallBackAiAgent : IAiAgent
    {
        private readonly GroqAgent _groqAgent;
        private readonly GeminiAgent _geminiAgent;
        private readonly ILogger<FallBackAiAgent> _logger;

        public FallBackAiAgent(GroqAgent groqAgent, GeminiAgent geminiAgent,ILogger<FallBackAiAgent> logger)
        {
            _groqAgent = groqAgent;
            _geminiAgent = geminiAgent;
            _logger = logger;
        }

        public async Task<string> AskAsync(string message, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _groqAgent.AskAsync(message, cancellationToken);
            }
            catch (HttpRequestException ex) when (ShouldFallback(ex))
            {
                _logger.LogWarning(ex, "Groq provider failed with status code {StatusCode}. Falling back to Gemini", ex.StatusCode);
                return await _geminiAgent.AskAsync(message, cancellationToken);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Groq provider timed out. Falling back to Gemini");
                return await _geminiAgent.AskAsync(message, cancellationToken);
            }
        }

        private static bool ShouldFallback(HttpRequestException exception)
        {
            if (!exception.StatusCode.HasValue)
            {
                return true;
            }

            int statusCode = (int)exception.StatusCode.Value;

            return statusCode == 429 || statusCode >= 500;
        }
    }
}
