#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/SourceBaseBE.FirebaseService/SourceBaseBE.FirebaseService.csproj", "src/SourceBaseBE.FirebaseService/"]
COPY ["subs/iSoft.Core/iSoft.Database/iSoft.Database.csproj", "subs/iSoft.Core/iSoft.Database/"]
COPY ["subs/iSoft.Core/iSoft.Common/iSoft.Common.csproj", "subs/iSoft.Core/iSoft.Common/"]
COPY ["subs/iSoft.Core/iSoft.DBLibrary/iSoft.DBLibrary.csproj", "subs/iSoft.Core/iSoft.DBLibrary/"]
COPY ["subs/iSoft.Core/iSoft.Redis/iSoft.Redis.csproj", "subs/iSoft.Core/iSoft.Redis/"]
RUN dotnet restore "src/SourceBaseBE.FirebaseService/SourceBaseBE.FirebaseService.csproj"
COPY . .
WORKDIR "/src/src/SourceBaseBE.FirebaseService"
RUN dotnet build "SourceBaseBE.FirebaseService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SourceBaseBE.FirebaseService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SourceBaseBE.FirebaseService.dll"]