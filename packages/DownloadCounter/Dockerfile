# syntax=docker/dockerfile:1
FROM golang:1.21.0-bullseye@sha256:63b8bbe5aa9637cd3473667f09a93c1f3c4ca86975db98a1c32f84a025727843 AS build
WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY ./ ./
RUN make build

FROM debian:bullseye-slim@sha256:61386e11b5256efa33823cbfafd668dd651dbce810b24a8fb7b2e32fa7f65a85
WORKDIR /app

RUN groupadd -g 1000 app && useradd -u 1000 -g app app

RUN apt-get update \
    && apt-get install -yqq --no-install-recommends \
      ca-certificates \
    && rm -rf /var/lib/apt/lists/*

USER app
COPY --from=build /app/download-counter ./
CMD ["./download-counter"]
