package dev.horoscope.divination

import dev.horoscope.divination.core.SseEventBus
import dev.horoscope.divination.primitives.SseEvent
import io.ktor.application.Application
import io.ktor.server.cio.CIO
import io.ktor.server.engine.addShutdownHook
import io.ktor.server.engine.embeddedServer
import kotlinx.coroutines.delay
import mu.KotlinLogging
import kotlin.time.Duration

suspend fun main() {
    val engine = embeddedServer(
        factory = CIO,
        port = Env.HTTP_PORT ?: 9090,
        host = Env.HTTP_HOST ?: "0.0.0.0",
        module = Application::module
    )
    engine.addShutdownHook {
        val logger = KotlinLogging.create("Divination.SseServer")
        logger.info { "Received SIGINT signal." }

        SseEventBus.tryBroadcast(SseEvent.Maintenance)
        Thread.sleep(5000)
    }
    engine.start()

    broadcastPingForever()
}

private suspend fun broadcastPingForever() {
    while (true) {
        delay(Duration.seconds(30))
        SseEventBus.broadcast(SseEvent.Ping)
    }
}
