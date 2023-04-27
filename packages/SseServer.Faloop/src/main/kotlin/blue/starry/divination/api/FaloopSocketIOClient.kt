package blue.starry.divination.api

import blue.starry.divination.Env
import blue.starry.divination.create
import blue.starry.divination.sse.SsePayload
import io.ktor.client.HttpClient
import io.ktor.client.plugins.contentnegotiation.ContentNegotiation
import io.ktor.client.request.post
import io.ktor.client.request.setBody
import io.ktor.http.ContentType
import io.ktor.http.HttpHeaders
import io.ktor.http.contentType
import io.ktor.http.withCharset
import io.ktor.serialization.kotlinx.json.json
import io.socket.client.IO
import io.socket.client.Socket
import io.socket.engineio.client.transports.WebSocket
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.serialization.json.Json
import mu.KotlinLogging
import org.json.JSONObject

object FaloopSocketIOClient {
    private val logger = KotlinLogging.create("Divination.FaloopHandler")
    private val httpClient = HttpClient {
        install(ContentNegotiation) {
            json(Json)
        }
    }

    suspend fun connect() {
      FaloopRequestManager.Auth.login(Env.FALOOP_USERNAME, Env.FALOOP_PASSWORD)

        val sessionId = FaloopRequestManager.Auth.sessionId
        requireNotNull(sessionId)

        val options = IO.Options().apply {
            transports = arrayOf(WebSocket.NAME)
            auth = mapOf("sessionid" to sessionId)
            extraHeaders = mapOf(
                HttpHeaders.Accept to listOf("*/*"),
                HttpHeaders.AcceptLanguage to listOf("ja"),
                HttpHeaders.Referrer to listOf("https://faloop.app/"),
                HttpHeaders.UserAgent to listOf("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.0.0 Safari/537.36")
            )
        }

        val socket = IO.socket(
          "wss://api.faloop.app/datacenter/${Env.FALOOP_DATACENTER_ID ?: 3}",  // Default: Gaia DC
          options
        )

        socket.on(Socket.EVENT_CONNECT) {
            onSocketConnected()

            socket.emit("ack")
        }
        socket.on(Socket.EVENT_DISCONNECT) {
            onSocketDisconnected()
        }
        socket.on(Socket.EVENT_CONNECT_ERROR) { events ->
            events.filterIsInstance<Exception>().forEach {
                onSocketConnectError(it)
            }
        }
        socket.on("message") { events ->
            events.filterIsInstance<JSONObject>().forEach {
                onSocketMessage(it)
            }
        }

        socket.connect()
        socket.open()
    }

    private var isReconnecting = false
    private fun onSocketConnected() {
        if (isReconnecting) {
            isReconnecting = false

            broadcast(
                "faloop_reconnected",
                "Faloop に再接続しました。"
            )
        }

        logger.info { "Connected" }
    }

    private fun onSocketDisconnected() {
        isReconnecting = true

        broadcast(
            "faloop_disconnected",
            "Faloop から切断されました。再接続を試みます。"
        )

        logger.error { "Disconnected" }
    }

    private fun onSocketConnectError(exception: Exception) {
        isReconnecting = true

        broadcast(
            "faloop_error",
            "Faloop への接続中にエラーが発生しました。再接続を試みます。"
        )

        logger.error(exception) { "Connect Error" }
    }

    private fun onSocketMessage(message: JSONObject) {
        broadcast("faloop_message", message.toString())
    }

    private fun broadcast(event: String, message: String) {
        GlobalScope.launch {
            httpClient.post("${Env.SSE_SERVER_ADDRESS ?: return@launch}/collect/$event") {
                contentType(ContentType.Application.Json.withCharset(Charsets.UTF_8))
                setBody(SsePayload(null, message))
            }

            logger.trace { "Sent" }
        }
    }
}
