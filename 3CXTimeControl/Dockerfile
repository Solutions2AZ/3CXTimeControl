FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["3CXTimeControl/3CXTimeControl.csproj", "3CXTimeControl/"]
RUN dotnet restore "./3CXTimeControl/3CXTimeControl.csproj"
COPY . .
WORKDIR "/src/3CXTimeControl"
RUN dotnet build "./3CXTimeControl.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./3CXTimeControl.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "3CXTimeControl.dll"]