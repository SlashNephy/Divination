# syntax=docker/dockerfile:1
FROM golang:1.20.5-bullseye@sha256:c66e71e889475b1c31f302832fc1a552e6ee3ec5a5d116472a785e06d40f0877 AS build
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
