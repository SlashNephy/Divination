package blue.starry.divination.core

import blue.starry.divination.core.xivapi.XivApiClient
import blue.starry.divination.create
import kotlinx.coroutines.withTimeoutOrNull
import mu.KotlinLogging
import java.time.LocalDateTime
import java.util.concurrent.ConcurrentHashMap
import kotlin.time.Duration
import kotlin.time.Duration.Companion.seconds
import java.time.Duration as JavaDuration

object LodestoneAvatarManager {
    private val avatarUrlCache = ConcurrentHashMap<String, Pair<String?, LocalDateTime>>()
    private val expired: JavaDuration get() = JavaDuration.ofHours(3)
    private val timeout: Duration get() = 10.seconds

    private val logger = KotlinLogging.create("Divination.SseServer.LodestoneAvatarManager")

    suspend fun fetchUrl(name: String): String? {
        return withTimeoutOrNull(timeout) {
            var url: String? = null

            val cache = avatarUrlCache[name]
            if (cache != null) {
                url = cache.first

                if (JavaDuration.between(cache.second, LocalDateTime.now()) < expired) {
                    return@withTimeoutOrNull url
                }
            }

            try {
                val response = XivApiClient.searchCharacter(name, "Valefor")
                url = response.results.firstOrNull()?.avatar

                avatarUrlCache[name] = url to LocalDateTime.now()
            } catch (e: Exception) {
                logger.error(e) { "Failed to fetch avatar for ${name}." }
            }

            url
        }
    }
}
