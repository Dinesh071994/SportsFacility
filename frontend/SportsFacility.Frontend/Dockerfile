# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["frontend/SportsFacility.Frontend/SportsFacility.Frontend.csproj", "frontend/SportsFacility.Frontend/"]
RUN dotnet restore "frontend/SportsFacility.Frontend/SportsFacility.Frontend.csproj"

# Copy everything else and build
COPY frontend/SportsFacility.Frontend/ frontend/SportsFacility.Frontend/
RUN dotnet publish "frontend/SportsFacility.Frontend/SportsFacility.Frontend.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SportsFacility.Frontend.dll"]
