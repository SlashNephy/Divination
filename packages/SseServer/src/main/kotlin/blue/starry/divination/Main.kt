package blue.starry.divination

import blue.starry.divination.core.SseEventBus
import blue.starry.divination.primitives.SseEvent
import io.ktor.server.application.Application
import io.ktor.server.engine.addShutdownHook
import io.ktor.server.engine.embeddedServer
import io.ktor.server.jetty.Jetty
import kotlinx.coroutines.delay
import mu.KotlinLogging
import kotlin.time.Duration.Companion.seconds

suspend fun main() {
    val engine = embeddedServer(
      factory = Jetty,
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
        delay(30.seconds)
        SseEventBus.broadcast(SseEvent.Ping)
    }
}
