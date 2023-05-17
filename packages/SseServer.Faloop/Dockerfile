FROM gradle:8.1.1-jdk17@sha256:7c56302cc359a0d17cf7ccae11fe51d027186d20579f45938cfa2f21803f744f AS cache
WORKDIR /app
ENV GRADLE_USER_HOME /app/gradle

COPY *.gradle.kts gradle.properties /app/

RUN gradle shadowJar --parallel --no-daemon --quiet

FROM gradle:8.1.1-jdk17@sha256:7c56302cc359a0d17cf7ccae11fe51d027186d20579f45938cfa2f21803f744f AS build
WORKDIR /app

COPY --from=cache /app/gradle /home/gradle/.gradle
COPY ./*.gradle.kts ./gradle.properties /app/
COPY ./src/main/ /app/src/main/

RUN gradle shadowJar --parallel --no-daemon

FROM amazoncorretto:17.0.7@sha256:0b950170bd22cff5f76a289e501486c2b06e93314b1117139b2dca5e44455d21
WORKDIR /app

COPY --from=build /app/build/libs/Divination.SseServer.Faloop-all.jar /app/Divination.SseServer.Faloop.jar

ENTRYPOINT ["java", "-jar", "/app/Divination.SseServer.Faloop.jar"]
