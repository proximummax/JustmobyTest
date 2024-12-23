using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Game.Scripts.Box
{
    public class BoxesScrollerView : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        private GameObject _mask;
        [Inject]
        private void Init()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        public void SetScrollAvailableState(bool available)
        {
            _scrollRect.enabled = available;
        }
    }
}