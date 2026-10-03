FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Dimdim.Api/Dimdim.Api.csproj", "src/Dimdim.Api/"]
COPY ["src/Dimdim.Application/Dimdim.Application.csproj", "src/Dimdim.Application/"]
COPY ["src/Dimdim.Domain/Dimdim.Domain.csproj", "src/Dimdim.Domain/"]
COPY ["src/Dimdim.Infrastructure/Dimdim.Infrastructure.csproj", "src/Dimdim.Infrastructure/"]
RUN dotnet restore "src/Dimdim.Api/Dimdim.Api.csproj"
COPY . .
RUN dotnet publish "src/Dimdim.Api/Dimdim.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Dimdim.Api.dll"]
