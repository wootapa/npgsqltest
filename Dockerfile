FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS build-env
WORKDIR /src
COPY . .
RUN dotnet publish -p:PublishSingleFile=true -r linux-musl-x64 --self-contained true -p:PublishTrimmed=True -p:TrimMode=Link -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0.4-alpine3.12
COPY --from=build-env /src/publish/ /app
WORKDIR /app
ENTRYPOINT ["./npgsqltest"]