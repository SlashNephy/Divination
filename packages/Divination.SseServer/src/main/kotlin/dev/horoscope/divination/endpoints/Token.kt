package dev.horoscope.divination.endpoints

import dev.horoscope.divination.Env
import io.ktor.application.ApplicationCall
import io.ktor.http.HttpHeaders
import io.ktor.request.header

fun ApplicationCall.isAuthorized(): Boolean {
    return Env.CLIENT_TOKEN == null
            || parameters["token"] == Env.CLIENT_TOKEN
            || request.header(HttpHeaders.Authorization) == Env.CLIENT_TOKEN
}
