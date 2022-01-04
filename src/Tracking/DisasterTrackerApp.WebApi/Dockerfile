FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tracking/DisasterTrackerApp.WebApi/DisasterTrackerApp.WebApi.csproj", "DisasterTrackerApp.WebApi/"]
RUN dotnet restore "Tracking/DisasterTrackerApp.WebApi/DisasterTrackerApp.WebApi.csproj"
COPY . .
WORKDIR "/src/DisasterTrackerApp.WebApi"
RUN dotnet build "DisasterTrackerApp.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DisasterTrackerApp.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DisasterTrackerApp.WebApi.dll"]
