# syntax=docker/dockerfile:1
FROM golang:1.20.4-bullseye@sha256:918857f4064db0fff49799ce5e7c4d43e394f452111cd89cca9af539c18a76a8 AS build
WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY ./ ./
RUN make build

FROM debian:bullseye-slim@sha256:7606bef5684b393434f06a50a3d1a09808fee5a0240d37da5d181b1b121e7637
WORKDIR /app

RUN groupadd -g 1000 app && useradd -u 1000 -g app app

RUN apt-get update \
    && apt-get install -yqq --no-install-recommends \
      ca-certificates \
    && rm -rf /var/lib/apt/lists/*

USER app
COPY --from=build /app/download-counter ./
CMD ["./download-counter"]
