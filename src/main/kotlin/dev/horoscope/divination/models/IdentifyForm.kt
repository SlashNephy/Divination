package dev.horoscope.divination.models

import kotlinx.serialization.Serializable

@Serializable
data class IdentifyForm(
    val sessionId: String
)
