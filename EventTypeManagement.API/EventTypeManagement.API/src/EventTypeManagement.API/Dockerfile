FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EventTypeManagement.API/EventTypeManagement.API/src/EventTypeManagement.API/EventTypeManagement.API.csproj", "EventTypeManagement.API/"]
RUN dotnet restore "EventTypeManagement.API/EventTypeManagement.API.csproj"
COPY ["EventTypeManagement.API/EventTypeManagement.API/src/EventTypeManagement.API", "EventTypeManagement.API/"]
WORKDIR "/src/EventTypeManagement.API"
RUN dotnet build "EventTypeManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventTypeManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# For debugging - list files
RUN ls -la /app
ENTRYPOINT ["dotnet", "EventTypeManagement.API.dll"] 