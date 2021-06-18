using System.Collections.Generic;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using VehicleReservations.Command.Api.Controllers;
using VehicleReservations.Command.Api.Filters;
using VehicleReservations.Command.Api.Models;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Core.Notifications;
using Xunit;

namespace VehicleReservations.Command.Api.Test.Filters
{
    public class RequestFilterTest
    {
        private const string ControllerRoute = "/api/v1/reservations";
        private readonly Mock<ILogWriter> _logWriter;
        private readonly Mock<INotification> _notifications;
        private readonly Mock<MediatR.IMediator> _mediator;

        public RequestFilterTest()
        {
            _logWriter = new(MockBehavior.Strict);
            _notifications = new(MockBehavior.Strict);
            _mediator = new(MockBehavior.Strict);
        }

        [Fact]
        public void OnActionExecuting_WhenRequestReceived_ThenLogRequest()
        {
            // Arrange
            var controller = GetController();
            var controllerName = controller.GetType().FullName;
            var action = GetActionExecutingContext(controller);
            _logWriter.Setup(lw => lw.Info("OnActionExecuting", null, null, null));
            var sut = new RequestFilter(_logWriter.Object, _notifications.Object);

            // Act
            sut.OnActionExecuting(action);

            // Assert
            _logWriter.VerifyAll();
        }

        [Fact]
        public void OnActionExecuted_WhenRequestExecutedWithoutNotification_ThenLogResponse()
        {
            // Arrange
            var controller = GetController();
            var controllerName = controller.GetType().FullName;
            var action = GetActionExecutedContext(controller);
            _notifications.Setup(n => n.Any()).Returns(false);
            _logWriter.Setup(x => x.Info("OnActionExecuted", StatusCodes.Status200OK, null, null));
            var sut = new RequestFilter(_logWriter.Object, _notifications.Object);

            // Act
            sut.OnActionExecuted(action);

            // Assert
            _notifications.VerifyAll();
            _logWriter.VerifyAll();
        }

        [Fact]
        public void OnActionExecuted_WhenRequestExecutedWithNotification_ThenLogResponse()
        {
            // Arrange
            const int httpStatusCodeBadRequest = 400;
            var controller = GetController();
            var controllerName = controller.GetType().FullName;
            var action = GetActionExecutedContext(controller);
            _notifications
                .Setup(n => n.Any())
                .Returns(true);
            _notifications
                .Setup(n => n.GetErrorStatus())
                .Returns(httpStatusCodeBadRequest.ToString());
            _notifications
                .Setup(n => n.GetSummary())
                .Returns(new Fixture().Create<string>());
            _logWriter.Setup(x => x.Error("Business error", httpStatusCodeBadRequest, It.IsAny<Error>(), null));
            var sut = new RequestFilter(_logWriter.Object, _notifications.Object);

            // Act
            sut.OnActionExecuted(action);

            // Assert
            _notifications.VerifyAll();
            _logWriter.VerifyAll();
        }

        private static ActionExecutingContext GetActionExecutingContext(object controller) => new(
                new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller);

        private static ActionExecutedContext GetActionExecutedContext(object controller) => new(
                new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ActionDescriptor()),
                new List<IFilterMetadata>(),
                controller);

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
