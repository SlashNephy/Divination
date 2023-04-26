using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts;

namespace Divination.Common.Toast
{
    public static class ToastNotificationEx
    {
        private const string DefaultAppId = "Advanced Combat Tracker";
        private static readonly ToastNotifier Notifier;

        static ToastNotificationEx()
        {
            Notifier = ToastNotificationManager.CreateToastNotifier(DefaultAppId);
        }

        public static ToastNotification Show(this ToastContent content)
        {
            var doc = new XmlDocument();
            doc.LoadXml(content.GetContent());

            var toast = new ToastNotification(doc);
            Notifier.Show(toast);

            return toast;
        }

        public static void Dismiss(this ToastNotification toast)
        {
            Notifier.Hide(toast);
        }
    }
}
