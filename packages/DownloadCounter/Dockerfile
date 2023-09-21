# syntax=docker/dockerfile:1
FROM golang:1.21.1-bullseye@sha256:b2607cee9712c70a557e82c3dd202d3aafbea8b4cdebd7aa9f4ad044e3e98aeb AS build
WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY ./ ./
RUN make build

FROM debian:bullseye-slim@sha256:c618be84fc82aa8ba203abbb07218410b0f5b3c7cb6b4e7248fda7785d4f9946
WORKDIR /app

RUN groupadd -g 1000 app && useradd -u 1000 -g app app

RUN apt-get update \
    && apt-get install -yqq --no-install-recommends \
      ca-certificates \
    && rm -rf /var/lib/apt/lists/*

USER app
COPY --from=build /app/download-counter ./
CMD ["./download-counter"]
