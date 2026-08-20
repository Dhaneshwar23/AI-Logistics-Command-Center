using AILogistics.Application.AI.Abstractions;
using AILogistics.Application.AI.Models;
using AILogistics.Infrastructure.AI.Groq;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AILogistics.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ai")]
    //[Authorize]
    public class AiController : ControllerBase
    {
        private readonly IAiAgent _aiAgent;
        private readonly GroqAgent _groqAgent;
        public AiController(IAiAgent aiAgent, GroqAgent groqAgent)
        {
            _aiAgent = aiAgent;
            _groqAgent = groqAgent;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask(AiChatRequest request, CancellationToken cancellationToken)
        {
            var res = await _aiAgent.AskAsync(request.Message, cancellationToken);

            return Ok(new { res });
        }

    }
}
