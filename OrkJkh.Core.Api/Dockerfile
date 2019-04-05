FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY OrkJkh.Core.Api.csproj .
RUN dotnet restore OrkJkh.Core.Api.csproj
WORKDIR /src/Api
COPY . .
RUN dotnet build OrkJkh.Core.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OrkJkh.Core.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrkJkh.Core.Api.dll"]
