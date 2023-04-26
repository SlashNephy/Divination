package dev.horoscope.divination.sse

import kotlinx.serialization.Serializable

@Serializable
data class SsePayload(
    val sender: String,
    val message: String
)
