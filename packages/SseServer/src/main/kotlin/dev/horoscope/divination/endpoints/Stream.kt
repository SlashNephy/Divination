package dev.horoscope.divination.endpoints

import dev.horoscope.divination.core.SseEventBus
import dev.horoscope.divination.primitives.SseEvent
import dev.horoscope.divination.primitives.serialize
import io.ktor.application.ApplicationCall
import io.ktor.application.call
import io.ktor.http.CacheControl
import io.ktor.http.ContentType
import io.ktor.response.cacheControl
import io.ktor.response.respondTextWriter
import io.ktor.routing.Route
import io.ktor.routing.get
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.flow.flow
import kotlinx.coroutines.withContext
import java.io.Writer
import kotlin.time.Duration

fun Route.getStream() {
    get("/stream") {
        if (!call.isAuthorized()) {
            call.respondEventFlow(flow {
                emit(SseEvent.Unauthorized)

                while (true) {
                    delay(Duration.seconds(15))
                }
            })
        } else {
            call.respondEventFlow(SseEventBus.events)
        }
    }
}

private suspend fun ApplicationCall.respondEventFlow(flow: Flow<SseEvent>) {
    response.cacheControl(CacheControl.NoCache(null))
    respondTextWriter(contentType = ContentType.Text.EventStream) {
        appendEvent(SseEvent.Welcome)

        flow.collect { event ->
            appendEvent(event)
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
