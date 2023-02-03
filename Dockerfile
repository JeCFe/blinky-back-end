FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000
ENV RDBConnectionString=Server=localhost;Port=3306;Database=blinky;Uid=root;Pwd=example;

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["blinky-back-end/blinky-back-end.csproj", "blinky-back-end/"]
RUN dotnet restore "blinky-back-end/blinky-back-end.csproj"
COPY . .
WORKDIR "/src/blinky-back-end"
RUN dotnet build "blinky-back-end.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blinky-back-end.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blinky-back-end.dll"]
