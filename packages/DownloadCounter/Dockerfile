# syntax=docker/dockerfile:1
FROM golang:1.20.5-bullseye@sha256:a3598b93d32819f1759893c532fa186bc61d58f1ced9aa49c2c77fe13383159a AS build
WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY ./ ./
RUN make build

FROM debian:bullseye-slim@sha256:924df86f8aad741a0134b2de7d8e70c5c6863f839caadef62609c1be1340daf5
WORKDIR /app

RUN groupadd -g 1000 app && useradd -u 1000 -g app app

RUN apt-get update \
    && apt-get install -yqq --no-install-recommends \
      ca-certificates \
    && rm -rf /var/lib/apt/lists/*

USER app
COPY --from=build /app/download-counter ./
CMD ["./download-counter"]
