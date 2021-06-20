FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS dotnet_restore
WORKDIR /app
COPY "VehicleReservations.Command.sln" "VehicleReservations.Command.sln"
COPY "src/VehicleReservations.Command.Api/VehicleReservations.Command.Api.csproj" "src/VehicleReservations.Command.Api/VehicleReservations.Command.Api.csproj"
COPY "src/VehicleReservations.Command.ApplicationServices/VehicleReservations.Command.ApplicationServices.csproj" "src/VehicleReservations.Command.ApplicationServices/VehicleReservations.Command.ApplicationServices.csproj"
COPY "src/VehicleReservations.Command.Core/VehicleReservations.Command.Core.csproj" "src/VehicleReservations.Command.Core/VehicleReservations.Command.Core.csproj"
COPY "src/VehicleReservations.Command.Core.Services/VehicleReservations.Command.Core.Services.csproj" "src/VehicleReservations.Command.Core.Services/VehicleReservations.Command.Core.Services.csproj"
COPY "src/VehicleReservations.Command.Infrastructure.Data/VehicleReservations.Command.Infrastructure.Data.csproj" "src/VehicleReservations.Command.Infrastructure.Data/VehicleReservations.Command.Infrastructure.Data.csproj"
COPY "src/VehicleReservations.Command.Infrastructure.CrossCutting.Ioc/VehicleReservations.Command.Infrastructure.CrossCutting.Ioc.csproj" "src/VehicleReservations.Command.Infrastructure.CrossCutting.Ioc/VehicleReservations.Command.Infrastructure.CrossCutting.Ioc.csproj"
COPY "src/VehicleReservations.Command.Infrastructure.CrossCutting.Logger/VehicleReservations.Command.Infrastructure.CrossCutting.Logger.csproj" "src/VehicleReservations.Command.Infrastructure.CrossCutting.Logger/VehicleReservations.Command.Infrastructure.CrossCutting.Logger.csproj"
COPY "nuget.config" "nuget.config"
RUN dotnet restore "src/VehicleReservations.Command.Api/VehicleReservations.Command.Api.csproj" --no-cache

FROM dotnet_restore AS dotnet_publish
WORKDIR /app
COPY . .
RUN dotnet publish "src/VehicleReservations.Command.Api/VehicleReservations.Command.Api.csproj" -c Release -o /out 

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
WORKDIR /out
COPY --from=dotnet_publish /out .

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "VehicleReservations.Command.Api.dll"]