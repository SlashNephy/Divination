# syntax=docker/dockerfile:1
FROM golang:1.20.5-bullseye@sha256:041c0baa8a321df827fdadd74893bc825a33b05cac76a9bb802a29ec89f9504b AS build
WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY ./ ./
RUN make build

FROM debian:bullseye-slim@sha256:3460d74bec6b88496cd183d7731930be55234c094f581f7dbdd96f56c1fc34d8
WORKDIR /app

RUN groupadd -g 1000 app && useradd -u 1000 -g app app

RUN apt-get update \
    && apt-get install -yqq --no-install-recommends \
      ca-certificates \
    && rm -rf /var/lib/apt/lists/*

USER app
COPY --from=build /app/download-counter ./
CMD ["./download-counter"]
