FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ApiGateway.csproj ./
RUN dotnet restore ApiGateway.csproj

COPY . .
RUN dotnet publish ApiGateway.csproj --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["sh", "-c", "if [ -n \"$PORT\" ]; then exec dotnet ApiGateway.dll --urls http://0.0.0.0:$PORT; else exec dotnet ApiGateway.dll; fi"]