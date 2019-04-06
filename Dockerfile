FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore OrkJkh.Core.Api/OrkJkh.Core.Api.csproj
# WORKDIR /src/Api
# COPY . .
RUN dotnet build OrkJkh.Core.Api/OrkJkh.Core.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OrkJkh.Core.Api/OrkJkh.Core.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS http://0.0.0.0:5000
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrkJkh.Core.Api.dll"]
