using AILogistics.Api.Extensions;
using AILogistics.Application.DTOs.Dashboard;
using AILogistics.Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AILogistics.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/dashboard")]
    [ApiVersion("1.0")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public DashboardController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [OutputCache(PolicyName = OutputCachingExtensions.GeneralPolicy,
            Tags = new[] { OutputCachingExtensions.ShipmentTag })]
        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryResponseDto>> GetSummary()
        {
            var res = await _shipmentService.GetDashboardSummaryAsync();

            return Ok(res);
        }
    }
}
