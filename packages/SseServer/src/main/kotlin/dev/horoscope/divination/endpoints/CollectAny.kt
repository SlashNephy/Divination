package dev.horoscope.divination.endpoints

import dev.horoscope.divination.core.SseEventBus
import dev.horoscope.divination.primitives.SseEvent
import io.ktor.application.call
import io.ktor.http.HttpStatusCode
import io.ktor.request.receiveText
import io.ktor.response.respond
import io.ktor.routing.Route
import io.ktor.routing.post
import io.ktor.util.getValue

fun Route.postCollect() {
    post("/collect/{event}") {
        if (!call.isAuthorized()) {
            return@post call.respond(HttpStatusCode.Unauthorized)
        }

        val event: String by call.parameters
        val payload = call.receiveText()

        val data = SseEvent.CollectedData(payload, event)
        SseEventBus.broadcast(data)

        call.respond(HttpStatusCode.OK)
    }
}
