package dev.horoscope.divination.core.xivapi


import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable

@Serializable
data class SearchCharacterResult(
    @SerialName("Pagination")
    val pagination: Pagination,
    @SerialName("Results")
    val results: List<Result>
) {
    @Serializable
    data class Pagination(
        @SerialName("Page")
        val page: Int,
        @SerialName("PageTotal")
        val pageTotal: Int,
        @SerialName("Results")
        val results: Int,
        @SerialName("ResultsPerPage")
        val resultsPerPage: Int,
        @SerialName("ResultsTotal")
        val resultsTotal: Int
    )

    @Serializable
    data class Result(
        @SerialName("Avatar")
        val avatar: String,
        @SerialName("FeastMatches")
        val feastMatches: Int,
        @SerialName("ID")
        val iD: Int,
        @SerialName("Lang")
        val lang: String,
        @SerialName("Name")
        val name: String,
        @SerialName("Server")
        val server: String
    )
}
