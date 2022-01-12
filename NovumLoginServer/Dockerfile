FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NovumLoginServer/NovumLoginServer.csproj", "NovumLoginServer/"]
RUN dotnet restore "NovumLoginServer/NovumLoginServer.csproj"
COPY . .
WORKDIR "/src/NovumLoginServer"
RUN dotnet build "NovumLoginServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NovumLoginServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NovumLoginServer.dll"]
