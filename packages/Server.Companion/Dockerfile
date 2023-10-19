FROM python:3.11.6-alpine@sha256:537b7980bc438b55f3a27806e034ba50cb8e45338596e3b1928e6b25c1169a60

COPY ./requirements.txt /tmp/
RUN apk add --update --no-cache --virtual .build-deps \
        build-base \
        linux-headers \
    && pip install --no-cache-dir -r /tmp/requirements.txt \
    && apk del --purge .build-deps \
    && rm -f /tmp/requirements.txt

COPY ./entrypoint.py /entrypoint.py

ENTRYPOINT ["python", "-u", "/entrypoint.py"]
