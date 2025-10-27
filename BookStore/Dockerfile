# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["BookStore.API/BookStore.API.csproj", "BookStore.API/"]
COPY ["BookStore.Core/BookStore.Core.csproj", "BookStore.Core/"]
COPY ["BookStore.Infrastructure/BookStore.Infrastructure.csproj", "BookStore.Infrastructure/"]
RUN dotnet restore "BookStore.API/BookStore.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/BookStore.API"
RUN dotnet build "BookStore.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BookStore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookStore.API.dll"]

