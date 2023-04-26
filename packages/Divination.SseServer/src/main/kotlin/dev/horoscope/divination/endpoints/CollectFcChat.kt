package dev.horoscope.divination.endpoints

import dev.horoscope.divination.Env
import dev.horoscope.divination.core.FcChatForwarder
import io.ktor.application.call
import io.ktor.http.HttpStatusCode
import io.ktor.request.receive
import io.ktor.response.respond
import io.ktor.routing.Route
import io.ktor.routing.post
import kotlinx.serialization.Serializable

fun Route.postCollectFcChat() {
    post("/collect/fc_chat") {
        if (!call.isAuthorized()) {
            return@post call.respond(HttpStatusCode.Unauthorized)
        }

        val webhookUrl = Env.DISCORD_FC_CHAT_WEBHOOK_URL ?: return@post call.respond(HttpStatusCode.Accepted)

        val payload = call.receive<FcChat>()
        FcChatForwarder.forward(webhookUrl, payload)

        call.respond(HttpStatusCode.OK)
    }
}

@Serializable
data class FcChat(
    val sender: String,
    val message: String
)
