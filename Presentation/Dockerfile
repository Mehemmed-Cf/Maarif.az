FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Globalization support - needed for many .NET apps
USER root
RUN apt-get update && apt-get install -y libicu-dev
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all csproj files
COPY ["Presentation/Presentation.csproj", "Presentation/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DataAccessLayer/DataAccessLayer.csproj", "DataAccessLayer/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restore
RUN dotnet restore "./Presentation/Presentation.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Presentation"
# Docker context omits a full .git tree; SDK git tasks then fail with "Invalid reference: ''".
RUN dotnet build "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build -p:EnableSourceControlManagerQueries=false

FROM build AS publish
RUN dotnet publish "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -p:EnableSourceControlManagerQueries=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Render expects the app to run on port 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Presentation.dll"]