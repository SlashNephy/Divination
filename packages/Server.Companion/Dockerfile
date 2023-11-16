FROM python:3.12.0-alpine@sha256:a5d1738d6abbdff3e81c10b7f86923ebcb340ca536e21e8c5ee7d938d263dba1

COPY ./requirements.txt /tmp/
RUN apk add --update --no-cache --virtual .build-deps \
        build-base \
        linux-headers \
    && pip install --no-cache-dir -r /tmp/requirements.txt \
    && apk del --purge .build-deps \
    && rm -f /tmp/requirements.txt

COPY ./entrypoint.py /entrypoint.py

ENTRYPOINT ["python", "-u", "/entrypoint.py"]
