FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview-alpine3.19 AS base
WORKDIR /app
EXPOSE 5898

FROM mcr.microsoft.com/dotnet/sdk:9.0.100-preview.3-alpine3.19 AS build
WORKDIR /src
COPY ["HyPlayer.Web.csproj", "HyPlayer.Web/"]
RUN dotnet restore "HyPlayer.Web/HyPlayer.Web.csproj"
COPY [".", "HyPlayer.Web/"]
WORKDIR "/src/HyPlayer.Web"
RUN dotnet build "HyPlayer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HyPlayer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HyPlayer.Web.dll"]
