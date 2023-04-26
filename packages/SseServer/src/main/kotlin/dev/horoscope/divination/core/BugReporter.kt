package dev.horoscope.divination.core

import dev.horoscope.divination.endpoints.BugReport
import io.ktor.client.HttpClient
import io.ktor.client.features.json.JsonFeature
import io.ktor.client.features.json.serializer.KotlinxSerializer
import io.ktor.client.request.forms.formData
import io.ktor.client.request.forms.submitFormWithBinaryData
import io.ktor.http.*
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json

object BugReporter {
    private val httpClient = HttpClient {
        install(JsonFeature) {
            serializer = KotlinxSerializer()
        }
    }

    suspend fun send(webhookUrl: String, report: BugReport) {
        val avatarUrl = LodestoneAvatarManager.fetchUrl(report.sender.trim())
        val payload = DiscordWebhookMessage(
            embeds = listOf(
                DiscordEmbed(
                    author = DiscordEmbed.Author(
                        name = report.sender,
                        url = null,
                        iconUrl = avatarUrl
                    ),
                    description = report.message,
                    fields = listOf(
                        DiscordEmbed.Field(
                            name = "バージョン",
                            value = report.version,
                            inline = true
                        )
                    )
                )
            )
        )

        httpClient.submitFormWithBinaryData<Unit>(
            url = webhookUrl,
            formData = formData {
                append("file", report.file, Headers.build {
                    append(HttpHeaders.ContentType, ContentType.Text.Plain.withCharset(Charsets.UTF_8))
                    append(HttpHeaders.ContentDisposition, ContentDisposition.Attachment.withParameter(ContentDisposition.Parameters.FileName, report.filename))
                })
                append("payload_json", Json.encodeToString(payload))
            }
        )
    }
}
