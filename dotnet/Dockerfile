FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Stage 2
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "dotnet.dll"]
