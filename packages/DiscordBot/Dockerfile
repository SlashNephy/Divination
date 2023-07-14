FROM gradle:8.2.1-jdk17@sha256:fa5a07f9d3a738e181c941b3896974a01659f9b28834531f38dd6d321bd3a039 AS cache
WORKDIR /app
ENV GRADLE_USER_HOME /app/gradle

COPY ./*.gradle.kts ./gradle.properties /app/

RUN gradle shadowJar --parallel --no-daemon --quiet

FROM gradle:8.2.1-jdk17@sha256:fa5a07f9d3a738e181c941b3896974a01659f9b28834531f38dd6d321bd3a039 AS build
WORKDIR /app

COPY --from=cache /app/gradle /home/gradle/.gradle
COPY ./*.gradle.kts ./gradle.properties /app/
COPY ./src/main/ /app/src/main/

RUN gradle shadowJar --parallel --no-daemon

FROM amazoncorretto:17.0.7@sha256:0b950170bd22cff5f76a289e501486c2b06e93314b1117139b2dca5e44455d21
WORKDIR /app

COPY --from=build /app/build/libs/DiscordBot-all.jar /app/DiscordBot.jar

ENTRYPOINT ["java", "-jar", "/app/DiscordBot.jar"]
