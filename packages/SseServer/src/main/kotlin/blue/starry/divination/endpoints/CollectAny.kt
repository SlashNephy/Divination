package blue.starry.divination.endpoints

import blue.starry.divination.core.SseEventBus
import blue.starry.divination.primitives.SseEvent
import io.ktor.http.HttpStatusCode
import io.ktor.server.application.call
import io.ktor.server.request.receiveText
import io.ktor.server.response.respond
import io.ktor.server.routing.Route
import io.ktor.server.routing.post
import io.ktor.server.util.getValue

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
