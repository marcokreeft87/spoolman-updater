# Use a build argument to determine the architecture (defaults to amd64)
ARG TARGETARCH=amd64

# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build the Angular app
FROM node:20-slim AS angular-build
WORKDIR /src

COPY UI/package*.json ./
COPY UI/angular.json ./
COPY UI/tsconfig*.json ./

# Install dependencies
RUN npm install

# Build the Angular app
COPY UI/src ./src
RUN npm run build:prod

# Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Gateways/Gateways.csproj", "Gateways/"]
RUN dotnet restore "./Application/Application.csproj"
COPY . .
WORKDIR "/src/Application"
RUN dotnet build "./Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the .NET application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image for running the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=angular-build /src/dist/spoolman-updater/browser ./wwwroot
ENTRYPOINT ["dotnet", "Application.dll"]
