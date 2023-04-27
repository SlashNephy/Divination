using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Divination.SseClient.Handlers.MobHunt.Faloop.Api;
using Divination.SseClient.Payloads;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Divination.SseClient.Handlers.MobHunt.Faloop;

public class FaloopMobInformationHandler : ISsePayloadReceiver, IDisposable
{
    private readonly FaloopApiClient client = new();
    private readonly Lazy<FaloopInitResult> data;
    private readonly Dictionary<(int worldId, int mobId), FaloopPayload.MobStatus> activeStatuses = new();

    public FaloopMobInformationHandler()
    {
        data = new Lazy<FaloopInitResult>(() => client.Init().ConfigureAwait(false).GetAwaiter().GetResult());
    }

    public bool CanReceive(string eventId)
    {
        return eventId == "faloop_message" &&
               (SseClient.Instance.Config.ReceiveFaloopSpawnMessages
                || SseClient.Instance.Config.ReceiveFaloopKillMessages
                || SseClient.Instance.Config.ReceiveFaloopSightMessages);
    }

    public void Receive(string eventId, SsePayload payload)
    {
        var faloop = JsonConvert.DeserializeObject<FaloopPayload>(payload.Message, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        })!;

        switch (faloop.Type)
        {
            case "mob":
                OnFaloopMobStatusReceived(faloop);
                return;
            case "mobworldkill":
            case "world":
            case "maintenance":
                return;
        }
    }


    private void OnFaloopMobStatusReceived(FaloopPayload payload)
    {

        var status = payload.Data?.ToObject<FaloopPayload.MobStatus>(new JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        if (status == null)
        {
            return;
        }


        // 座標情報が更新された場合
        if (status.Sightings != null)
        {
            OnFaloopMobSighted(status, status.Sightings);
            return;
        }

        // モブが湧いた情報があれば新規に湧いたと判断する
        if (status.Spawn != null)
        {
            OnFaloopMobSpawned(status, status.Spawn);
            return;
        }
        // モブ情報が格納されている場合
        if (status.Window != null)
        {
            // ワールド+モブの古い情報が格納されている場合、取り出して処理する
            if (activeStatuses.TryGetValue((status.WorldId, status.MobId), out var oldStatus) && activeStatuses.Remove((status.WorldId, status.MobId)))
            {
                // 過去に格納された古いモブ情報より、新しく受信したモブ情報の開始時間が早い場合、討伐時間修正と判断する
                if (status.Window.StartedAt <= oldStatus.Window?.StartedAt)
                {
                    OnFaloopMissReport(status, oldStatus);
                }
                else
                {
                    // そうでない場合、モブは討伐されたものとする
                    OnFaloopMobKilled(status, status.Window, oldStatus.Spawn);
                }
            }
            else
            {
                // 格納されていない場合、報告されずに討伐されたと判断する
                OnFaloopMobKilledUnknown(status, status.Window);
            }
        }
    }

    private void OnFaloopMobSpawned(FaloopPayload.MobStatus status, FaloopPayload.MobStatus.SpawnStatus spawn)
    {
        if (!SseClient.Instance.Config.ReceiveFaloopSpawnMessages)
        {
            return;
        }

        activeStatuses[(status.WorldId, status.MobId)] = status;

        var payloads = new List<Payload>
        {
            new TextPayload($"{(char) SeIconChar.BoxedLetterS} {status.Mob.Singular} ")
        };

        if (spawn.Location != null)
        {
            payloads.AddRange(CreateMapLink(status.MobId, spawn.Location, status.ZoneInstance).Payloads);
        }

        payloads.AddRange(new List<Payload>
        {
            new IconPayload(BitmapFontIcon.CrossWorld),
            new TextPayload($"{status.World.Name} が湧きました。{FormatTimeSpan(DateTimeOffset.FromUnixTimeSeconds(spawn.Timestamp/1000).DateTime)}")
        });

        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = SseClient.Instance.Config.MobHuntFaloopSpawnMessagesType,
            Name = FormatReporterName(spawn.ReporterName),
            Message = new SeString(payloads)
        });
    }

    private void OnFaloopMissReport(FaloopPayload.MobStatus status, FaloopPayload.MobStatus oldStatus)
    {
        if (!SseClient.Instance.Config.ReceiveFaloopSpawnMessages)
        {
            return;
        }

        if (oldStatus.Spawn == null)
        {
            return;
        }

        var payloads = new List<Payload>
        {
            new IconPayload(BitmapFontIcon.Warning),
            new TextPayload($"{(char) SeIconChar.BoxedLetterS} {status.Mob.Singular} ")
        };

        if (oldStatus.Spawn.Location != null)
        {
            payloads.AddRange(CreateMapLink(status.MobId, oldStatus.Spawn.Location, status.ZoneInstance).Payloads);
        }

        payloads.AddRange(new List<Payload>
        {
            new IconPayload(BitmapFontIcon.CrossWorld),
            new TextPayload($"{status.World.Name} は誤報でした。")
        });

        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = SseClient.Instance.Config.MobHuntFaloopSpawnMessagesType,
            Name = FormatReporterName(oldStatus.Spawn.ReporterName),
            Message = new SeString(payloads)
        });
    }

    private static void OnFaloopMobKilled(FaloopPayload.MobStatus status, FaloopPayload.MobStatus.WindowStatus window, FaloopPayload.MobStatus.SpawnStatus? oldSpawn)
    {
        if (!SseClient.Instance.Config.ReceiveFaloopKillMessages)
        {
            return;
        }

        var payloads = new List<Payload>
        {
            new TextPayload($"{(char) SeIconChar.BoxedLetterS} {status.Mob.Singular} "),
            new IconPayload(BitmapFontIcon.CrossWorld),
            new TextPayload($"{status.World.Name} が討伐されました。{FormatTimeSpan(DateTimeOffset.FromUnixTimeSeconds(window.StartedAt/1000).DateTime)}")
        };

        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = SseClient.Instance.Config.MobHuntFaloopKillMessagesType,
            Name = FormatReporterName(oldSpawn?.ReporterName),
            Message = new SeString(payloads)
        });
    }

    private void OnFaloopMobKilledUnknown(FaloopPayload.MobStatus status, FaloopPayload.MobStatus.WindowStatus window)
    {
        if (!SseClient.Instance.Config.ReceiveFaloopKillMessages)
        {
            return;
        }

        if (SseClient.Instance.Config.FilterOldMobKills && DateTime.UtcNow - DateTimeOffset.FromUnixTimeSeconds(window.StartedAt/1000).LocalDateTime > TimeSpan.FromMinutes(10))
        {
            return;
        }

        var payloads = new List<Payload>
        {
            new TextPayload($"{(char) SeIconChar.BoxedLetterS} {status.Mob.Singular} "),
            new IconPayload(BitmapFontIcon.CrossWorld),
            new TextPayload($"{status.World.Name} が討伐されました。{FormatTimeSpan(DateTimeOffset.FromUnixTimeSeconds(window.StartedAt/1000).DateTime)}")
        };

        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = SseClient.Instance.Config.MobHuntFaloopKillMessagesType,
            Name = "Faloop",
            Message = new SeString(payloads)
        });
    }

    private void OnFaloopMobSighted(FaloopPayload.MobStatus status, FaloopPayload.MobStatus.SightingInfo sightings)
    {
        if (!SseClient.Instance.Config.ReceiveFaloopSightMessages)
        {
            return;
        }

        if (sightings.Sightings == null)
        {
            return;
        }


        // DictionaryをloopしてlatestSightingを抽出する
        var latestSighting = sightings.Sightings.First();
        foreach (KeyValuePair<string, FaloopPayload.MobStatus.SightingPoint> sight in sightings.Sightings)
        {
            // 格納されているlatestSightingより、時間が新しいsightがあれば、latestSightingを更新する
            if (latestSighting.Value.SightedAt < sight.Value.SightedAt)
            {
                latestSighting = sight;
            }
        }

        // Sモブが討伐された後など、プレイヤーによるSightingではない場合はスキップ
        if (latestSighting.Value.ReporterName == null)
        {
            return;
        }

        var payloads = new List<Payload>
        {
            new TextPayload($"{(char) SeIconChar.BoxedLetterB} {status.Mob.Singular} ")
        };
        payloads.AddRange(CreateMapLink(status.MobId, latestSighting.Key, status.ZoneInstance).Payloads);
        payloads.AddRange(new List<Payload>
        {
            new IconPayload(BitmapFontIcon.CrossWorld),
            new TextPayload(status.World.Name)
        });

        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = SseClient.Instance.Config.MobHuntFaloopSightMessagesType,
            Name = FormatReporterName(latestSighting.Value.ReporterName),
            Message = new SeString(payloads)
        });
    }

    private SeString CreateMapLink(int mobId, string location, int? instance)
    {
        var mob = data.Value.Mobs.First(x => x.Id == mobId);
        var zone = data.Value.Zones.First(x => x.Id == mob.ZoneId);
        var n = 41 / (zone.SizeFactor / 100.0);
        var loc = location
            .Split(new[] {','}, 2)
            .Select(int.Parse)
            .Select(x => x / 2048.0 * n + 1)
            .Select(x => Math.Round(x, 1))
            .Select(x => (float) x)
            .ToList();

        var territory = SseClient.Instance.Dalamud.DataManager
                            .GetExcelSheet<TerritoryType>()?
                            .GetRow((uint) zone.Id)
                        ?? throw new ArgumentException($"Not valid territory type: {zone.Id}");
        var mapLink = SeString.CreateMapLink((uint) zone.Id, territory.Map.Row, loc[0], loc[1]);

        var instanceIcon = InstanceIcon(instance);
        return instanceIcon != null ? mapLink.Append(new TextPayload(instanceIcon.Value.ToString())) : mapLink;
    }

    private static char? InstanceIcon(int? instance)
    {
        if (instance is >= 1 and <= 9)
        {
            return (char) (SeIconChar.Instance1 + (instance - 1));
        }

        return null;
    }

    private static string FormatTimeSpan(DateTime time)
    {
        var span = DateTime.UtcNow - time;
        var seconds = (int) Math.Floor(span.TotalSeconds);
        var minutes = Math.DivRem(seconds, 60, out _);
        var hours = Math.DivRem(minutes, 60, out var minute);
        var day = Math.DivRem(hours, 24, out var hour);

        var text = new StringBuilder();
        if (day > 0)
        {
            text.Append($"{day}日");
        }

        if (hour > 0)
        {
            text.Append($"{hour}時間");
        }

        if (minute > 0)
        {
            text.Append($"{minute}分");
        }

        return text.Length == 0 ? string.Empty : $"({text}前)";
    }

    private static SeString FormatReporterName(string? reporter)
    {
        if (string.IsNullOrEmpty(reporter))
        {
            return "Faloop";
        }

        return new SeString(new List<Payload>
        {
            new TextPayload(reporter),
            new IconPayload(BitmapFontIcon.CrossWorld),
            new TextPayload("Faloop")
        });
    }

    public void Dispose()
    {
        client.Dispose();
    }
}