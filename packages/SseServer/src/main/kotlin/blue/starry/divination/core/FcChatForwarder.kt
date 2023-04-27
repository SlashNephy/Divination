package blue.starry.divination.core

import blue.starry.divination.endpoints.FcChat
import io.ktor.client.HttpClient
import io.ktor.client.plugins.contentnegotiation.ContentNegotiation
import io.ktor.client.request.post
import io.ktor.client.request.setBody
import io.ktor.http.ContentType
import io.ktor.http.contentType
import io.ktor.serialization.kotlinx.json.json
import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock
import kotlinx.serialization.json.Json

object FcChatForwarder {
    private val httpClient = HttpClient {
        install(ContentNegotiation) {
            json(Json)
        }
    }

    private val lock = Mutex()
    private var lastSender: String? = null
    private var lastMessage: String? = null

    suspend fun forward(webhookUrl: String, chat: FcChat) {
        lock.withLock {
            if (chat.sender == lastSender && chat.message == lastMessage) {
                return
            }

            httpClient.post(webhookUrl) {
                contentType(ContentType.Application.Json)

                setBody(
                  DiscordWebhookMessage(
                    content = chat.message,
                    username = chat.sender,
                    avatarUrl = LodestoneAvatarManager.fetchUrl(chat.sender.removePrefix("[FC]").trim())
                )
                )
            }

            lastSender = chat.sender
            lastMessage = chat.message
        }
    }
}
