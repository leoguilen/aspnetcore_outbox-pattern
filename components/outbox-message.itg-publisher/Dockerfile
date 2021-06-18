FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS dotnet_restore
WORKDIR /app
COPY "OutboxMessage.Itg.sln" "OutboxMessage.Itg.sln"
COPY "src/OutboxMessage.Itg.Publisher/OutboxMessage.Itg.Publisher.csproj" "src/OutboxMessage.Itg.Publisher/OutboxMessage.Itg.Publisher.csproj"
COPY "src/OutboxMessage.Itg.Core/OutboxMessage.Itg.Core.csproj" "src/OutboxMessage.Itg.Core/OutboxMessage.Itg.Core.csproj"
COPY "src/OutboxMessage.Itg.Core.Services/OutboxMessage.Itg.Core.Services.csproj" "src/OutboxMessage.Itg.Core.Services/OutboxMessage.Itg.Core.Services.csproj"
COPY "src/OutboxMessage.Itg.Infra.Broker/OutboxMessage.Itg.Infra.Broker.csproj" "src/OutboxMessage.Itg.Infra.Broker/OutboxMessage.Itg.Infra.Broker.csproj"
COPY "src/OutboxMessage.Itg.Infra.Data/OutboxMessage.Itg.Infra.Data.csproj" "src/OutboxMessage.Itg.Infra.Data/OutboxMessage.Itg.Infra.Data.csproj"
COPY "src/OutboxMessage.Itg.Infra.CrossCutting.Logger/OutboxMessage.Itg.Infra.CrossCutting.Logger.csproj" "src/OutboxMessage.Itg.Infra.CrossCutting.Logger/OutboxMessage.Itg.Infra.CrossCutting.Logger.csproj"
COPY "src/OutboxMessage.Itg.Infra.CrossCutting.IoC/OutboxMessage.Itg.Infra.CrossCutting.IoC.csproj" "src/OutboxMessage.Itg.Infra.CrossCutting.IoC/OutboxMessage.Itg.Infra.CrossCutting.IoC.csproj"
COPY "nuget.config" "nuget.config"
RUN dotnet restore "src/OutboxMessage.Itg.Publisher/OutboxMessage.Itg.Publisher.csproj" --no-cache

FROM dotnet_restore AS dotnet_publish
WORKDIR /app
COPY . .
RUN dotnet publish "src/OutboxMessage.Itg.Publisher/OutboxMessage.Itg.Publisher.csproj" -c Release -o /out 

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
WORKDIR /out
COPY --from=dotnet_publish /out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

ENTRYPOINT ["dotnet", "OutboxMessage.Itg.Publisher.dll"]