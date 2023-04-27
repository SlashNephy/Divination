package blue.starry.divination.endpoints

import blue.starry.divination.Env
import blue.starry.divination.core.BugReporter
import io.ktor.http.HttpStatusCode
import io.ktor.server.application.call
import io.ktor.server.request.receive
import io.ktor.server.response.respond
import io.ktor.server.routing.Route
import io.ktor.server.routing.post
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
