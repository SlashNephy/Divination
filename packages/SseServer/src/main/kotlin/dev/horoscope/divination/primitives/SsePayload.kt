package dev.horoscope.divination.primitives

import kotlinx.serialization.Serializable

@Serializable
data class SsePayload(
    val sender: String?,
    val message: String?
)
