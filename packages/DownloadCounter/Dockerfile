# syntax=docker/dockerfile:1
FROM golang:1.21.3-bullseye@sha256:26c7537d6ac3827eb4638034d16edc64de57bb011c8cc8fe301ac13a6568f6f4 AS build
WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY ./ ./
RUN make build

FROM debian:bullseye-slim@sha256:19664a5752dddba7f59bb460410a0e1887af346e21877fa7cec78bfd3cb77da5
WORKDIR /app

RUN groupadd -g 1000 app && useradd -u 1000 -g app app

RUN apt-get update \
    && apt-get install -yqq --no-install-recommends \
      ca-certificates \
    && rm -rf /var/lib/apt/lists/*

USER app
COPY --from=build /app/download-counter ./
CMD ["./download-counter"]
