FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
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