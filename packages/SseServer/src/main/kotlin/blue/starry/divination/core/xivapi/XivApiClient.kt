package blue.starry.divination.core.xivapi

import blue.starry.divination.Env
import io.ktor.client.HttpClient
import io.ktor.client.call.body
import io.ktor.client.plugins.contentnegotiation.ContentNegotiation
import io.ktor.client.request.get
import io.ktor.http.ParametersBuilder
import io.ktor.http.URLProtocol
import io.ktor.http.encodedPath
import io.ktor.serialization.kotlinx.json.json
import io.ktor.server.util.url
import kotlinx.serialization.json.Json

object XivApiClient {
    private val httpClient = HttpClient {
        install(ContentNegotiation) {
            json(Json)
        }
    }

    suspend fun searchCharacter(
        name: String,
        server: String? = null,
        page: Int? = null,
        vararg parameters: Pair<String, Any?>
    ): SearchCharacterResult {
        val url = url {
            protocol = URLProtocol.HTTPS
            host = "xivapi.com"
            encodedPath = "/character/search"
            this.parameters.apply {
                append("name", name)
                appendIfNotNull("server", server)
                appendIfNotNull("page", page)

                for (it in parameters) {
                    append(it.first, it.second?.toString() ?: continue)
                }

                appendIfNotNull("private_key", Env.XIVAPI_TOKEN)
            }
        }

        return httpClient.get(url).body()
    }

    private fun ParametersBuilder.appendIfNotNull(name: String, value: Any?) {
        append(name, value?.toString() ?: return)
    }
}
