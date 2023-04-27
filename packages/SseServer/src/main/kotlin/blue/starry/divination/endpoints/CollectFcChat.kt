package blue.starry.divination.endpoints

import blue.starry.divination.Env
import blue.starry.divination.core.FcChatForwarder
import io.ktor.http.HttpStatusCode
import io.ktor.server.application.call
import io.ktor.server.request.receive
import io.ktor.server.response.respond
import io.ktor.server.routing.Route
import io.ktor.server.routing.post
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
