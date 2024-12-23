using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Notifications
{
    public class NotificationLifetimeScope : LifetimeScope
    {
        [SerializeField] private NotificationView _notificationView;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_notificationView);
            builder.RegisterEntryPoint<NotificationService>().AsSelf();
        }
    }
}
