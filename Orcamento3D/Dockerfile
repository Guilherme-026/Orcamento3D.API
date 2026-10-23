# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Orcamento3D.API/Orcamento3D.API.csproj", "Orcamento3D.API/"]
RUN dotnet restore "Orcamento3D.API/Orcamento3D.API.csproj"
COPY . .
WORKDIR "/src/Orcamento3D.API"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Estágio Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Orcamento3D.API.dll"]