package dev.horoscope.divination.xivapi

import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable

@Serializable
data class XivApiSearchResult(
    @SerialName("Results") val results: List<Result>
) {
    @Serializable
    data class Result(
        @SerialName("ID") val id: Int,
        @SerialName("Icon") val icon: String?,
        @SerialName("Name") val name: String?,
        @SerialName("Url") val url: String,
        @SerialName("UrlType") val urlType: String
    )
}
