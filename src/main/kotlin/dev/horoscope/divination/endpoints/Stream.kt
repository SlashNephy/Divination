package dev.horoscope.divination.endpoints

import dev.horoscope.divination.core.SseEventBus
import dev.horoscope.divination.primitives.SseEvent
import dev.horoscope.divination.primitives.serialize
import io.ktor.application.call
import io.ktor.http.CacheControl
import io.ktor.http.ContentType
import io.ktor.http.HttpStatusCode
import io.ktor.response.cacheControl
import io.ktor.response.respond
import io.ktor.response.respondTextWriter
import io.ktor.routing.Route
import io.ktor.routing.get
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.withContext
import java.io.Writer

fun Route.getStream() {
    get("/stream") {
        if (!call.isAuthorized()) {
            return@get call.respond(HttpStatusCode.Unauthorized)
        }

        call.response.cacheControl(CacheControl.NoCache(null))
        call.respondTextWriter(contentType = ContentType.Text.EventStream) {
            appendEvent(SseEvent.Welcome)

            SseEventBus.events.collect { event ->
                appendEvent(event)
            }
        }
    }
}

@Suppress("BlockingMethodInNonBlockingContext")
private suspend fun Writer.appendEvent(event: SseEvent) {
    appendLine(event.serialize())
    appendLine()

    withContext(Dispatchers.IO) {
        flush()
    }
}
