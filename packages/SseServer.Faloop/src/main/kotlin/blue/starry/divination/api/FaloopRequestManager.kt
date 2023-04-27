package blue.starry.divination.api

import blue.starry.divination.create
import blue.starry.divination.models.IdentifyForm
import blue.starry.divination.models.LoginForm
import blue.starry.divination.models.LoginResponse
import io.ktor.client.HttpClient
import io.ktor.client.call.body
import io.ktor.client.plugins.HttpTimeout
import io.ktor.client.plugins.contentnegotiation.ContentNegotiation
import io.ktor.client.plugins.cookies.AcceptAllCookiesStorage
import io.ktor.client.plugins.cookies.HttpCookies
import io.ktor.client.plugins.defaultRequest
import io.ktor.client.plugins.logging.LogLevel
import io.ktor.client.plugins.logging.Logger
import io.ktor.client.plugins.logging.Logging
import io.ktor.client.request.HttpRequestBuilder
import io.ktor.client.request.header
import io.ktor.client.request.post
import io.ktor.client.request.setBody
import io.ktor.http.*
import io.ktor.serialization.kotlinx.json.json
import kotlinx.serialization.json.Json
import mu.KotlinLogging

object FaloopRequestManager {
    private val httpClient = HttpClient {
        install(HttpCookies) {
            storage = AcceptAllCookiesStorage()
        }
        install(ContentNegotiation) {
            json(Json)
        }

        Logging {
            level = LogLevel.INFO
            logger = object: Logger {
                private val logger = KotlinLogging.create("Divination.FaloopRequestManager")

                override fun log(message: String) {
                    logger.debug(message)
                }
            }
        }

        install(HttpTimeout) {
            requestTimeoutMillis = 3000
        }

        defaultRequest {
            header(HttpHeaders.Accept, "application/json, text/plain, */*")
            header(HttpHeaders.AcceptEncoding, "gzip, deflate, br")
            header(HttpHeaders.AcceptLanguage, "ja")
            header(HttpHeaders.Origin, "https://faloop.app")
            header(HttpHeaders.Referrer, "https://faloop.app/")
            userAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.0.0 Safari/537.36")
        }
    }

    object Auth {
        private const val LoginEndpointUrl = "https://api.faloop.app/api/auth/user/login"
        private const val IdentifyEndpointUrl = "https://api.faloop.app/api/auth/user/identify"

        private var token: String? = null
        var sessionId: String? = null
            private set

        suspend fun login(username: String, password: String) {
            val identity = identify()
            token = identity.token

            val login = login(username, password, identity.session.id)
            token = login.token
            sessionId = login.session.id
        }

        private suspend fun login(username: String, password: String, sessionId: String): LoginResponse {
            return httpClient.post(LoginEndpointUrl) {
                contentType(ContentType.Application.Json.withCharset(Charsets.UTF_8))
                setBody(LoginForm(username, password, true, sessionId))
                setToken(token)
            }.body()
        }

        private suspend fun identify(): LoginResponse {
            return httpClient.post(IdentifyEndpointUrl) {
                contentType(ContentType.Application.Json.withCharset(Charsets.UTF_8))
                setBody(IdentifyForm(""))
                setToken(token)
            }.body()
        }

        private fun HttpRequestBuilder.setToken(token: String?) {
            header(HttpHeaders.Authorization, token.toString())
        }
    }
}
