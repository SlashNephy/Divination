package dev.horoscope.divination.listeners

import blue.starry.gerolt.time.EorzeaTime
import blue.starry.gerolt.time.now
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.delay
import kotlinx.coroutines.isActive
import kotlinx.coroutines.launch
import net.dv8tion.jda.api.OnlineStatus
import net.dv8tion.jda.api.entities.Activity
import net.dv8tion.jda.api.events.ReadyEvent
import net.dv8tion.jda.api.hooks.ListenerAdapter
import kotlin.time.Duration

object UpdatePresence: ListenerAdapter() {
    override fun onReady(event: ReadyEvent) {
        event.jda.presence.setPresence(OnlineStatus.ONLINE, false)

        GlobalScope.launch {
            while (isActive) {
                val time = EorzeaTime.now()
                val activity = Activity.playing("ET ${time.format()}")
                event.jda.presence.setPresence(activity, false)

                delay(Duration.seconds(3))
            }
        }
    }

    private fun EorzeaTime.format(): String {
        return "${month}/${day} ${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}"
    }
}
