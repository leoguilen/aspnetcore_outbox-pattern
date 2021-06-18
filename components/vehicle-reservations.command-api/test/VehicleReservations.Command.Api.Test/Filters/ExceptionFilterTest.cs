using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using VehicleReservations.Command.Api.Filters;
using VehicleReservations.Command.Api.Models;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using Xunit;

namespace VehicleReservations.Command.Api.Test.Filters
{
    public class ExceptionFilterTest
    {
        private readonly Mock<ILogWriter> _logWriter;

        public ExceptionFilterTest()
        {
            _logWriter = new(MockBehavior.Strict);
        }

        [Theory, AutoData]
        public void OnException_WhenExceptionIsThrowed_ThenLogError(Exception exception)
        {
            // Arrange
            var error = Error.FromDefault(exception);
            var context = GetExceptionContext(exception);
            _logWriter.Setup(x => x.Error(exception.Message, error.StatusCode, null, exception));
            var sut = new ExceptionFilter(_logWriter.Object);

            // Act
            sut.OnException(context);

            // Assert
            _logWriter.VerifyAll();
        }

        private static ExceptionContext GetExceptionContext(Exception exception) => new(
            new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor()),
            new List<IFilterMetadata>())
        {
            Exception = exception,
        };
    }
}
