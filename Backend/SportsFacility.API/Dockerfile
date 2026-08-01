# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["Backend/SportsFacility.API/SportsFacility.API.csproj", "Backend/SportsFacility.API/"]
COPY ["Backend/SportsFacility.Domain/SportsFacility.Domain.csproj", "Backend/SportsFacility.Domain/"]
COPY ["Backend/SportsFacility.Infrastructure/SportsFacility.Infrastructure.csproj", "Backend/SportsFacility.Infrastructure/"]
COPY ["Backend/SportsFacility.DTO/SportsFacility.DTO.csproj", "Backend/SportsFacility.DTO/"]
COPY ["Backend/SportsFacility.Entity/SportsFacility.Entity.csproj", "Backend/SportsFacility.Entity/"]

RUN dotnet restore "Backend/SportsFacility.API/SportsFacility.API.csproj"

# Copy everything else and build
COPY Backend/ Backend/
RUN dotnet publish "Backend/SportsFacility.API/SportsFacility.API.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SportsFacility.API.dll"]
