# Divination.SseServer

📡 A host server to provide sse messages to SseClient

## Docker

`docker-compose.yml`

```yaml
version: '3.8'

services:
  sse-server:
    container_name: Divination.SseServer
    image: ghcr.io/slashnephy/divination-sse-server
    restart: always
    ports:
      - 9090:9090/tcp
    environment:
      # HTTP server host and port (Optional)
      HTTP_HOST: 0.0.0.0
      HTTP_PORT: 9090
      # Discord Webhook url for FC chats (Optional)
      DISCORD_FC_CHAT_WEBHOOK_URL: xxx
      # Discord Webhook url for bug reports (Optional)
      DISCORD_BUG_REPORT_WEBHOOK_URL: xxx
      # Accept only clients which have CLIENT_TOKEN (Optional)
      CLIENT_TOKEN: xxx
      # XivApi API Key (Optional)
      XIVAPI_TOKEN: xxx
      # Log level (Optional)
      LOG_LEVEL: DEBUG
```
