FROM gradle:8.1.1-jdk17@sha256:e7a4bc8f4ee27feae2eac4de61ca64406b7137e6f6b107052accd24bf0806043 AS cache
WORKDIR /app
ENV GRADLE_USER_HOME /app/gradle

COPY ./*.gradle.kts ./gradle.properties /app/

RUN gradle shadowJar --parallel --no-daemon --quiet

FROM gradle:8.1.1-jdk17@sha256:e7a4bc8f4ee27feae2eac4de61ca64406b7137e6f6b107052accd24bf0806043 AS build
WORKDIR /app

COPY --from=cache /app/gradle /home/gradle/.gradle
COPY ./*.gradle.kts ./gradle.properties /app/
COPY ./src/main/ /app/src/main/

RUN gradle shadowJar --parallel --no-daemon

FROM amazoncorretto:17.0.7@sha256:0b950170bd22cff5f76a289e501486c2b06e93314b1117139b2dca5e44455d21
WORKDIR /app

COPY --from=build /app/build/libs/DiscordBot-all.jar /app/DiscordBot.jar

ENTRYPOINT ["java", "-jar", "/app/DiscordBot.jar"]
