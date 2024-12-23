using UniRx;
using VContainer.Unity;

namespace Game.Scripts.Notifications
{
    public class NotificationService : IStartable
    {
        private readonly NotificationView _notificationView;
        public ReactiveProperty<string> NotificationMessage { get; private set; } = new();

        private NotificationService(NotificationView notificationView)
        {
            _notificationView = notificationView;
        }

        public void Start()
        {
            NotificationMessage.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(xs => { _notificationView.ShowNotification(xs); }).AddTo(_notificationView);
        }
    }
}