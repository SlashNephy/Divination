package blue.starry.divination

import blue.starry.divination.endpoints.getStream
import blue.starry.divination.endpoints.postCollect
import blue.starry.divination.endpoints.postCollectBugReport
import blue.starry.divination.endpoints.postCollectFcChat
import io.ktor.serialization.kotlinx.json.json
import io.ktor.server.application.Application
import io.ktor.server.application.install
import io.ktor.server.plugins.callloging.CallLogging
import io.ktor.server.plugins.contentnegotiation.ContentNegotiation
import io.ktor.server.plugins.forwardedheaders.XForwardedHeaders
import io.ktor.server.routing.routing
import kotlinx.serialization.json.Json
import mu.KotlinLogging
import org.slf4j.event.Level

fun Application.module() {
    install(XForwardedHeaders)
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
