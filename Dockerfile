FROM python:3.9-alpine

RUN apk add --update --no-cache --virtual .build-deps \
        build-base \
        linux-headers \
    && pip install --no-cache-dir \
        discord.py \
        aiohttp \
    && apk del --purge .build-deps

COPY entrypoint.py /entrypoint.py

ENTRYPOINT ["python", "-u", "/entrypoint.py"]
