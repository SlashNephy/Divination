package blue.starry.divination.sse

import kotlinx.serialization.Serializable

@Serializable
data class SsePayload(
    val sender: String,
    val message: String
)
