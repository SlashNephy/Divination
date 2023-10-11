# syntax=docker/dockerfile:1
FROM golang:1.21.2-bullseye@sha256:23ad9fe7915fab922c85c8ab34768c5fb58f10c20fdcce3c5b700cbffdb2ae78 AS build
WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY ./ ./
RUN make build

FROM debian:bullseye-slim@sha256:9bec46ecd98ce4bf8305840b021dda9b3e1f8494a0768c407e2b233180fa1466
WORKDIR /app

RUN groupadd -g 1000 app && useradd -u 1000 -g app app

RUN apt-get update \
    && apt-get install -yqq --no-install-recommends \
      ca-certificates \
    && rm -rf /var/lib/apt/lists/*

USER app
COPY --from=build /app/download-counter ./
CMD ["./download-counter"]
