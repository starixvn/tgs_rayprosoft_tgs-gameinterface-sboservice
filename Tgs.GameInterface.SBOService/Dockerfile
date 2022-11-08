#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.SBOService/Tgs.GameInterface.SBOService.csproj", "Tgs.GameInterface.SBOService/"]
COPY ["Tgs.GameInterface.SBOService.Core/Tgs.GameInterface.SBOService.Core.csproj", "Tgs.GameInterface.SBOService.Core/"]
RUN dotnet restore "Tgs.GameInterface.SBOService/Tgs.GameInterface.SBOService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.SBOService"
RUN dotnet build "Tgs.GameInterface.SBOService.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameInterface.SBOService.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.SBOService.dll"]