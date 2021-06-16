using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleReservations.Command.Api.Models;
using VehicleReservations.Command.ApplicationServices.Feature;

namespace VehicleReservations.Command.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class ReservationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationsController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Criar nova reserva de veiculo para determinado cliente.
        /// </summary>
        /// <param name="request">Modelo de request para criar nova reserva.</param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="422">Unprocessable Entity</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="503">Service Unavailable</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateReserveCommand), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateReserveAsync([FromBody] CreateReserveCommand request)
        {
            await _mediator.Send(request);
            return Created(string.Empty, request);
        }

        /// <summary>
        /// Cancelar reserva de veiculo existente de determinado cliente.
        /// </summary>
        /// <param name="reserveId">Identificação da reserva no sistema.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="503">Service Unavailable</response>
        [HttpPatch("{reserveId:Guid}/cancel")]
        [ProducesResponseType(typeof(CancelReserveCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CancelReserveAsync([FromRoute] Guid reserveId)
        {
            await _mediator.Send(new CancelReserveCommand(reserveId));
            return Ok();
        }

        /// <summary>
        /// Renovar reserva de veiculo existente de determinado cliente.
        /// </summary>
        /// <param name="reserveId">Identificação da reserva no sistema.</param>
        /// <param name="days">Número de dias para renovação da reserva.</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="503">Service Unavailable</response>
        [HttpPatch("{reserveId:Guid}/renew/{days:int}")]
        [ProducesResponseType(typeof(RenewReserveCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> RenewReserveAsync([FromRoute] Guid reserveId, [FromRoute] int days)
        {
            await _mediator.Send(new RenewReserveCommand(reserveId, days));
            return Ok();
        }
    }
}
