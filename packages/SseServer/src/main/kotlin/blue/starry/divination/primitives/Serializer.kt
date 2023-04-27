package blue.starry.divination.primitives

import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json

fun SseEvent.serialize(): String = buildString {
    when (this@serialize) {
        is SseEvent.JsonData -> {
            appendLine("id: $id")
            appendLine("event: $event")

            val payload = SsePayload(
                sender = sender,
                message = message
            )
            appendLine("data: ${Json.encodeToString(payload)}")
        }
        is SseEvent.CollectedData -> {
            appendLine("id: $id")
            appendLine("event: $event")

            for (line in data.lines()) {
                appendLine("data: $line")
            }
        }
        is SseEvent.Ping -> {
            appendLine("event: ping")
        }
        is SseEvent.Welcome -> {
            return SseEvent.JsonData(
                event = "welcome", message = "SseServer に接続しました！"
            ).serialize()
        }
        is SseEvent.Unauthorized -> {
            return SseEvent.JsonData(
                event = "welcome", message = "SseServer の資格情報が間違っています。データを送受信することはできません。"
            ).serialize()
        }
        is SseEvent.Maintenance -> {
            return SseEvent.JsonData(
                event = "maintenance", message = "SseServer はメンテナンス中です。しばらくお待ちください。"
            ).serialize()
        }
    }
}
