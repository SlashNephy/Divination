FROM python:3.11.4-alpine@sha256:bd16cc548687f25bf7cbc29c3d284c290cf63899b281f0f3a52fc697b48884c7

COPY ./requirements.txt /tmp/
RUN apk add --update --no-cache --virtual .build-deps \
        build-base \
        linux-headers \
    && pip install --no-cache-dir -r /tmp/requirements.txt \
    && apk del --purge .build-deps \
    && rm -f /tmp/requirements.txt

COPY ./entrypoint.py /entrypoint.py

ENTRYPOINT ["python", "-u", "/entrypoint.py"]
