FROM gradle:8.4.0-jdk17@sha256:70da12adf27e83bcc125af9d2bc6f9432590e89c96609625aa688135b27e75fb AS cache
WORKDIR /app
ENV GRADLE_USER_HOME /app/gradle

COPY ./*.gradle.kts ./gradle.properties /app/

RUN gradle shadowJar --parallel --no-daemon --quiet

FROM gradle:8.4.0-jdk17@sha256:70da12adf27e83bcc125af9d2bc6f9432590e89c96609625aa688135b27e75fb AS build
WORKDIR /app

COPY --from=cache /app/gradle /home/gradle/.gradle
COPY ./*.gradle.kts ./gradle.properties /app/
COPY ./src/main/ /app/src/main/

RUN gradle shadowJar --parallel --no-daemon

FROM amazoncorretto:17.0.9@sha256:4f12c3ab62e9a75d8de9b8d69245790c12a660778aea141a3a9aac243820cf68
WORKDIR /app

COPY --from=build /app/build/libs/DiscordBot-all.jar /app/DiscordBot.jar

ENTRYPOINT ["java", "-jar", "/app/DiscordBot.jar"]
