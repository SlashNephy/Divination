package dev.horoscope.divination.listeners

import dev.horoscope.divination.create
import dev.horoscope.divination.xivapi.XivApiClient
import dev.horoscope.divination.xivapi.XivApiItemModel
import io.ktor.client.HttpClient
import io.ktor.client.request.forms.submitForm
import io.ktor.http.Parameters
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.async
import kotlinx.coroutines.runBlocking
import mu.KotlinLogging
import net.dv8tion.jda.api.EmbedBuilder
import net.dv8tion.jda.api.events.message.guild.GuildMessageReceivedEvent
import net.dv8tion.jda.api.hooks.ListenerAdapter
import org.jsoup.Jsoup

object ItemMessageHandler: ListenerAdapter() {
    private val httpClient = HttpClient()
    private val pattern = ":baggage_claim: \\[(.+)]".toRegex()

    private val logger = KotlinLogging.create("Divination.ItemMessageHandler")

    override fun onGuildMessageReceived(event: GuildMessageReceivedEvent) {
        if (event.author == event.jda.selfUser) {
            return
        }

        val match = pattern.find(event.message.contentDisplay) ?: return
        val name = match.groupValues[1].removeSuffix(":hq:").trim()

        val xivApiTask = GlobalScope.async {
            val search = XivApiClient.search(listOf("item"), name, language = "ja", limit = 1)

            search.results.firstOrNull()?.let {
                XivApiClient.get<XivApiItemModel>("item", it.id)
            }
        }
        val erionesTask = GlobalScope.async {
            searchInEriones(name)
        }

        val builder = EmbedBuilder().apply {
            setTitle(name)
            setDescription(":hourglass_flowing_sand: 情報を取得中です...")
            setColor(0xeb4934)
        }

        val message = event.channel.sendMessageEmbeds(builder.build()).complete()

        try {
            val item = runBlocking {
                xivApiTask.await()
            }
            val eriones = runBlocking {
                erionesTask.await()
            }

            builder.setDescription(null)
            builder.descriptionBuilder.apply {
                if (item != null) {
                    append(item.description.replace("\n\n", "\n"))
                }

                val links = buildList {
                    if (eriones != null) {
                        add("[:link: Eriones](${eriones})")
                    }

                    if (item != null) {
                        add("[:link: FFXIV Teamcraft](https://ffxivteamcraft.com/db/ja/item/${item.id})")
                    }
                }

                if (links.isNotEmpty()) {
                    appendLine()
                    append(links.joinToString(" / "))
                }
            }

            if (item != null) {
                builder.setThumbnail("https://xivapi.com${item.icon}")
            }
        } catch (e: Exception) {
            builder.setDescription(":warning: 指定されたアイテムは見つかりませんでした。")
            logger.error(e) { "Failed to fetch item info." }
        }

        message.editMessageEmbeds(builder.build()).complete()
    }

    private val erionesPattern = "onIIDT\\('(\\d+)'\\)".toRegex()

    private suspend fun searchInEriones(name: String): String? {
        val response = httpClient.submitForm<String>("https://eriones.com/tmp/load/incis", formParameters = Parameters.build {
            append("q", name)
        })

        val html = Jsoup.parse(response)
        val result = html.select("table > tbody > tr")
        if (result.isEmpty()) {
            return null
        }

        val match = erionesPattern.find(result.first().attr("onmouseover")) ?: return null
        return "https://eriones.com/${match.groupValues[1]}"
    }
}
