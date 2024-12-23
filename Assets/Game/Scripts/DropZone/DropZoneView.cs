using Game.Scripts.Notifications;
using Game.Scripts.ScriptableObjects;
using Game.Scripts.Utils;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Game.Scripts.DropZone
{
    public class DropZoneView : MonoBehaviour
    {
        private enum EDropZoneType
        {
            Tower,
            Stash,
            ScrollView
        }

        [SerializeField] private EDropZoneType _zoneType;

        private float _hideDuration;
        private float _backToTowerDuration;
        private TowerZoneService _towerZoneService;
        private NotificationService _notificationService;

        [Inject]
        private void Init(GameConfig gameConfig, TowerZoneService towerZoneService,
            NotificationService notificationService)
        {
            _hideDuration = gameConfig.HideDuration;
            _backToTowerDuration = gameConfig.BackToTowerDuration;
            _towerZoneService = towerZoneService;
            _notificationService = notificationService;

            this.AddComponent<ObservableDropTrigger>().OnDropAsObservable().Subscribe(OnDrop).AddTo(this);
        }

        private void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag || !eventData.pointerDrag.TryGetComponent(out BoxView boxView)) return;
            switch (_zoneType)
            {
                case EDropZoneType.Tower:
                    if (boxView.PlacementPoint == BoxView.EBoxPlacementPoint.Tower)
                    {
                        boxView.BackToTower(_backToTowerDuration);
                        _notificationService.NotificationMessage.Value = AppMessages.ALREADY_CONNECTED_MESSAGE;
                    }
                    else
                    {
                        _towerZoneService.TryPlaceBox(boxView);
                    }

                    break;
                case EDropZoneType.Stash:
                    if (boxView.PlacementPoint != BoxView.EBoxPlacementPoint.Tower)
                    {
                        boxView.SmoothDestroy(_hideDuration);
                    }
                    else
                    {
                        _towerZoneService.RemoveBoxFromTower(boxView);
                    }

                    break;
                case EDropZoneType.ScrollView:
                    if (boxView.PlacementPoint == BoxView.EBoxPlacementPoint.Tower)
                    {
                        boxView.BackToTower(_backToTowerDuration);
                    }
                    else
                    {
                        boxView.SmoothDestroy(_hideDuration);
                    }

                    _notificationService.NotificationMessage.Value = AppMessages.ERROR_PLACED_MESSAGE;

                    break;
            }
        }
    }
}