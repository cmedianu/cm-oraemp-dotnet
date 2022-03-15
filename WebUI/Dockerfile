FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 7100

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebUI/WebUI.csproj", "WebUI/"]
RUN dotnet restore "WebUI/WebUI.csproj"
COPY . .
WORKDIR "/src/WebUI"
RUN dotnet build "WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebUI.csproj" -c Release -o /app/publish

FROM base AS final
EXPOSE 7100
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Docker
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebUI.dll"]
