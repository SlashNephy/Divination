FROM python:3.11.6-alpine@sha256:3e73c0b6c1f0205225359acac5c8ab1852e7281248d72afda3e1bb8c6f47deab

COPY ./requirements.txt /tmp/
RUN apk add --update --no-cache --virtual .build-deps \
        build-base \
        linux-headers \
    && pip install --no-cache-dir -r /tmp/requirements.txt \
    && apk del --purge .build-deps \
    && rm -f /tmp/requirements.txt

COPY ./entrypoint.py /entrypoint.py

ENTRYPOINT ["python", "-u", "/entrypoint.py"]
