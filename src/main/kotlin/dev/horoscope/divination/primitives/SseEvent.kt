package dev.horoscope.divination.primitives

import io.ktor.util.generateNonce

sealed class SseEvent {
    data class JsonData(
        val event: String,
        val message: String? = null,
        val sender: String? = null,
        val id: String = generateNonce()
    ): SseEvent()

    data class CollectedData(
        val data: String,
        val event: String
    ): SseEvent() {
        val id = generateNonce()
    }

    object Ping: SseEvent()
    object Welcome: SseEvent()
    object Unauthorized: SseEvent()
    object Maintenance: SseEvent()
}
