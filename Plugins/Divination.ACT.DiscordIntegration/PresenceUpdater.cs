using System;
using System.Threading.Tasks;
using Advanced_Combat_Tracker;
using DiscordRPC;
using Divination.ACT.DiscordIntegration.Data;
using Divination.ACT.DiscordIntegration.Properties;
using Divination.Common;
using FFXIV_ACT_Plugin.Common;
using FFXIV_ACT_Plugin.Common.Models;
using Sharlayan;
using Sharlayan.Core;

#nullable enable
namespace Divination.ACT.DiscordIntegration
{
    internal class PresenceUpdater
    {
        private static uint _territoryId;
        private static uint _worldId;
        private static string? _duty;

        private readonly ProcessType type;
        private Emoji? emoji;
        private bool isInCombat;
        private bool isInContents;
        private bool isTargeting;
        private string? largeImageKey;
        private bool shouldResetTimer;
        private string? smallImageKey;

        private PresenceUpdater(ProcessType type)
        {
            this.type = type;
        }

        public static void AsGame()
        {
            new PresenceUpdater(ProcessType.Game).Update();
        }

        public static void AsLauncher()
        {
            new PresenceUpdater(ProcessType.Launcher).Update();
        }

        private RichPresence CreateRichPresence()
        {
            if (type == ProcessType.Launcher)
            {
                status = Resources.StatusLauncher;
            }
            else
            {
                var actor = Plugin.CurrentActor;
                if (actor == null)
                {
                    status = Resources.StatusInTitleScreen;
                }
                else
                {
                    UpdateZone(actor);
                    UpdateJob(actor);
                    UpdateOnlineStatus();
                    UpdateCharacter(actor);
                    UpdateWorld(actor);
                    UpdateCoordinates(actor);
                    UpdateParty(actor);
                    UpdateHitPoint(actor);
                    UpdateMagicPoint(actor);
                    UpdateTarget();
                }
            }

            return new RichPresence
            {
                Details = isInCombat && (!Settings.RequireTargetingOnCombat || isTargeting)
                    ? Format(Settings.DetailsInCombatValue)
                    : Format(Settings.DetailsValue),
                State = Format(Settings.StateValue),
                Assets = new Assets
                {
                    LargeImageKey = largeImageKey ?? "logo",
                    LargeImageText = Format(Settings.LogoTooltipValue),
                    SmallImageKey = Settings.JobIconEnabled ? smallImageKey : null,
                    SmallImageText = Settings.JobIconEnabled ? Format(Settings.TooltipValue) : null
                }
            };
        }

        private string Format(string template)
        {
            return template
                .Replace("{status}", status ??= string.Empty)
                .Replace("{act_zone}", actZone ??= string.Empty)
                .Replace("{region}", region ??= string.Empty)
                .Replace("{place}", place ??= string.Empty)
                .Replace("{zone}", zone ??= string.Empty)
                .Replace("{duty}", duty ??= string.Empty)
                .Replace("{character}", character ??= string.Empty)
                .Replace("{level}", level ??= string.Empty)
                .Replace("{job}", shortJobName ??= string.Empty)
                .Replace("{job_name}", fullJobName ??= string.Empty)
                .Replace("{world}", world ??= string.Empty)
                .Replace("{home_world}", homeWorld ??= string.Empty)
                .Replace("{party}", party ??= string.Empty)
                .Replace("{x}", x ??= string.Empty)
                .Replace("{y}", y ??= string.Empty)
                .Replace("{z}", z ??= string.Empty)
                .Replace("{hp}", hp ??= string.Empty)
                .Replace("{hpp}", hpp ??= string.Empty)
                .Replace("{mp}", mp ??= string.Empty)
                .Replace("{mpp}", mpp ??= string.Empty)
                .Replace("{target}", target ??= string.Empty)
                .Replace("{thp}", thp ??= string.Empty)
                .Replace("{thpp}", thpp ??= string.Empty);
        }

        private void Update()
        {
            try
            {
                var presence = CreateRichPresence();
                DiscordApi.UpdatePresence(presence, shouldResetTimer);

                if (Settings.CustomStatusEnabled)
                {
                    var format = isInContents ? Settings.CustomStatusOnContentsValue : Settings.CustomStatusValue;
                    var text = Format(format);
                    if (Settings.JobEmojiEnabled && type == ProcessType.Game)
                    {
                        emoji ??= ClassJob.Unknown.GetEmoji();
                    }

                    Task.Run(async () => { await DiscordApi.UpdateCustomStatus(emoji, text); });
                }
            }
            catch (Exception e)
            {
                Plugin.Logger.Error(e);
            }

            // 変数の値をフォームに報告
            if (Plugin.TabControl.Visible)
            {
                Plugin.TabControl.statusSampleLabel.Text = status;
                Plugin.TabControl.actZoneSampleLabel.Text = actZone;
                Plugin.TabControl.regionSampleLabel.Text = region;
                Plugin.TabControl.placeSampleLabel.Text = place;
                Plugin.TabControl.zoneSampleLabel.Text = zone;
                Plugin.TabControl.dutySampleLabel.Text = duty;
                Plugin.TabControl.characterSampleLabel.Text = character;
                Plugin.TabControl.levelSampleLabel.Text = level;
                Plugin.TabControl.jobSampleLabel.Text = shortJobName;
                Plugin.TabControl.jobNameSampleLabel.Text = fullJobName;
                Plugin.TabControl.worldSampleLabel.Text = world;
                Plugin.TabControl.homeWorldSampleLabel.Text = homeWorld;
                Plugin.TabControl.partySampleLabel.Text = party;
                Plugin.TabControl.xSampleLabel.Text = x;
                Plugin.TabControl.ySampleLabel.Text = y;
                Plugin.TabControl.zSampleLabel.Text = z;
                Plugin.TabControl.hpSampleLabel.Text = hp;
                Plugin.TabControl.hppSampleLabel.Text = hpp;
                Plugin.TabControl.mpSampleLabel.Text = mp;
                Plugin.TabControl.mppSampleLabel.Text = mpp;
                Plugin.TabControl.targetSampleLabel.Text = target;
                Plugin.TabControl.thpSampleLabel.Text = thp;
                Plugin.TabControl.thppSampleLabel.Text = thpp;
            }
        }

        private enum ProcessType
        {
            Game,
            Launcher
        }

        #region Format Variables

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        private string? status;
        private string? actZone;
        private string? region;
        private string? place;
        private string? zone;
        private string? duty;
        private string? character;
        private string? level;
        private string? shortJobName;
        private string? fullJobName;
        private string? world;
        private string? homeWorld;
        private string? party;
        private string? x;
        private string? y;
        private string? z;
        private string? hp;
        private string? hpp;
        private string? mp;
        private string? mpp;
        private string? target;
        private string? thp;

        private string? thpp;
        // ReSharper restore FieldCanBeMadeReadOnly.Local

        #endregion

        #region Update Functions

        private void UpdateZone(Combatant combatant)
        {
            actZone = ActGlobals.oFormActMain.CurrentZone;

            var territoryId = Plugin.DataRepository.GetCurrentTerritoryID();
            var territoryType = Plugin.XivApi.GetTerritoryType(territoryId);
            region = territoryType.GetLocalizedString("PlaceNameRegion", "Name");
            place = territoryType.GetLocalizedString("PlaceName", "Name");
            zone = territoryType.GetLocalizedString("PlaceNameZone", "Name");

            int loadingImageId = territoryType.Dynamic.LoadingImageTargetID;
            if (loadingImageId != 0)
            {
                if (Enum.IsDefined(typeof(LoadingImage), loadingImageId))
                {
                    var loadingImage = (LoadingImage) loadingImageId;
                    largeImageKey = loadingImage.GetImageKey();
                }
            }

            if ((territoryId != _territoryId || combatant.WorldID != _worldId) && Settings.ResetTimerEnabled)
            {
                shouldResetTimer = true;
            }

            _territoryId = territoryId;
            _worldId = combatant.WorldID;
        }

        private void UpdateOnlineStatus()
        {
            Reader.GetActors();
            var user = ActorItem.CurrentUser;

            var onlineStatus = user != null && Enum.IsDefined(typeof(OnlineStatus), user.IconID)
                ? (OnlineStatus) user.IconID
                : OnlineStatus.Online;
            status = onlineStatus.GetLocalizedName();

            if (user?.InCombat == true)
            {
                status = Resources.StatusInCombat;
                isInCombat = true;
            }

            if (Plugin.XivApi.IsContentTerritory(_territoryId))
            {
                var condition = Plugin.XivApi.GetContentFinderCondition(_territoryId);
                duty = condition?.GetLocalizedString("Name") ?? place;

                status = OnlineStatus.InDuty.GetLocalizedName();
                isInContents = true;

                if (duty == _duty && shouldResetTimer)
                {
                    shouldResetTimer = false;
                }

                _duty = duty;
            }

            if (Settings.OnlineStatusEmojiEnabled && (!Settings.JobEmojiEnabled ||
                                                      isInContents &&
                                                      onlineStatus.ShouldOverrideJobEmojiOnInstance() ||
                                                      !isInContents && onlineStatus.ShouldOverrideJobEmojiOnField()))
            {
                emoji = onlineStatus.GetEmoji() ?? emoji;
            }
        }

        private void UpdateCharacter(Combatant actor)
        {
            character = actor.Name;
            level = actor.Level.ToString();
        }

        private void UpdateJob(Combatant actor)
        {
            var job = Plugin.XivApi.GetClassJob((uint) actor.Job);
            var classJob = Enum.IsDefined(typeof(ClassJob), (byte) actor.Job) ? (ClassJob) actor.Job : ClassJob.Unknown;

            shortJobName = job.GetLocalizedString("Abbreviation");
            fullJobName = job.GetLocalizedString("Name");

            smallImageKey = classJob.GetImageKey();
            if (Settings.JobEmojiEnabled)
            {
                emoji = classJob.GetEmoji();
            }
        }

        private void UpdateWorld(Combatant actor)
        {
            var worldData = Plugin.XivApi.GetWorld(actor.CurrentWorldID);
            world = worldData.GetLocalizedString("Name");

            var homeWorldData = Plugin.XivApi.GetWorld(actor.WorldID);
            homeWorld = homeWorldData.GetLocalizedString("Name");
        }

        private void UpdateCoordinates(Combatant actor)
        {
            static double ToHorizontalMapPosition(float rawHorizontalPosition)
            {
                const double offset = 21.5;
                const double pitch = 50.0;

                return offset + rawHorizontalPosition / pitch;
            }

            static double ToVerticalMapPosition(float rawVerticalPosition)
            {
                const double offset = 1.0;
                const double pitch = 100.0;

                return (rawVerticalPosition - offset) / pitch + 0.01;
            }

            x = $"{Math.Round(ToHorizontalMapPosition(actor.PosX), 1)}";
            y = $"{Math.Round(ToHorizontalMapPosition(actor.PosY), 1)}";
            z = $"{Math.Round(ToVerticalMapPosition(actor.PosZ), 1)}";
        }

        private void UpdateParty(Combatant actor)
        {
            var members = Reader.GetPartyMembers()?.PartyMembers;
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (members?.Count == 1 || actor.PartyType == PartyTypeEnum.None)
            {
                party = Resources.PartySolo;
            }
            else if (members?.Count < 4)
            {
                party = $"{Resources.PartyLightParty} ({members.Count}/4)";
            }
            else if (members?.Count == 4)
            {
                party = Resources.PartyLightParty;
            }
            else if (members?.Count < 8)
            {
                party = $"{Resources.PartyLightParty} ({members.Count}/8)";
            }
            else if (actor.PartyType == PartyTypeEnum.Alliance)
            {
                party = Resources.PartyAllianceParty;
            }
            else
            {
                party = Resources.PartyFullParty;
            }
        }

        private void UpdateHitPoint(Combatant actor)
        {
            hp = $"{actor.CurrentHP}/{actor.MaxHP}";
            hpp = $"{Math.Round((double) actor.CurrentHP / actor.MaxHP * 100, 1)}";
        }

        private void UpdateMagicPoint(Combatant actor)
        {
            mp = $"{actor.CurrentMP}/{actor.MaxMP}";
            mpp = $"{Math.Round((double) actor.CurrentMP / actor.MaxMP * 100, 1)}";
        }

        private void UpdateTarget()
        {
            var combatant = Plugin.DataRepository.GetCombatantByOverlayType(OverlayType.Target);
            if (combatant != null)
            {
                isTargeting = true;
                target = combatant.Name;
                thp = $"{combatant.CurrentHP}/{combatant.MaxHP}";
                thpp = $"{Math.Round((double) combatant.CurrentHP / combatant.MaxHP * 100, 1)}";
            }
        }

        #endregion
    }
}
