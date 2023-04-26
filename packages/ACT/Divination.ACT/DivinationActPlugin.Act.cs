using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using FFXIV_ACT_Plugin.Common;

namespace Divination.ACT
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public abstract partial class DivinationActPlugin<TW, TU, TS>
    {
        public static IDataRepository DataRepository => (IDataRepository) FFXIV_ACT_Plugin.DataRepository;
        public static IDataSubscription DataSubscription => (IDataSubscription) FFXIV_ACT_Plugin.DataSubscription;
#pragma warning disable 8618
        public static string AssemblyDirectory { get; private set; }
        public static ActPluginData PluginData { get; private set; }
        public static TabPage ScreenSpace { get; private set; }
        public static Label StatusText { get; private set; }

        // ReSharper disable once InconsistentNaming
        public static dynamic FFXIV_ACT_Plugin { get; private set; }
#pragma warning restore 8618
    }
}
