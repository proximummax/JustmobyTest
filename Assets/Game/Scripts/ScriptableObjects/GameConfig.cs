using UnityEngine;

namespace Game.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private Color[] _boxColors;
        public Color[] BoxColors => _boxColors;

        [SerializeField] private int _boxesCount;
        public int BoxesCount => _boxesCount;

        [SerializeField] private float _fallDownDuration;
        public float FallDownDuration => _fallDownDuration;
        
        [SerializeField] private float _hideDuration;
        public float HideDuration => _hideDuration;
        
        [SerializeField] private float _backToTowerDuration;
        public float BackToTowerDuration => _backToTowerDuration;
    }
}