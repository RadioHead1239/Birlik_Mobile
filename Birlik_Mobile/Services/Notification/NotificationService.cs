using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace Birlik_Mobile.Services
{
    public class NotificationService
    {
        /// <summary>
        /// Muestra una notificación local inmediata (o muy próxima).
        /// </summary>
        public async Task ShowNotificationAsync(string title, string message, int delaySeconds = 1)
        {
            var request = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = title,
                Description = message,

                Android = new AndroidOptions
                {
                    IconSmallName = new AndroidIcon("birlik_icono")
                },

                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(delaySeconds)
                }
            };

            await LocalNotificationCenter.Current.Show(request);
        }
    }
}
