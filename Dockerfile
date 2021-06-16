FROM mcr.microsoft.com/dotnet/sdk:5.0 AS dotnet_restore
WORKDIR /app
COPY "VehicleReservations.Command.sln" "VehicleReservations.Command.sln"
COPY "src/VehicleReservations.Command.Api/VehicleReservations.Command.Api.csproj" "src/VehicleReservations.Command.Api/VehicleReservations.Command.Api.csproj"
COPY "src/VehicleReservations.Command.ApplicationServices/VehicleReservations.Command.ApplicationServices.csproj" "src/VehicleReservations.Command.ApplicationServices/VehicleReservations.Command.ApplicationServices.csproj"
COPY "src/VehicleReservations.Command.Core/VehicleReservations.Command.Core.csproj" "src/VehicleReservations.Command.Core/VehicleReservations.Command.Core.csproj"
COPY "src/VehicleReservations.Command.Core.Services/VehicleReservations.Command.Core.Services.csproj" "src/VehicleReservations.Command.Core.Services/VehicleReservations.Command.Core.Services.csproj"
COPY "src/VehicleReservations.Command.Infrastructure.Data/VehicleReservations.Command.Infrastructure.Data.csproj" "src/VehicleReservations.Command.Infrastructure.Data/VehicleReservations.Command.Infrastructure.Data.csproj"
COPY "src/VehicleReservations.Command.Infrastructure.CrossCutting.Ioc/VehicleReservations.Command.Infrastructure.CrossCutting.Ioc.csproj" "src/VehicleReservations.Command.Infrastructure.CrossCutting.Ioc/VehicleReservations.Command.Infrastructure.CrossCutting.Ioc.csproj"
COPY "src/VehicleReservations.Command.Infrastructure.CrossCutting.Logger/VehicleReservations.Command.Infrastructure.CrossCutting.Logger.csproj" "src/VehicleReservations.Command.Infrastructure.CrossCutting.Logger/VehicleReservations.Command.Infrastructure.CrossCutting.Logger.csproj"
COPY "test/VehicleReservations.Command.Api.Test/VehicleReservations.Command.Api.Test.csproj" "test/VehicleReservations.Command.Api.Test/VehicleReservations.Command.Api.Test.csproj"
COPY "test/VehicleReservations.Command.ApplicationServices.Test/VehicleReservations.Command.ApplicationServices.Test.csproj" "test/VehicleReservations.Command.ApplicationServices.Test/VehicleReservations.Command.ApplicationServices.Test.csproj"
COPY "test/VehicleReservations.Command.Core.Test/VehicleReservations.Command.Core.Test.csproj" "test/VehicleReservations.Command.Core.Test/VehicleReservations.Command.Core.Test.csproj"
COPY "test/VehicleReservations.Command.Core.Services.Test/VehicleReservations.Command.Core.Services.Test.csproj" "test/VehicleReservations.Command.Core.Services.Test/VehicleReservations.Command.Core.Services.Test.csproj"
COPY "test/VehicleReservations.Command.Infrastructure.Data.Test/VehicleReservations.Command.Infrastructure.Data.Test.csproj" "test/VehicleReservations.Command.Infrastructure.Data.Test/VehicleReservations.Command.Infrastructure.Data.Test.csproj"
COPY "test/VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Test/VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Test.csproj" "test/VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Test/VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Test.csproj"
COPY "test/VehicleReservations.Command.FunctionalTest/VehicleReservations.Command.FunctionalTest.csproj" "test/VehicleReservations.Command.FunctionalTest/VehicleReservations.Command.FunctionalTest.csproj"
COPY "test/VehicleReservations.Command.AcceptanceTest/VehicleReservations.Command.AcceptanceTest.csproj" "test/VehicleReservations.Command.AcceptanceTest/VehicleReservations.Command.AcceptanceTest.csproj"
COPY "nuget.config" "nuget.config"
RUN dotnet restore "VehicleReservations.Command.sln" --no-cache

FROM dotnet_restore AS dotnet_publish
WORKDIR /app
COPY . .
RUN dotnet publish "VehicleReservations.Command.sln" -c Release -o /out 

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /out
COPY --from=dotnet_publish /out .

ENTRYPOINT ["dotnet", "VehicleReservations.Command.Api.dll"]