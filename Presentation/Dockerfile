FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

USER root
RUN apt-get update && apt-get install -y --no-install-recommends libicu-dev libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Presentation/Presentation.csproj", "Presentation/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DataAccessLayer/DataAccessLayer.csproj", "DataAccessLayer/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "./Presentation/Presentation.csproj"

COPY . .
WORKDIR "/src/Presentation"
<<<<<<< HEAD
# Docker context omits a full .git tree; SDK git tasks then fail with "Invalid reference: ''".
RUN dotnet build "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build -p:EnableSourceControlManagerQueries=false

FROM build AS publish
RUN dotnet publish "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -p:EnableSourceControlManagerQueries=false
=======
# .dockerignore excludes .git — disable Git/SourceLink SCM queries so build does not fail
RUN dotnet build "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build \
    /p:EnableSourceControlManagerQueries=false

FROM build AS publish
RUN dotnet publish "./Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false \
    /p:EnableSourceControlManagerQueries=false
>>>>>>> d494ec92 (Added Attendance and detached docker and Superadmin role for faster development)

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Presentation.dll"]