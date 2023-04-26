package dev.horoscope.divination

import kotlin.properties.ReadOnlyProperty

object Env {
    val DISCORD_TOKEN by string
    val SSE_SERVER_ADDRESS by stringOrNull
    val XIVAPI_TOKEN by stringOrNull
    val LOG_LEVEL by stringOrNull
}

private val string: ReadOnlyProperty<Env, String>
    get() = ReadOnlyProperty { _, property ->
        System.getenv(property.name) ?: throw IllegalArgumentException("env: ${property.name} is not presense.")
    }

private val stringOrNull: ReadOnlyProperty<Env, String?>
    get() = ReadOnlyProperty { _, property ->
        System.getenv(property.name)
    }
