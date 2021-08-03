﻿using Advanced_Combat_Tracker;

namespace Divination.ACT.CustomTrigger
{
    public class Plugin : DivinationActPlugin<Worker, PluginTabControl, Settings>, IActPluginV1
    {
        public override Worker CreateWorker()
        {
            return new Worker();
        }

        public override PluginTabControl CreateTabControl()
        {
            return new PluginTabControl();
        }

        public override Settings CreateSettings()
        {
            return new Settings();
        }
    }
}
