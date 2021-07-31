package dev.horoscope.divination

import dev.horoscope.divination.core.SseEventBus
import dev.horoscope.divination.primitives.SseEvent
import io.ktor.application.Application
import io.ktor.server.cio.CIO
import io.ktor.server.engine.embeddedServer
import kotlinx.coroutines.delay
import mu.KotlinLogging
import kotlin.concurrent.thread
import kotlin.time.Duration

suspend fun main() {
    embeddedServer(
        factory = CIO,
        port = Env.HTTP_PORT ?: 9090,
        host = Env.HTTP_HOST ?: "0.0.0.0",
        module = Application::module
    ).start()

    registerShutdownHandler()
    broadcastPingForever()
}

private fun registerShutdownHandler() {
    Runtime.getRuntime().addShutdownHook(thread(start = false) {
        val logger = KotlinLogging.create("Divination.SseServer")
        logger.info { "Received SIGINT signal." }

        SseEventBus.tryBroadcast(SseEvent.Maintenance)
        Thread.sleep(5000)
    })
}

private suspend fun broadcastPingForever() {
    while (true) {
        delay(Duration.seconds(30))
        SseEventBus.broadcast(SseEvent.Ping)
    }
}
