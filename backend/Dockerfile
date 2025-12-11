FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "src/Cobranca.API/Cobranca.API.csproj" -c Release -o /pub

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /pub .
EXPOSE 80
EXPOSE 443
ENTRYPOINT dotnet Cobranca.API.dll