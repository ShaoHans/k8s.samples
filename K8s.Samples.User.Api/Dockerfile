FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["K8s.Samples.User.Api/K8s.Samples.User.Api.csproj", "K8s.Samples.User.Api/"]
RUN dotnet restore "K8s.Samples.User.Api/K8s.Samples.User.Api.csproj"
COPY . .
WORKDIR "/src/K8s.Samples.User.Api"
RUN dotnet build "K8s.Samples.User.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "K8s.Samples.User.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "K8s.Samples.User.Api.dll"]