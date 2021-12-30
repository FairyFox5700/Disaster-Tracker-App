FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DisasterTrackerApp.Identity/DisasterTrackerApp.Identity.csproj", "DisasterTrackerApp.Identity/"]
RUN dotnet restore "DisasterTrackerApp.Identity/DisasterTrackerApp.Identity.csproj"
COPY . .
WORKDIR "/src/DisasterTrackerApp.Identity"
RUN dotnet build "DisasterTrackerApp.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DisasterTrackerApp.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DisasterTrackerApp.Identity.dll"]
