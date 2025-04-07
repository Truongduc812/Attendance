#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/UDF.TrackDeviceService/UDF.TrackDeviceService.csproj", "src/UDF.TrackDeviceService/"]
COPY ["subs/iSoft.Core/iSoft.AuthenticationService/iSoft.AuthenticationService.csproj", "subs/iSoft.Core/iSoft.AuthenticationService/"]
COPY ["subs/iSoft.Core/iSoft.Database/iSoft.Database.csproj", "subs/iSoft.Core/iSoft.Database/"]
COPY ["subs/iSoft.Core/iSoft.Common/iSoft.Common.csproj", "subs/iSoft.Core/iSoft.Common/"]
COPY ["subs/iSoft.Core/iSoft.DBLibrary/iSoft.DBLibrary.csproj", "subs/iSoft.Core/iSoft.DBLibrary/"]
COPY ["subs/iSoft.Core/iSoft.ExcelHelper/iSoft.ExcelHepler/iSoft.ExcelHepler.csproj", "subs/iSoft.Core/iSoft.ExcelHelper/iSoft.ExcelHepler/"]
COPY ["src/UDF.Common/UDF.Common.csproj", "src/UDF.Common/"]
COPY ["src/UDF.Database/UDF.Database.csproj", "src/UDF.Database/"]
COPY ["subs/iSoft.Core/iSoft.ConnectionCommon/iSoft.ConnectionCommon.csproj", "subs/iSoft.Core/iSoft.ConnectionCommon/"]
COPY ["src/UDF.MainService/UDF.MainService.csproj", "src/UDF.MainService/"]
COPY ["src/UDF.TwincatLibrary/UDF.TwincatLibrary.csproj", "src/UDF.TwincatLibrary/"]
RUN dotnet restore "src/UDF.TrackDeviceService/UDF.TrackDeviceService.csproj"
COPY . .
WORKDIR "/src/src/UDF.TrackDeviceService"
RUN dotnet build "UDF.TrackDeviceService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UDF.TrackDeviceService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./setting ./setting
ENTRYPOINT ["dotnet", "UDF.TrackDeviceService.dll"]