# STEP 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["UserService.csproj", "./"]
RUN dotnet restore "UserService.csproj"

COPY . .
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

# STEP 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UserService.dll"]