package blue.starry.divination.endpoints

import blue.starry.divination.Env
import io.ktor.http.HttpHeaders
import io.ktor.server.application.ApplicationCall
import io.ktor.server.request.header

fun ApplicationCall.isAuthorized(): Boolean {
    return Env.CLIENT_TOKEN == null
            || parameters["token"] == Env.CLIENT_TOKEN
            || request.header(HttpHeaders.Authorization) == Env.CLIENT_TOKEN
}
