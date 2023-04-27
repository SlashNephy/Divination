package blue.starry.divination.listeners

import blue.starry.divination.Env
import blue.starry.divination.sse.SsePayload
import io.ktor.client.HttpClient
import io.ktor.client.plugins.contentnegotiation.ContentNegotiation
import io.ktor.client.request.post
import io.ktor.client.request.setBody
import io.ktor.http.ContentType
import io.ktor.http.contentType
import io.ktor.http.withCharset
import io.ktor.serialization.kotlinx.json.json
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.serialization.json.Json
import net.dv8tion.jda.api.events.message.MessageReceivedEvent
import net.dv8tion.jda.api.hooks.ListenerAdapter

object DiscordMessageHost: ListenerAdapter() {
    private val httpClient = HttpClient {
        install(ContentNegotiation) {
            json(Json)
        }
    }

    override fun onMessageReceived(event: MessageReceivedEvent) {
        if (event.author.isBot || event.isWebhookMessage) {
            return
        }

        val sseAddress = Env.SSE_SERVER_ADDRESS ?: return

        val sender = "${event.member?.effectiveName ?: event.author.name}\uE05D${event.channel.name}"
        val message = buildString {
            appendLine(event.message.contentStripped)

            event.message.attachments.forEach {
                appendLine("[${it.fileName}]")
            }
        }.trim()

        if (message.isEmpty()) {
            return
        }

        GlobalScope.launch {
            httpClient.post("$sseAddress/collect/discord_message") {
                contentType(ContentType.Application.Json.withCharset(Charsets.UTF_8))
                setBody(SsePayload(sender, message))
            }
        }
    }
}
