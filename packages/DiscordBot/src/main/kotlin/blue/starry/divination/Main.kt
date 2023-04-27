package blue.starry.divination

import blue.starry.divination.listeners.DiscordMessageHost
import blue.starry.divination.listeners.ItemMessageHandler
import blue.starry.divination.listeners.UpdatePresence
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
