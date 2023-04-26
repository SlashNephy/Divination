using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Divination.ACT.TheHuntNotifier.TheHunt;
using Divination.ACT.TheHuntNotifier.TheHunt.Data;
using Divination.Common;
using Divination.Common.Toast;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;

namespace Divination.ACT.TheHuntNotifier
{
    public class Worker : IPluginWorker
    {
        private readonly Dictionary<string, DateTime> cache;
        private readonly TheHuntClient client;
        private readonly Timer timer;
        private DateTime lastChecked;

        public Worker()
        {
            client = new TheHuntClient();

            cache = new Dictionary<string, DateTime>();

            timer = new Timer(Settings.IntervalMs);
            lastChecked = DateTime.Now;
            timer.Elapsed += Check;
            timer.Start();
        }

        public void Dispose()
        {
            timer.Dispose();
            client.Dispose();
        }

        private async void Check(object sender, ElapsedEventArgs args)
        {
            if (DateTime.Now - lastChecked > TimeSpan.FromMinutes(5))
            {
                lastChecked = DateTime.Now;
                return;
            }

            ApiResponse data;
            try
            {
                data = await client.Fetch(Settings.World);
            }
            catch (Exception e)
            {
                Plugin.Logger.Error(e);
                return;
            }

            foreach (var entity in data.Entries.FindAll(e => e.Reports.Any()))
            {
                var mob = entity.Mob;
                var area = mob?.Area;

                if (mob == null || area == null)
                {
                    continue;
                }

                if (!Settings.TargetMobRanks.Contains(mob.Rank))
                {
                    continue;
                }

                for (var i = 1; i <= area.Instance; i++)
                {
                    var lastReport = entity.GetReportsAt(i).LastOrDefault();
                    if (lastReport == null)
                    {
                        continue;
                    }

                    var key = $"{entity.MobId}_{i}";
                    if (cache.ContainsKey(key) && cache[key] < lastReport.Time)
                    {
                        var message = area.HasInstance
                            ? $"[{mob.Rank}] {mob.Name} ({area.Name} {i}) が討伐されました。"
                            : $"[{mob.Rank}] {mob.Name} ({area.Name}) が討伐されました。";

                        var toast = new ToastContent
                        {
                            Visual = new ToastVisual
                            {
                                BindingGeneric = new ToastBindingGeneric
                                {
                                    Children =
                                    {
                                        new AdaptiveText
                                        {
                                            Text = $"FFXIV the Hunt ({Settings.World})"
                                        },
                                        new AdaptiveText
                                        {
                                            Text = message
                                        }
                                    },
                                    AppLogoOverride = new ToastGenericAppLogo
                                    {
                                        Source = "file:///" + Path.Combine(Plugin.AssemblyDirectory, "mob_hunt.png")
                                    }
                                }
                            }
                        };
                        toast.Show();

                        Plugin.AppendStatusText(message);
                        Plugin.Logger.Info(message);
                    }

                    cache[key] = lastReport.Time;
                }
            }

            lastChecked = DateTime.Now;
        }
    }
}
