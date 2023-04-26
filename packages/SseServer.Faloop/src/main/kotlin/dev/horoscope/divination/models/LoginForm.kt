package dev.horoscope.divination.models

import kotlinx.serialization.Serializable

@Serializable
data class LoginForm(
    val username: String,
    val password: String,
    val rememberMe: Boolean,
    val sessionId: String
)
