FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Google.PubSub.Client.Web.csproj", "Google.PubSub.Client.Web/"]
WORKDIR "Google.PubSub.Client.Web"
RUN dotnet restore "Google.PubSub.Client.Web.csproj"
COPY . .
RUN dotnet build "Google.PubSub.Client.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Google.PubSub.Client.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Google.PubSub.Client.Web.dll"]
