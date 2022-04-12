#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["RVezy.Presentation.WebAPI.csproj", "."]
COPY ["../0 - Common/RVezy.Common.DI/RVezy.Common.DI.csproj", "../0 - Common/RVezy.Common.DI/"]
COPY ["../3 - Infra/RVezy.Infra.Infra/RVezy.Infra.Infra.csproj", "../3 - Infra/RVezy.Infra.Infra/"]
COPY ["../2 - Domain/RVezy.Domain.Domain/RVezy.Domain.Domain.csproj", "../2 - Domain/RVezy.Domain.Domain/"]
RUN dotnet restore "./RVezy.Presentation.WebAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "RVezy.Presentation.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RVezy.Presentation.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RVezy.Presentation.WebAPI.dll"]