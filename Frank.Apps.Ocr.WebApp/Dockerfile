FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Frank.Apps.Ocr.WebApp/Frank.Apps.Ocr.WebApp.csproj", "Frank.Apps.Ocr.WebApp/"]
RUN dotnet restore "Frank.Apps.Ocr.WebApp/Frank.Apps.Ocr.WebApp.csproj"
COPY . .
WORKDIR "/src/Frank.Apps.Ocr.WebApp"
RUN dotnet build "./Frank.Apps.Ocr.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Frank.Apps.Ocr.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frank.Apps.Ocr.WebApp.dll"]
