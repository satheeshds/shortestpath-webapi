FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["shortestpath-webapi/shortestpath-webapi.csproj", "shortestpath-webapi/"]
RUN dotnet restore "shortestpath-webapi/shortestpath-webapi.csproj"
COPY . .
WORKDIR "/src/shortestpath-webapi"
RUN dotnet build "shortestpath-webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "shortestpath-webapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT="Development"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "shortestpath-webapi.dll"]
