FROM gradle:8.1.1-jdk17@sha256:e7a4bc8f4ee27feae2eac4de61ca64406b7137e6f6b107052accd24bf0806043 AS cache
WORKDIR /app
ENV GRADLE_USER_HOME /app/gradle

COPY *.gradle.kts gradle.properties /app/

RUN gradle shadowJar --parallel --no-daemon --quiet

FROM gradle:8.1.1-jdk17@sha256:e7a4bc8f4ee27feae2eac4de61ca64406b7137e6f6b107052accd24bf0806043 AS build
WORKDIR /app

COPY --from=cache /app/gradle /home/gradle/.gradle
COPY ./*.gradle.kts ./gradle.properties /app/
COPY ./src/main/ /app/src/main/

RUN gradle shadowJar --parallel --no-daemon

FROM amazoncorretto:17.0.9@sha256:c83e35197786df3064183de644784b634982d6912ab22dc2b81c1bffaf7976f7
WORKDIR /app

COPY --from=build /app/build/libs/Divination.SseServer.Faloop-all.jar /app/Divination.SseServer.Faloop.jar

ENTRYPOINT ["java", "-jar", "/app/Divination.SseServer.Faloop.jar"]
