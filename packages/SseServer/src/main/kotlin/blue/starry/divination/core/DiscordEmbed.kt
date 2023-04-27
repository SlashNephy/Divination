package blue.starry.divination.core

import kotlinx.serialization.KSerializer
import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable
import kotlinx.serialization.descriptors.PrimitiveKind
import kotlinx.serialization.descriptors.PrimitiveSerialDescriptor
import kotlinx.serialization.encoding.Decoder
import kotlinx.serialization.encoding.Encoder
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter

@Serializable
data class DiscordWebhookMessage(
    val content: String? = null,
    val username: String? = null,
    @SerialName("avatar_url") val avatarUrl: String? = null,
    val embeds: List<DiscordEmbed> = emptyList()
)

@Serializable
data class DiscordEmbed(
    val title: String? = null,
    val type: Type? = null,
    val description: String? = null,
    val url: String? = null,
    @Serializable(with = ZonedDateTimeSerializer::class) val timestamp: ZonedDateTime? = null,
    val color: Int? = null,
    val footer: Footer? = null,
    val image: Image? = null,
    val thumbnail: Thumbnail? = null,
    val video: Video? = null,
    val provider: Provider? = null,
    val author: Author? = null,
    val fields: List<Field> = emptyList()
) {
    @Serializable
    enum class Type {
        @SerialName("rich") Rich,
        @SerialName("image") Image,
        @SerialName("video") Video,
        @SerialName("gifv") Gifv,
        @SerialName("article") Article,
        @SerialName("link") Link
    }

    @Serializable
    data class Footer(
        val text: String,
        @SerialName("icon_url") val iconUrl: String? = null,
        @SerialName("proxy_icon_url") val proxyIconUrl: String? = null
    )

    @Serializable
    data class Image(
        val url: String? = null,
        @SerialName("proxy_url") val proxyUrl: String? = null,
        val height: Int? = null,
        val width: Int? = null
    )

    @Serializable
    data class Thumbnail(
        val url: String? = null,
        @SerialName("proxy_url") val proxyUrl: String? = null,
        val height: Int? = null,
        val width: Int? = null
    )

    @Serializable
    data class Video(
        val url: String? = null,
        @SerialName("proxy_url") val proxyUrl: String? = null,
        val height: Int? = null,
        val width: Int? = null
    )

    @Serializable
    data class Provider(
        val name: String? = null,
        val url: String? = null
    )

    @Serializable
    data class Author(
        val name: String? = null,
        val url: String? = null,
        @SerialName("icon_url") val iconUrl: String? = null,
        @SerialName("proxy_icon_url") val proxyIconUrl: String? = null
    )

    @Serializable
    data class Field(
        val name: String,
        val value: String,
        val inline: Boolean? = null
    )
}

object ZonedDateTimeSerializer: KSerializer<ZonedDateTime> {
    override val descriptor = PrimitiveSerialDescriptor("java.time.ZonedDateTime", PrimitiveKind.STRING)

    override fun serialize(encoder: Encoder, value: ZonedDateTime) {
        val text = value.format(DateTimeFormatter.ISO_OFFSET_DATE_TIME)
        encoder.encodeString(text)
    }

    override fun deserialize(decoder: Decoder): ZonedDateTime {
        val text = decoder.decodeString()
        return ZonedDateTime.parse(text, DateTimeFormatter.ISO_OFFSET_DATE_TIME)
    }
}
