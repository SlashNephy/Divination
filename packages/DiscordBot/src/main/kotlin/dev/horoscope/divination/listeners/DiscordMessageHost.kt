package dev.horoscope.divination.listeners

import dev.horoscope.divination.Env
import dev.horoscope.divination.sse.SsePayload
import io.ktor.client.HttpClient
import io.ktor.client.features.json.JsonFeature
import io.ktor.client.features.json.serializer.KotlinxSerializer
import io.ktor.client.request.post
import io.ktor.http.ContentType
import io.ktor.http.contentType
import io.ktor.http.withCharset
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json
import net.dv8tion.jda.api.events.message.guild.GuildMessageReceivedEvent
import net.dv8tion.jda.api.hooks.ListenerAdapter

object DiscordMessageHost: ListenerAdapter() {
    private val httpClient = HttpClient {
        install(JsonFeature) {
            serializer = KotlinxSerializer()
        }
    }

    override fun onGuildMessageReceived(event: GuildMessageReceivedEvent) {
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
            httpClient.post<Unit>("$sseAddress/collect/discord_message") {
                contentType(ContentType.Application.Json.withCharset(Charsets.UTF_8))
                body = SsePayload(sender, message)
            }
        }
    }
}
