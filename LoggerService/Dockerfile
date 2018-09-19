FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app2

# copy csproj and restore as distinct layers
COPY LoggerService/LoggerService.csproj ./
RUN dotnet restore LoggerService.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish LoggerService.csproj -c Release -o out
#
# Build runtime image
#FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app2
COPY --from=build-env /app2/out .
ENTRYPOINT ["dotnet", "loggerservice.dll"]