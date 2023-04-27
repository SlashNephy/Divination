package blue.starry.divination.endpoints

import blue.starry.divination.core.SseEventBus
import blue.starry.divination.primitives.SseEvent
import blue.starry.divination.primitives.serialize
import io.ktor.http.CacheControl
import io.ktor.http.ContentType
import io.ktor.server.application.ApplicationCall
import io.ktor.server.application.call
import io.ktor.server.response.cacheControl
import io.ktor.server.response.respondTextWriter
import io.ktor.server.routing.Route
import io.ktor.server.routing.get
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow
import kotlinx.coroutines.withContext
import java.io.Writer
import kotlin.time.Duration.Companion.seconds

fun Route.getStream() {
    get("/stream") {
        if (!call.isAuthorized()) {
            call.respondEventFlow(flow {
                emit(SseEvent.Unauthorized)

                while (true) {
                    delay(15.seconds)
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
