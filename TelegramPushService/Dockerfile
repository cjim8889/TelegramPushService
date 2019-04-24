FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["TelegramPushService/TelegramPushService.csproj", "TelegramPushService/"]
RUN dotnet restore "TelegramPushService/TelegramPushService.csproj"
COPY . .
WORKDIR "/src/TelegramPushService"
RUN dotnet build "TelegramPushService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TelegramPushService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .


ENTRYPOINT ["dotnet", "TelegramPushService.dll"]