package blue.starry.divination.models

import kotlinx.serialization.Serializable

@Serializable
data class LoginResponse(
    val characters: List<Character>?,
    val permissions: List<String>,
    val session: Session,
    val success: Boolean,
    val token: String,
    val user: User?
) {
    @Serializable
    data class Character(
        val avatarUrl: String,
        val dataCenterId: Int,
        val id: Int,
        val isMain: Boolean,
        val name: String,
        val portraitUrl: String,
        val syncedAt: String,
        val worldId: Int
    )

    @Serializable
    data class Session(
        val id: String,
        val verifyCodes: List<String>
    )

    @Serializable
    data class User(
        val characterName: String,
        val createdAt: String,
        val id: Int,
        val permissions: List<String>,
        val username: String
    )
}
