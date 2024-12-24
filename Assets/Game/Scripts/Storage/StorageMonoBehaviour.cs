using UnityEngine;
using VContainer;

namespace Game.Scripts.Storage
{
    public class StorageMonoBehaviour : MonoBehaviour
    {
        private StorageBoxesService _storageBoxesService;
        
        [Inject]
        public void Init(StorageBoxesService storageBoxesService)
        {
            _storageBoxesService = storageBoxesService;
        }
        private void OnApplicationQuit()
        {
            _storageBoxesService.SaveGame(true);
        }
    }
}