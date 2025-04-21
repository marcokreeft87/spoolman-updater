# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM node:18-slim AS angular-build
WORKDIR /src
# Copy the Angular workspace files (including angular.json, package.json, etc.)
COPY Application/wwwroot/spoolman-updater/package*.json ./wwwroot/spoolman-updater/
COPY Application/wwwroot/spoolman-updater/angular.json ./wwwroot/spoolman-updater/
COPY Application/wwwroot/spoolman-updater/tsconfig*.json ./wwwroot/spoolman-updater/
COPY Application/wwwroot/spoolman-updater/src ./wwwroot/spoolman-updater/src

# Install dependencies and build the Angular app
WORKDIR /src/wwwroot/spoolman-updater
RUN npm install
RUN npm run build -- --configuration production

# This stage is used to build the service project
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

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=angular-build /src/wwwroot/spoolman-updater/dist/spoolman-updater ./wwwroot
ENTRYPOINT ["dotnet", "Application.dll"]