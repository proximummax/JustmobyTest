using TMPro;
using UnityEngine;

namespace Game.Scripts.Notifications
{
    public class NotificationView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void ShowNotification(string notification)
        {
            gameObject.SetActive(true);
            _text.text = notification;
        }
        
    }
}
