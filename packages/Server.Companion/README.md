# Divination.Server.Companion

😎 A companion tool with Divination.Server.

```yml
version: '3.8'

services:
  server:
    container_name: Divination.Server
    image: ghcr.io/horoscope-dev/divination-server:latest
    restart: always
    ports:
      - 127.0.0.1:9090:9090/tcp
    volumes:
      - data:/app/data
    environment:
      DISCORD_TOKEN: xxx
      HTTP_HOST: 0.0.0.0
      HTTP_PORT: 9090
      SSE_TOKEN: xxx
      DISCORD_GUILD_ID: 514067751988625408
      DISCORD_VC_WEBHOOK_URL: https://discord.com/api/webhooks/xxx
      DISCORD_TOOLS_WEBHOOK_URL: https://discord.com/api/webhooks/xxx
      DISCORD_REPORTS_WEBHOOK_URL: https://discord.com/api/webhooks/xxx
      XIVAPI_TOKEN: xxx
      FALOOP_USERNAME: xxx
      FALOOP_PASSWORD: xxx
      GITHUB_TOKEN: xxx

  companion:
    container_name: Divination.Server.Companion
    image: ghcr.io/horoscope-dev/divination-server-companion:latest
    restart: always
    environment:
      DIVINATION_SERVER_URL: http://server:9090
      DISCORD_USER_TOKEN: xxx
      DISCORD_ANNOUNCEMENT_WEBHOOK_URL: https://discord.com/api/webhooks/xxx

volumes:
  data:
    driver: local
```
