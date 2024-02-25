using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using DiscordRPC;
using Divination.DiscordIntegration.Data;
using Divination.DiscordIntegration.Ipc;
using Lumina.Excel.GeneratedSheets;
using ClassJob = Divination.DiscordIntegration.Data.ClassJob;
using OnlineStatus = Divination.DiscordIntegration.Data.OnlineStatus;

namespace Divination.DiscordIntegration;

public class Formatter
{
    #region Singleton

    private static Formatter? _instance;
    private static readonly object Lock = new();

    public static Formatter? Instance
    {
        get
        {
            lock (Lock)
            {
                if (_instance == null)
                {
                    try
                    {
                        _instance ??= new Formatter();
                    }
                    catch (InvalidDalamudDataProvidedException)
                    {
                        _instance = null;
                    }
                    catch (Exception ex)
                    {
                        DalamudLog.Log.Error(ex, "Error occurred while getting Instance");
                    }
                }

                return _instance;
            }
        }
    }

    #endregion

    private Formatter()
    {
        player = DiscordIntegration.Instance.Dalamud.ClientState.LocalPlayer ?? throw new InvalidDalamudDataProvidedException();
        territoryId = DiscordIntegration.Instance.Dalamud.ClientState.TerritoryType;
        if (territoryId == default)
        {
            throw new InvalidDalamudDataProvidedException();
        }

        UpdatePlayerVariables();
        UpdateJob();
        UpdateTerritory();
        UpdateTarget();
        UpdateParty();
        UpdateInCombat();
        UpdateTitle();

        UpdateDuty();
        UpdateOnlineStatus();

        UpdateTimestamp();
    }

    #region Variables

    public string? Fc { get; private set; }
    public string? World { get; private set; }
    public string? HomeWorld { get; private set; }
    public string? Level { get; private set; }
    public string? Name { get; private set; }
    public string? Job { get; private set; }
    public string? JobName { get; private set; }
    public string? X { get; private set; }
    public string? Y { get; private set; }
    public string? Z { get; private set; }
    public string? Hp { get; private set; }
    public string? Hpp { get; private set; }
    public string? Mp { get; private set; }
    public string? Mpp { get; private set; }
    public string? Cp { get; private set; }
    public string? Cpp { get; private set; }
    public string? Gp { get; private set; }
    public string? Gpp { get; private set; }
    public string? Territory { get; private set; }
    public string? Place { get; private set; }
    public string? Region { get; private set; }
    public string? Zone { get; private set; }
    public string? Target { get; private set; }
    public string? Thp { get; private set; }
    public string? Thpp { get; private set; }
    public string? Party { get; private set; }
    public string? Duty { get; private set; }
    public string? Status { get; private set; }
    public string? Title { get; private set; }

    public static readonly Dictionary<string, IpcValueRecord> IpcVariables = new();
    public static readonly Dictionary<string, IpcValueRecord> IpcTemplates = new();

    #endregion

    private readonly PlayerCharacter player;
    private readonly ushort territoryId;

    private bool shouldResetTimer;
    private bool isTargeting;
    private bool inCombat;
    private bool inDuty;
    private bool isOnline;
    private string? smallImageKey;
    private string? largeImageKey;
    private string? customStatusEmojiId;
    private string? customStatusEmojiName;

    private static DateTime _startTime = DateTime.UtcNow;
    private static uint? _previousTerritoryId;
    private static uint? _previousWorldId;
    private static string? _previousDuty;

    #region Update Functions

    private void UpdatePlayerVariables()
    {
        Fc = player.CompanyTag.TextValue;
        World = player.CurrentWorld.GameData?.Name.RawString;
        HomeWorld = player.HomeWorld.GameData?.Name.RawString;
        Level = player.Level.ToString();
        if (Level == "0")
        {
            throw new InvalidDalamudDataProvidedException();
        }

        Name = player.Name.TextValue;
        Job = player.ClassJob.GameData?.Abbreviation.RawString;
        if (Job == "ADV")
        {
            throw new InvalidDalamudDataProvidedException();
        }

        JobName = player.ClassJob.GameData?.Name.RawString;
        X = $"{player.Position.X:F1}";
        Y = $"{player.Position.Y:F1}";
        Z = $"{player.Position.Z:F1}";
        Hp = $"{player.CurrentHp.ToString()}/{player.MaxHp.ToString()}";
        Hpp = $"{Math.Round((float)player.CurrentHp / player.MaxHp * 100, 1).ToString(CultureInfo.InvariantCulture)}";
        Mp = $"{player.CurrentMp.ToString()}/{player.MaxMp.ToString()}";
        Mpp = $"{Math.Round((float)player.CurrentMp / player.MaxMp * 100, 1).ToString(CultureInfo.InvariantCulture)}";
        Cp = $"{player.CurrentCp.ToString()}/{player.MaxCp.ToString()}";
        Cpp = $"{Math.Round((float)player.CurrentCp / player.MaxCp * 100, 1).ToString(CultureInfo.InvariantCulture)}";
        Gp = $"{player.CurrentGp}/{player.MaxGp}";
        Gpp = $"{Math.Round((float)player.CurrentGp / player.MaxGp * 100, 1).ToString(CultureInfo.InvariantCulture)}";
    }

    private void UpdateJob()
    {
        var classJob = Enum.IsDefined(typeof(ClassJob), player.ClassJob.Id) ? (ClassJob)player.ClassJob.Id : ClassJob.Unknown;
        if (DiscordIntegration.Instance.Config.ShowJobSmallImage)
        {
            smallImageKey = classJob.GetImageKey();
        }

        if (DiscordIntegration.Instance.Config.ShowJobCustomStatusEmoji)
        {
            (customStatusEmojiId, customStatusEmojiName) = classJob.GetEmoji();
        }
    }

    private void UpdateTerritory()
    {
        var territoryType = DiscordIntegration.Instance.Dalamud.DataManager.GetExcelSheet<TerritoryType>()!.GetRow(territoryId);

        Territory = territoryType?.Name?.RawString;
        Place = territoryType?.PlaceName?.Value?.Name?.RawString;
        Region = territoryType?.PlaceNameRegion?.Value?.Name?.RawString;
        Zone = territoryType?.PlaceNameZone?.Value?.Name?.RawString;

        if (DiscordIntegration.Instance.Config.ShowLoadingLargeImage)
        {
            var loadingImage = (LoadingImage)(territoryType?.LoadingImage ?? 0);
            largeImageKey = loadingImage.GetImageKey();
        }
    }

    private void UpdateTarget()
    {
        var target = DiscordIntegration.Instance.Dalamud.TargetManager.Target;
        Target = target?.Name.TextValue;

        if (target is Character character)
        {
            isTargeting = true;
            Thp = $"{character.CurrentHp.ToString()}/{character.MaxHp.ToString()}";
            Thpp = $"{Math.Round((float)character.CurrentHp / character.MaxHp * 100, 1).ToString(CultureInfo.InvariantCulture)}";
        }
    }

    private void UpdateParty()
    {
        var partyListCount = DiscordIntegration.Instance.Dalamud.PartyList.Length;
        switch (partyListCount)
        {
            case 0:
                return;
            case < 2:
                Party = "SOLO";
                break;
            case < 4:
                Party = $"LIGHT PARTY ({partyListCount.ToString()} / 4)";
                break;
            case 4:
                Party = "LIGHT PARTY";
                break;
            case < 8:
                Party = $"LIGHT PARTY ({partyListCount.ToString()} / 8)";
                break;
            case 8:
                Party = "FULL PARTY";
                break;
            default:
                Party = "ALLIANCE";
                break;
        }
    }

    private void UpdateDuty()
    {
        var condition = DiscordIntegration.Instance.Dalamud.DataManager.GetExcelSheet<ContentFinderCondition>()!.FirstOrDefault(x => x.TerritoryType.Row == territoryId);
        if (condition != null)
        {
            Duty = condition.Name;

            Status = OnlineStatus.InDuty.GetLocalizedName();
            inDuty = true;
        }
    }

    private void UpdateInCombat()
    {
        var combat = DiscordIntegration.Instance.Dalamud.Condition[ConditionFlag.InCombat];

        inCombat = combat && (!DiscordIntegration.Instance.Config.RequireTargetingOnCombat || isTargeting);
    }

    private void UpdateTimestamp()
    {
        if (_previousTerritoryId != territoryId || _previousWorldId != player.CurrentWorld.Id)
        {
            shouldResetTimer = true;
        }

        if (inDuty && Duty == _previousDuty)
        {
            shouldResetTimer = false;
        }

        _previousTerritoryId = territoryId;
        _previousWorldId = player.CurrentWorld.Id;
        _previousDuty = Duty;

        if (shouldResetTimer && DiscordIntegration.Instance.Config.ResetTimerOnAreaChange)
        {
            _startTime = DateTime.UtcNow;
        }
    }

    private void UpdateOnlineStatus()
    {
        OnlineStatus onlineStatus;
        var icon = player.GetIcon().GetValueOrDefault();
        if (Enum.IsDefined(typeof(OnlineStatus), icon))
        {
            onlineStatus = (OnlineStatus)icon;
        }
        else
        {
            onlineStatus = OnlineStatus.Online;

            if (icon != 0)
            {
                DalamudLog.Log.Warning("不明な Icon を検出しました。 Icon = {Icon}", icon);
            }
        }

        if (onlineStatus == OnlineStatus.Online)
        {
            isOnline = true;
        }

        Status = onlineStatus.GetLocalizedName();
        if (inCombat)
        {
            Status = "戦闘中";
        }

        if (DiscordIntegration.Instance.Config.ShowOnlineStatusCustomStatusEmoji && (!DiscordIntegration.Instance.Config.ShowJobCustomStatusEmoji ||
            inDuty &&
            onlineStatus.ShouldOverrideJobEmojiOnInstance() ||
            !inDuty && onlineStatus.ShouldOverrideJobEmojiOnField()))
        {
            var emoji = onlineStatus.GetEmoji();
            if (emoji != null)
            {
                (customStatusEmojiId, customStatusEmojiName) = emoji.Value;
            }
        }
    }

    private void UpdateTitle()
    {
        var titleId = player.GetTitle().GetValueOrDefault();
        if (titleId != default)
        {
            var titleSheet = DiscordIntegration.Instance.Dalamud.DataManager.GetExcelSheet<Title>()!.GetRow((uint)titleId);
            Title = titleSheet?.Feminine?.RawString;
        }
    }

    #endregion

    #region Public Api

    public static void Reset()
    {
        lock (Lock)
        {
            _instance = new Formatter();
        }
    }

    private string Format(string templateKey)
    {
        if (IpcTemplates.TryGetValue(templateKey, out var ipcTemplate))
        {
            // ReSharper disable once PossibleNullReferenceException
            if (!string.IsNullOrEmpty(ipcTemplate.Value))
            {
                return Render(ipcTemplate.Value);
            }

        }

        return Render(GetTemplate(templateKey));
    }

    public string Render(string template)
    {
        return IpcVariables.Aggregate(template, (x, item) => x.Replace(item.Key, item.Value.Value))
            .Replace("{fc}", Fc ??= string.Empty)
            .Replace("{world}", World ??= string.Empty)
            .Replace("{home_world}", HomeWorld ??= string.Empty)
            .Replace("{level}", Level ??= string.Empty)
            .Replace("{name}", Name ??= string.Empty)
            .Replace("{job}", Job ??= string.Empty)
            .Replace("{job_name}", JobName ??= string.Empty)
            .Replace("{x}", X ??= string.Empty)
            .Replace("{y}", Y ??= string.Empty)
            .Replace("{z}", Z ??= string.Empty)
            .Replace("{hp}", Hp ??= string.Empty)
            .Replace("{hpp}", Hpp ??= string.Empty)
            .Replace("{mp}", Mp ??= string.Empty)
            .Replace("{mpp}", Mpp ??= string.Empty)
            .Replace("{cp}", Cp ??= string.Empty)
            .Replace("{cpp}", Cpp ??= string.Empty)
            .Replace("{gp}", Gp ??= string.Empty)
            .Replace("{gpp}", Gpp ??= string.Empty)
            .Replace("{territory}", Territory ??= string.Empty)
            .Replace("{place}", Place ??= string.Empty)
            .Replace("{region}", Region ??= string.Empty)
            .Replace("{zone}", Zone ??= string.Empty)
            .Replace("{target}", Target ??= string.Empty)
            .Replace("{thp}", Thp ??= string.Empty)
            .Replace("{thpp}", Thpp ??= string.Empty)
            .Replace("{party}", Party ??= string.Empty)
            .Replace("{duty}", Duty ??= string.Empty)
            .Replace("{status}", Status ??= string.Empty)
            .Replace("{title}", Title ??= string.Empty);
    }

    public string GetTemplate(string templateKey)
    {
        switch (templateKey)
        {
            case "details":
                if (inCombat)
                {
                    return DiscordIntegration.Instance.Config.DetailsInCombatFormat;
                }
                else if (inDuty)
                {
                    return DiscordIntegration.Instance.Config.DetailsInDutyFormat;
                }
                else if (isOnline)
                {
                    return DiscordIntegration.Instance.Config.DetailsInOnlineFormat;
                }
                else
                {
                    return DiscordIntegration.Instance.Config.DetailsFormat;
                }
            case "state":
                return DiscordIntegration.Instance.Config.StateFormat;
            case "small_image_text":
                return DiscordIntegration.Instance.Config.SmallImageTextFormat;
            case "large_image_text":
                return DiscordIntegration.Instance.Config.LargeImageTextFormat;
            case "custom_status":
                return inDuty ? DiscordIntegration.Instance.Config.CustomStatusInDutyFormat : DiscordIntegration.Instance.Config.CustomStatusFormat;
            default:
                throw new ArgumentException($"{nameof(templateKey)} = {templateKey} は無効です。");
        }
    }

    public static RichPresence? CreatePresence()
    {
        var instance = Instance;
        if (instance == null)
        {
            return null;
        }

        return new RichPresence
        {
            Details = instance.Format("details"),
            State = instance.Format("state"),
            Assets = new Assets
            {
                SmallImageKey = instance.smallImageKey,
                SmallImageText = DiscordIntegration.Instance.Config.ShowJobSmallImage ? instance.Format("small_image_text") : null,
                LargeImageKey = instance.largeImageKey,
                LargeImageText = DiscordIntegration.Instance.Config.ShowLoadingLargeImage ? instance.Format("large_image_text") : null,
            },
            Timestamps = new Timestamps(_startTime),
        };
    }

    public static (string?, string?, string)? CreateCustomStatus()
    {
        var instance = Instance;
        if (instance == null)
        {
            return null;
        }

        var text = instance.Format("custom_status");

        return (instance.customStatusEmojiId, instance.customStatusEmojiName, text);
    }

    #endregion
}

internal class InvalidDalamudDataProvidedException : Exception
{
}
