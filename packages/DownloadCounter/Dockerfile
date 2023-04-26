FROM python:3.11.2-alpine
WORKDIR /
EXPOSE 5000
ENV TZ=Asia/Tokyo
ENV FLASK_APP=entrypoint.py
ENV PYTHONUNBUFFERED=1

RUN apk add --update --no-cache tzdata

COPY ./requirements.txt /tmp/
RUN pip install --no-cache-dir -r /tmp/requirements.txt \
    && rm -f /tmp/requirements.txt

COPY ./entrypoint.py /

ENTRYPOINT ["flask", "run", "--host", "0.0.0.0"]
