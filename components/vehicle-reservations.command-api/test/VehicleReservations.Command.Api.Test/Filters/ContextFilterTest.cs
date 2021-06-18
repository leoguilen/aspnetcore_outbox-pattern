using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using VehicleReservations.Command.Api.Controllers;
using VehicleReservations.Command.Api.Filters;
using VehicleReservations.Command.Core.Holders;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using Xunit;

namespace VehicleReservations.Command.Api.Test.Filters
{
    public class ContextFilterTest
    {
        private const string ControllerRoute = "/api/v1/reservations";
        private readonly Mock<ILogWriter> _logWriter;
        private readonly Mock<IRequestContextHolder> _requestContextHolder;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IMediator> _mediator;

        public ContextFilterTest()
        {
            _logWriter = new(MockBehavior.Strict);
            _mediator = new(MockBehavior.Strict);
            _requestContextHolder = new(MockBehavior.Loose);
            _configuration = new(MockBehavior.Strict);
        }

        [Theory, AutoData]
        public void OnActionExecuting_WhenRequestReceived_ThenConfigureLogContext(string applicationName, Guid correlationId)
        {
            // Arrange
            var controller = GetController();
            var action = GetActionExecutingContext(controller, correlationId);
            var controllerName = (string)action.HttpContext.Request.RouteValues["controller"];
            var operation = (string)action.HttpContext.Request.RouteValues["action"];
            var traceId = action.HttpContext.TraceIdentifier;
            _configuration
                .SetupGet(x => x["AppSettings:Application"])
                .Returns(applicationName);
            _logWriter.Setup(x => x.SetupLogger(controllerName, operation, correlationId, traceId));
            var sut = new ContextFilter(_logWriter.Object, _requestContextHolder.Object, _configuration.Object);

            // Act
            sut.OnActionExecuting(action);

            // Assert
            Mock.VerifyAll(_logWriter, _configuration, _requestContextHolder);
        }

        private static ActionExecutingContext GetActionExecutingContext(
            object controller,
            Guid correlationId)
        {
            var request = new Mock<HttpRequest>(MockBehavior.Strict);
            request.Setup(r => r.Headers)
            .Returns(new HeaderDictionary(new Dictionary<string, StringValues>()
            {
                { "x-correlation-id", new StringValues(correlationId.ToString()) },
            }));

            var context = new Mock<HttpContext>(MockBehavior.Strict);
            context
                .Setup(c => c.Request)
                .Returns(request.Object);
            context
                .Setup(c => c.Request.RouteValues)
                .Returns(new RouteValueDictionary(new
                {
                    controller = nameof(ReservationsController),
                    action = "GET",
                }));
            context
                .SetupGet(c => c.TraceIdentifier)
                .Returns(Guid.NewGuid().ToString());

            return new ActionExecutingContext(
                new ActionContext(
                    context.Object,
                    new RouteData(),
                    new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller);
        }

        private ReservationsController GetController()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = ControllerRoute;
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext,
            };

            return new ReservationsController(_mediator.Object)
            {
                ControllerContext = controllerContext,
            };
        }
    }
}
