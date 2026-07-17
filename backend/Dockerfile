FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY PosFarmacia.slnx ./
COPY src/PosFarmacia.Domain/*.csproj src/PosFarmacia.Domain/
COPY src/PosFarmacia.Application/*.csproj src/PosFarmacia.Application/
COPY src/PosFarmacia.Infrastructure/*.csproj src/PosFarmacia.Infrastructure/
COPY src/PosFarmacia.Presentation/*.csproj src/PosFarmacia.Presentation/
RUN dotnet restore src/PosFarmacia.Presentation/PosFarmacia.Presentation.csproj

COPY src/ src/
RUN dotnet publish src/PosFarmacia.Presentation/PosFarmacia.Presentation.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PosFarmacia.Presentation.dll"]
