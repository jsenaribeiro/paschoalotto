FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/Cobranca.Service
RUN dotnet publish -c Release -o /pub

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /pub .
EXPOSE 80
EXPOSE 443
ENTRYPOINT dotnet Cobranca.Service.dll