package dev.horoscope.divination.core

import dev.horoscope.divination.endpoints.FcChat
import io.ktor.client.HttpClient
import io.ktor.client.features.json.JsonFeature
import io.ktor.client.features.json.serializer.KotlinxSerializer
import io.ktor.client.request.post
import io.ktor.http.ContentType
import io.ktor.http.contentType
import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock

object FcChatForwarder {
    private val httpClient = HttpClient {
        install(JsonFeature) {
            serializer = KotlinxSerializer()
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

            httpClient.post<Unit>(webhookUrl) {
                contentType(ContentType.Application.Json)

                body = DiscordWebhookMessage(
                    content = chat.message,
                    username = chat.sender,
                    avatarUrl = LodestoneAvatarManager.fetchUrl(chat.sender.removePrefix("[FC]").trim())
                )
            }

            lastSender = chat.sender
            lastMessage = chat.message
        }
    }
}
