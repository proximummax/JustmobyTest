using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Game.Scripts.Box;
using Game.Scripts.DropZone;
using UnityEngine;

namespace Game.Scripts.Storage
{
    [System.Serializable]
    public class BoxSaveParameters
    {
        public BoxSaveParameters(Color color, Vector3 position)
        {
            Color = color;
            Position = position;
        }
        
        public Color Color;
        public Vector3 Position;
    }

    public class StorageBoxesService
    {
        private BoxView _boxViewPrefab;
        private Transform _boxesParent;
        private readonly TowerZoneService _towerZoneService;
        private readonly BoxesPresenter _boxesPresenter;

        private string _filePath;
        private const char SlashSymbol = '/';
        private const string SaveFileName = "save";

        public StorageBoxesService(TowerZoneService towerZoneService, BoxesPresenter boxesPresenter)
        {
            _boxesPresenter = boxesPresenter;
            _towerZoneService = towerZoneService;
            LoadGame();
        }

        private void LoadGame()
        {
            _filePath = Application.persistentDataPath;

            LoadBoxes();
        }

        public void SaveGame()
        {
            string savePath = _filePath + SlashSymbol + SaveFileName;
            File.Delete(savePath);

            XDocument xDoc;

            bool isFileExists = IsSaveExists(SaveFileName);
            if (!isFileExists)
            {
                xDoc = new XDocument();
                XElement scoreElement = SaveBoxes(isFileExists, _towerZoneService.Tower);
                xDoc.Add(scoreElement);
            }
            else
            {
                xDoc = XDocument.Load(savePath);
                SaveBoxes(isFileExists, _towerZoneService.Tower);
            }

            xDoc.Save(savePath);
        }

        private bool IsSaveExists(string saveName)
        {
            return File.Exists(_filePath + SlashSymbol + saveName);
        }

        private XElement SaveBoxes(bool fileExists, List<BoxView> boxes)
        {
            XElement saveGameElement = null;
            XElement boxesParameters = null;

            if (!fileExists)
            {
                saveGameElement = new XElement("SaveGame");
                boxesParameters = new XElement("BoxesParameters");
            }

            for (int i = 0; i < boxes.Count; i++)
            {
                BoxSaveParameters formatToBoxSave = new BoxSaveParameters(boxes[i].Color, boxes[i].PositionInTower);
                XElement boxParametersElement = new XElement("BoxParameters");
                XAttribute boxDataAttribute = new XAttribute("BoxData", JsonUtility.ToJson(formatToBoxSave));

                boxParametersElement.Add(boxDataAttribute);
                if (boxesParameters != null) boxesParameters.Add(boxParametersElement);
            }

            saveGameElement.Add(boxesParameters);

            return saveGameElement;
        }

        private void LoadBoxes()
        {
            List<BoxSaveParameters> boxesParameters = new List<BoxSaveParameters>();

            string savePath = _filePath + SlashSymbol + SaveFileName;

            if (!IsSaveExists(SaveFileName))
                return;

            XDocument xDoc = XDocument.Load(savePath);

            XElement saveGameElement = xDoc.Element("SaveGame");
            if (saveGameElement != null)
            {
                XElement boxesParametersElement = saveGameElement.Element("BoxesParameters");
                if (boxesParametersElement != null)
                    foreach (XElement boxParameters in boxesParametersElement.Elements("BoxParameters"))
                    {
                        XAttribute boxDataAttribute = boxParameters.Attribute("BoxData");
                        if (boxDataAttribute != null)
                        {
                            BoxSaveParameters formatToBoxSave =
                                JsonUtility.FromJson<BoxSaveParameters>(boxDataAttribute.Value);

                            boxesParameters.Add(formatToBoxSave);
                        }
                    }
            }

            var boxes = _boxesPresenter.CreateBoxesByLoadData(boxesParameters);
            for (int i = 0; i < boxes.Length; i++)
            {
                _towerZoneService.PlaceBoxInTower(boxes[i], boxesParameters[i].Position, true);
            }
        }
    }
}