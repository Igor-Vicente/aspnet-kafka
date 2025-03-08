FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
WORKDIR /source

COPY *.csproj ./ms-app/
WORKDIR ./ms-app
RUN dotnet restore

COPY . .
RUN dotnet publish -c $BUILD_CONFIGURATION -o /release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as final
WORKDIR /app/
COPY --from=build /release .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Kafka.Producer.Api.dll"]
