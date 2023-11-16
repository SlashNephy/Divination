plugins {
    kotlin("jvm") version "1.9.20"
    kotlin("plugin.serialization") version "1.8.21"
    id("com.github.johnrengelman.shadow") version "8.1.1"
}

repositories {
    mavenCentral()
}

dependencies {
  implementation("io.ktor:ktor-client-java:2.3.0")
  implementation("io.ktor:ktor-client-content-negotiation:2.3.0")
  implementation("io.ktor:ktor-server-jetty:2.3.0")
  implementation("io.ktor:ktor-server-content-negotiation:2.3.0")
  implementation("io.ktor:ktor-server-call-logging:2.3.0")
  implementation("io.ktor:ktor-server-forwarded-header:2.3.0")
  implementation("io.ktor:ktor-serialization-kotlinx-json:2.3.0")

  implementation("io.github.microutils:kotlin-logging:3.0.5")
  implementation("ch.qos.logback:logback-core:1.4.7")
  implementation("ch.qos.logback:logback-classic:1.4.8")
}

kotlin {
    target {
        compilations.all {
            kotlinOptions {
                jvmTarget = JavaVersion.VERSION_17.toString()
                apiVersion = "1.8"
                languageVersion = "1.8"
            }
        }
    }

    sourceSets.all {
        languageSettings.progressiveMode = true
    }
}

tasks.withType<com.github.jengelman.gradle.plugins.shadow.tasks.ShadowJar> {
    mergeServiceFiles()
    manifest {
        attributes("Main-Class" to "blue.starry.divination.MainKt")
    }
}
