package blue.starry.divination

import blue.starry.divination.api.FaloopSocketIOClient
import kotlinx.coroutines.delay
import kotlin.time.Duration.Companion.seconds

suspend fun main() {
    FaloopSocketIOClient.connect()

    while (true) {
        delay(15.seconds)
    }
}
