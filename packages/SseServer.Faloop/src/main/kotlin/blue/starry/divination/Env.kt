package blue.starry.divination

import kotlin.properties.ReadOnlyProperty

object Env {
    val FALOOP_USERNAME by string
    val FALOOP_PASSWORD by string
    val FALOOP_DATACENTER_ID by intOrNull
    val SSE_SERVER_ADDRESS by stringOrNull
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

private val intOrNull: ReadOnlyProperty<Env, Int?>
    get() = ReadOnlyProperty { _, property ->
        System.getenv(property.name)?.toIntOrNull()
    }
