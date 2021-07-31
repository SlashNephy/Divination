package dev.horoscope.divination

import dev.horoscope.divination.endpoints.getStream
import dev.horoscope.divination.endpoints.postCollect
import dev.horoscope.divination.endpoints.postCollectBugReport
import dev.horoscope.divination.endpoints.postCollectFcChat
import io.ktor.application.Application
import io.ktor.application.install
import io.ktor.features.CallLogging
import io.ktor.features.ContentNegotiation
import io.ktor.features.XForwardedHeaderSupport
import io.ktor.routing.routing
import io.ktor.serialization.json
import kotlinx.serialization.json.Json
import mu.KotlinLogging
import org.slf4j.event.Level

fun Application.module() {
    install(XForwardedHeaderSupport)
    install(CallLogging) {
        level = Level.DEBUG
        logger = KotlinLogging.create("Divination.SseServer.Ktor")
    }
    install(ContentNegotiation) {
        json(Json {
            ignoreUnknownKeys = true
        })
    }

    routing {
        getStream()
        postCollect()
        postCollectBugReport()
        postCollectFcChat()
    }
}
