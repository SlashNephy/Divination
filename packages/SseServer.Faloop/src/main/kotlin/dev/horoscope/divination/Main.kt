package dev.horoscope.divination

import dev.horoscope.divination.api.FaloopSocketIOClient
import kotlinx.coroutines.delay
import kotlin.time.Duration

suspend fun main() {
    FaloopSocketIOClient.connect()

    while (true) {
        delay(Duration.seconds(15))
    }
}
