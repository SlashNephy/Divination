import logging
import os

import aiohttp
import discord

DIVINATION_SERVER_URL = os.getenv("DIVINATION_SERVER_URL", "http://divination-server:9090")
DISCORD_USER_TOKEN = os.getenv("DISCORD_USER_TOKEN")
DISCORD_ANNOUNCEMENT_WEBHOOK_URL = os.getenv("DISCORD_ANNOUNCEMENT_WEBHOOK_URL")

class UserBotClient(discord.Client):
    ANNOUNCE_CHANNELS = [
        677907661970341917,  # FFXIV_ACT_Plugin: announce

        324075704436260866,  # TexTools: textools_main_release
        673271263191629864,  # TexTools: textools_beta_release

        585180735032393730,  # goat place: announcements
        663675584844660736,  # goat place: plugin-updates-showcase
        586590269063954432,  # goat place: known_issues_faq

        335938212264411137,  # StormShard FF14: announcements

        621162620946219011,  # Universails: announcements

        581962767767175168,  # お知らせ
        614267746691186692,  # メンテナンス
        583117987460939824,  # faloop関連
    ]
    BETA_TESTING_CHANNELS = [
        551476805148737546,  # FFXIV_ACT_Plugin
        639024446643896350,  # beta-testing
        719513457988337724,  # goat place: plugin-testing
    ]
    MOB_POP_CHANNELS = [
        666443303830421552,  # faloop-info
        603848058417184769,  # mob-hunt-general
        670433688689049620,  # gaia-s-announce
        616481397669888010,  # s-rank-沸かせ
        603821349734973443,  # new-s-rank-announce
        603820455773601792,  # old-s-rank-announce
        603820489927819288,  # a-rank-tour
        603820593506287626,  # fate-announce
    ]

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)

        self.logger = logging.getLogger("Divination.Server.Companion")
        self.logger.setLevel(logging.INFO)

        handler = logging.StreamHandler()
        formatter = logging.Formatter(fmt="[%(asctime)s] [%(levelname)s] %(message)s", datefmt="%Y-%m-%d %H:%M:%S")
        handler.setFormatter(formatter)
        self.logger.addHandler(handler)

    async def on_ready(self):
        self.logger.info(f"UserBot: logged in as {self.user.name} (ID: {self.user.id}).")

    async def on_message(self, message):
        if not isinstance(message.channel, discord.TextChannel):
            return

        if message.channel.id in self.ANNOUNCE_CHANNELS and DISCORD_ANNOUNCEMENT_WEBHOOK_URL:
            await self.on_announce_message(message)
            self.logger.info(f"Handled announce message: {message}")

        if message.channel.id in self.BETA_TESTING_CHANNELS and DISCORD_ANNOUNCEMENT_WEBHOOK_URL:
            await self.on_beta_testing_message(message)
            self.logger.info(f"Handled beta-testing message: {message}")

        if message.channel.id in self.MOB_POP_CHANNELS:
            await self.on_mob_pop_message(message)
            self.logger.info(f"Handled mob pop message: {message}")

    async def on_message_edit(self, before, after):
        await self.on_message(after)

    async def on_announce_message(self, message):
        content = message.clean_content
        # Append mark if edited
        if message.edited_at:
            content += " [:pencil:]"

        async with aiohttp.ClientSession() as session:
            webhook = discord.Webhook.from_url(DISCORD_ANNOUNCEMENT_WEBHOOK_URL, adapter=discord.AsyncWebhookAdapter(session))
            await webhook.send(
                content=content,
                username=f"{message.author.display_name} (#{message.channel.name})",
                avatar_url=message.author.avatar_url
            )

    async def on_beta_testing_message(self, message):
        # Ignore if edited
        if message.edited_at:
            return

        # Ignore if attachments don't have .zip or .7z files
        if not (attachments := [x for x in message.attachments if x.filename.endswith(".zip") or x.filename.endswith(".7z")]):
            # Log if attachments is not empty but don't have .zip or .7z files
            if message.attachments:
                self.logger.info(f"message.attachments: {', '.join([x.filename for x in message.attachments])}")

            return

        embed = discord.Embed(
            description=message.clean_content,
            timestamp=datetime.utcnow()
        )

        for attachment in attachments:
            embed.add_field(
                name=f":paperclip: {attachment.filename} ({round(attachment.size / 2 ** 20, 2)} MB)",
                value=attachment.url
            )

        async with aiohttp.ClientSession() as session:
            webhook = discord.Webhook.from_url(DISCORD_ANNOUNCEMENT_WEBHOOK_URL, adapter=discord.AsyncWebhookAdapter(session))
            await webhook.send(
                embed=embed,
                username=f"{message.author.display_name} (#{message.channel.name})",
                avatar_url=message.author.avatar_url
            )

    async def on_mob_pop_message(self, message):
        # Ignore Faloop Bot
        if message.author.id == 386003163221721088:
            return

        # Ignore if empty message with embeds
        if not message.clean_content:
            return

        async with aiohttp.ClientSession() as session:
            payload = {
                "sender": f"{message.author.display_name}\uE05D{message.channel.name}",
                "message": message.clean_content
            }

            async with session.post(f"{DIVINATION_SERVER_URL}/collect/mobhunt_discord", json=payload):
                pass

if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO)

    if not DISCORD_USER_TOKEN:
        raise KeyError("Env: DISCORD_USER_TOKEN is not present.")

    client = UserBotClient(status=discord.Status.invisible)
    client.run(DISCORD_USER_TOKEN, bot=False)
