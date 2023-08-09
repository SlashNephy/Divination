FROM python:3.11.4-alpine@sha256:4352dc7ee495075659df0738d21b27b74b7b61fa2351dafcdbd4cbba39d19af3

COPY ./requirements.txt /tmp/
RUN apk add --update --no-cache --virtual .build-deps \
        build-base \
        linux-headers \
    && pip install --no-cache-dir -r /tmp/requirements.txt \
    && apk del --purge .build-deps \
    && rm -f /tmp/requirements.txt

COPY ./entrypoint.py /entrypoint.py

ENTRYPOINT ["python", "-u", "/entrypoint.py"]
