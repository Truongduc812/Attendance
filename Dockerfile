#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY src .
# RUN mkdir /subs
WORKDIR /subs
COPY subs .
# COPY ["../../subs/iSoft.Core/iSoft.Common/iSoft.Common.csproj", "/subs/iSoft.Core/iSoft.Common/"]
# COPY ["../../subs/iSoft.Core/iSoft.ExportLibrary/iSoft.ExportLibrary.csproj", "/subs/iSoft.Core/iSoft.ExportLibrary/"]
# COPY ["../SourceBaseBE.Common/SourceBaseBE.Common.csproj", "/src/SourceBaseBE.Common/"]
# COPY ["../../src/SourceBaseBE.Database/SourceBaseBE.Database.csproj", "src/SourceBaseBE.Database/"]
# COPY ["../../subs/iSoft.Core/iSoft.Database/iSoft.Database.csproj", "subs/iSoft.Core/iSoft.Database/"]
# COPY ["../../subs/iSoft.Core/iSoft.DBLibrary/iSoft.DBLibrary.csproj", "subs/iSoft.Core/iSoft.DBLibrary/"]
RUN dotnet restore /src/SourceBaseBE.MainService/SourceBaseBE.MainService.csproj

WORKDIR /src/SourceBaseBE.MainService
RUN dotnet build "SourceBaseBE.MainService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SourceBaseBE.MainService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SourceBaseBE.MainService.dll"]