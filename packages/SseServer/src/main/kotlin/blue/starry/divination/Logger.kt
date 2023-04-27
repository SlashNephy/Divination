package blue.starry.divination

import ch.qos.logback.classic.Level
import mu.KLogger
import mu.KotlinLogging

fun KotlinLogging.create(name: String): KLogger {
    val logger = logger(name)
    val underlying = logger.underlyingLogger
    if (underlying is ch.qos.logback.classic.Logger) {
        underlying.level = Level.toLevel(Env.LOG_LEVEL, Level.INFO)
    }

    return logger
}
