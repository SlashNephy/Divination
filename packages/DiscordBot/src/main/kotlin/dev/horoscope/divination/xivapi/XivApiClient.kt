package dev.horoscope.divination.xivapi

import dev.horoscope.divination.Env
import io.ktor.client.HttpClient
import io.ktor.client.features.json.JsonFeature
import io.ktor.client.features.json.serializer.KotlinxSerializer
import io.ktor.client.request.get
import io.ktor.http.ParametersBuilder
import io.ktor.http.URLProtocol
import java.util.*

object XivApiClient {
    @PublishedApi internal val httpClient = HttpClient {
        install(JsonFeature) {
            serializer = KotlinxSerializer()
        }
    }

    suspend inline fun <reified T> get(
        content: String,
        id: Int,
        language: String? = null,
        pretty: Boolean? = null,
        snakeCase: Boolean? = null,
        columns: List<String>? = null,
        vararg parameters: Pair<String, Any?>
    ): T {
        return httpClient.get {
            url {
                protocol = URLProtocol.HTTPS
                host = "xivapi.com"

                val path = content.lowercase(Locale.getDefault())
                encodedPath = "/${path}/${id}"

                this.parameters.apply {
                    appendIfNotNull("language", language)
                    appendIfNotNull("pretty", pretty?.toInt())
                    appendIfNotNull("snake_case", snakeCase?.toInt())
                    appendIfNotNull("columns", columns?.joinToString(","))

                    for (pair in parameters) {
                        append(pair.first, pair.second?.toString() ?: continue)
                    }

                    appendIfNotNull("private_key", Env.XIVAPI_TOKEN)
                }
            }
        }
    }

    suspend fun search(
        indexes: List<String>? = null,
        string: String? = null,
        stringAlgo: String? = null,
        stringColumn: String? = null,
        page: Int? = null,
        sortField: String? = null,
        sortOrder: String? = null,
        limit: Int? = null,
        language: String? = null,
        pretty: Boolean? = null,
        snakeCase: Boolean? = null,
        columns: List<String>? = null,
        vararg parameters: Pair<String, Any?>
    ): XivApiSearchResult {
        return httpClient.get {
            url {
                protocol = URLProtocol.HTTPS
                host = "xivapi.com"
                encodedPath = "/search"
                this.parameters.apply {
                    appendIfNotNull("indexes", indexes?.joinToString(","))
                    appendIfNotNull("string", string)
                    appendIfNotNull("string_algo", stringAlgo)
                    appendIfNotNull("string_column", stringColumn)
                    appendIfNotNull("page", page)
                    appendIfNotNull("sort_field", sortField)
                    appendIfNotNull("sort_order", sortOrder)
                    appendIfNotNull("limit", limit)

                    appendIfNotNull("language", language)
                    appendIfNotNull("pretty", pretty?.toInt())
                    appendIfNotNull("snake_case", snakeCase?.toInt())
                    appendIfNotNull("columns", columns?.joinToString(","))

                    for (pair in parameters) {
                        append(pair.first, pair.second?.toString() ?: continue)
                    }

                    appendIfNotNull("private_key", Env.XIVAPI_TOKEN)
                }
            }
        }
    }

    @PublishedApi internal fun Boolean.toInt(): Int {
        return when (this) {
            true -> 1
            false -> 0
        }
    }

    @PublishedApi internal fun ParametersBuilder.appendIfNotNull(name: String, value: Any?) {
        append(name, value?.toString() ?: return)
    }
}
