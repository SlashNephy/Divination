package dev.horoscope.divination.endpoints

import dev.horoscope.divination.Env
import dev.horoscope.divination.core.BugReporter
import io.ktor.application.call
import io.ktor.http.HttpStatusCode
import io.ktor.request.receive
import io.ktor.response.respond
import io.ktor.routing.Route
import io.ktor.routing.post
import kotlinx.serialization.Serializable

@Serializable
data class BugReport(
    val sender: String,
    val message: String,
    val file: String,
    val filename: String,
    val version: String
)

fun Route.postCollectBugReport() {
    post("/collect/bug_report") {
        if (!call.isAuthorized()) {
            return@post call.respond(HttpStatusCode.Unauthorized)
        }

        val webhookUrl = Env.DISCORD_BUG_REPORT_WEBHOOK_URL ?: return@post call.respond(HttpStatusCode.Accepted)

        val payload = call.receive<BugReport>()
        BugReporter.send(webhookUrl, payload)

        call.respond(HttpStatusCode.OK)
    }
}
