FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["_3CXTimeControl/_3CXTimeControl.csproj", "_3CXTimeControl/"]
RUN dotnet restore "./_3CXTimeControl/_3CXTimeControl.csproj"
COPY . .
WORKDIR "/src/_3CXTimeControl"
RUN dotnet build "./_3CXTimeControl.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./_3CXTimeControl.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "_3CXTimeControl.dll"]