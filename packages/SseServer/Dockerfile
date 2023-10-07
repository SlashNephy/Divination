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

FROM amazoncorretto:17.0.7@sha256:42caee75951cc43cd6056d5d49393b9196eca3a62437d9179108c4f437db1473
WORKDIR /app

COPY --from=build /app/build/libs/Divination.SseServer-all.jar /app/Divination.SseServer.jar

ENTRYPOINT ["java", "-jar", "/app/Divination.SseServer.jar"]
