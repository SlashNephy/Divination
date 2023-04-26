FROM gradle:8.1.1-jdk17 AS cache
WORKDIR /app
ENV GRADLE_USER_HOME /app/gradle

COPY ./*.gradle.kts ./gradle.properties /app/

RUN gradle shadowJar --parallel --no-daemon --quiet

FROM gradle:8.1.1-jdk17 AS build
WORKDIR /app

COPY --from=cache /app/gradle /home/gradle/.gradle
COPY ./*.gradle.kts ./gradle.properties /app/
COPY ./src/main/ /app/src/main/

RUN gradle shadowJar --parallel --no-daemon

FROM amazoncorretto:17.0.7
WORKDIR /app

COPY --from=build /app/build/libs/Divination.DiscordBot-all.jar /app/Divination.DiscordBot.jar

ENTRYPOINT ["java", "-jar", "/app/Divination.DiscordBot.jar"]
