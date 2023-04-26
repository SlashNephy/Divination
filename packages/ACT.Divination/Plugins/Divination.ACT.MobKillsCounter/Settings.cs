namespace Divination.ACT.MobKillsCounter
{
    public class Settings : PluginSettings
    {
        public Settings()
        {
            this.Bind("AllKillsEnable", Plugin.TabControl.checkAllKillsEnable);
            this.Bind("ViewVisible", Plugin.TabControl.checkViewVisible);
            this.Bind("ViewMouseEnable", Plugin.TabControl.checkViewMouseEnable);
            this.Bind("ViewX", Plugin.TabControl.udViewX);
            this.Bind("ViewY", Plugin.TabControl.udViewY);
            // 互換性のため OpacityPercentage で保存
            //settings.AddControlSetting("Opacity", ctl.trackBarOpacity);
            SettingsSerializer.AddIntSetting("OpacityPercentage");
        }
    }
}
