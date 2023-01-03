FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source

COPY SayingHelloApp.sln .
COPY SayingHelloLibrary/*.csproj ./SayingHelloLibrary/
COPY SayingHelloTests/*.csproj ./SayingHelloTests/
COPY SayingHelloWebApi/*.csproj ./SayingHelloWebApi/
RUN dotnet restore

COPY SayingHelloLibrary/. ./SayingHelloLibrary/
COPY SayingHelloTests/. ./SayingHelloTests/
COPY SayingHelloWebApi/. ./SayingHelloWebApi/
WORKDIR /source/SayingHelloWebApi
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80
ENTRYPOINT ["dotnet", "SayingHelloWebApi.dll"]
