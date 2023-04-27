package blue.starry.divination.xivapi

import kotlinx.serialization.SerialName

data class XivApiItemModel(
    @SerialName("Icon") val icon: String,
    @SerialName("ID") val id: Int,
    @SerialName("Description_ja") val description: String
)
