package dev.horoscope.divination

import dev.horoscope.divination.listeners.DiscordMessageHost
import dev.horoscope.divination.listeners.ItemMessageHandler
import dev.horoscope.divination.listeners.UpdatePresence
import net.dv8tion.jda.api.JDABuilder
import net.dv8tion.jda.api.OnlineStatus

fun main() {
    JDABuilder.createDefault(Env.DISCORD_TOKEN)
        .setStatus(OnlineStatus.DO_NOT_DISTURB)
        .apply {
            addEventListeners(UpdatePresence)
            addEventListeners(DiscordMessageHost)
            addEventListeners(ItemMessageHandler)
        }
        .build()
}
