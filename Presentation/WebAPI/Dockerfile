FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

RUN mkdir /app

COPY Presentation/WebAPI/WebAPI.csproj /app/Presentation/WebAPI/WebAPI.csproj

COPY Application/Application.Common/Application.Common.csproj /app/Application/Application.Common/Application.Common.csproj

COPY Application/Application.Core/Application.Core.csproj /app/Application/Application.Core/Application.Core.csproj

COPY Domain/Domain.csproj /app/Domain/Domain.csproj

COPY Framework/Framework.Core/Framework.Core.csproj /app/Framework/Framework.Core/Framework.Core.csproj

COPY Infrastructure/Infrastructure.csproj /app/Infrastructure/Infrastructure.csproj

WORKDIR /app

RUN dotnet restore Presentation/WebAPI/WebAPI.csproj --disable-parallel

COPY . /app/

WORKDIR /app/Presentation/WebAPI/

RUN dotnet publish WebAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

RUN mkdir /app
WORKDIR /app
COPY --from=build /app/publish .

RUN apt-get update -y && apt-get install -y wget ca-certificates gnupg

RUN wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | gpg --dearmor --yes --output /usr/share/keyrings/apt.postgresql.org-archive-keyring.gpg

RUN echo "deb [signed-by=/usr/share/keyrings/apt.postgresql.org-archive-keyring.gpg] http://apt.postgresql.org/pub/repos/apt/ bullseye-pgdg main" > /etc/apt/sources.list.d/pgdg.list

RUN apt-get update && apt-get install -y postgresql-client-14

EXPOSE 80

ENTRYPOINT [ "dotnet","WebAPI.dll" ]
