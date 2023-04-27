package blue.starry.divination.core

import blue.starry.divination.primitives.SseEvent
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.asSharedFlow

object SseEventBus {
    private val flow = MutableSharedFlow<SseEvent>()
    val events = flow.asSharedFlow()

    suspend fun broadcast(event: SseEvent) {
        flow.emit(event)
    }

    fun tryBroadcast(event: SseEvent) {
        flow.tryEmit(event)
    }
}
