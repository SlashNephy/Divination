FROM python:alpine

RUN apk add --update --no-cache tzdata
ENV TZ=Asia/Tokyo

RUN pip install --no-cache-dir flask

COPY ./entrypoint.py /

WORKDIR /
EXPOSE 5000
ENV FLASK_APP=entrypoint.py
ENV PYTHONUNBUFFERED=1

LABEL org.opencontainers.image.source https://github.com/horoscope-dev/Dalamud.DivinationPluginRepo.DownloadCounter
ENTRYPOINT ["flask", "run", "--host", "0.0.0.0"]
