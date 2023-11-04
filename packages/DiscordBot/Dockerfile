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

FROM amazoncorretto:21.0.1@sha256:530ed98ce8d82d0405fecab6933190f30a4abba20eda2d6bbcf8f30fbf8e1f7d
WORKDIR /app

COPY --from=build /app/build/libs/DiscordBot-all.jar /app/DiscordBot.jar

ENTRYPOINT ["java", "-jar", "/app/DiscordBot.jar"]
