package dev.horoscope.divination

import kotlin.properties.ReadOnlyProperty

object Env {
    val HTTP_HOST by stringOrNull
    val HTTP_PORT by intOrNull

    val DISCORD_FC_CHAT_WEBHOOK_URL by stringOrNull
    val DISCORD_BUG_REPORT_WEBHOOK_URL by stringOrNull

    val CLIENT_TOKEN by stringOrNull
    val XIVAPI_TOKEN by stringOrNull

    val LOG_LEVEL by stringOrNull
}

private val stringOrNull: ReadOnlyProperty<Env, String?>
    get() = ReadOnlyProperty { _, property ->
        System.getenv(property.name)
    }

private val intOrNull: ReadOnlyProperty<Env, Int?>
    get() = ReadOnlyProperty { _, property ->
        System.getenv(property.name)?.toIntOrNull()
    }
