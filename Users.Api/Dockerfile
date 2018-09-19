FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Users.Api/Users.Api.csproj", "Users.Api/"]
RUN dotnet restore "Users.Api/Users.Api.csproj"
COPY . .
WORKDIR "/src/Users.Api"
RUN dotnet build "Users.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Users.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Users.Api.dll"]