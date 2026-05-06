using System.Collections.Generic;
using System.IO;
using System.Linq;
using GiantLaserTest.Core.Desk;
using GiantLaserTest.Core.Library;
using GiantLaserTest.Core.Ports;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GiantLaserTest.Core.Save
{
    public class LayoutSaveController : MonoBehaviour, ILayoutSaveController
    {
        [Header("References")]
        [SerializeField]
        private DeskController deskController;
        [SerializeField]
        private LibraryItemPrefabsContainerSO libraryItemPrefabsContainer;
        [SerializeField]
        private GameObject portsConnectionPrefab;

        private string saveFilePath;

        private void Awake()
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        }

        public void SaveLayout()
        {
            var saveData = GetDataForSaving();
            string saveJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);

            try
            {
                File.WriteAllText(saveFilePath, saveJson);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error saving: {e.Message}");
            }
        }

        public void LoadLayout()
        {
            if (!File.Exists(saveFilePath))
            {
                Debug.LogWarning("No save file found!");
            }
            else
            {
                List<LibraryItemDTO> itemsDTO = null;
                try
                {
                    string json = File.ReadAllText(saveFilePath);
                    itemsDTO = JsonConvert.DeserializeObject<List<LibraryItemDTO>>(json);

                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error reading: {e.Message}");
                }

                RemoveExistingElements();
                List<LibraryItem> instantiatedItems = new List<LibraryItem>();
                PlaceElementsOnDesk(itemsDTO, instantiatedItems);
                MakePortsConnections(itemsDTO, instantiatedItems);
            }
        }

        private List<LibraryItemDTO> GetDataForSaving()
        {
            List<LibraryItemDTO> itemsToSave = new List<LibraryItemDTO>();
            var items = deskController.LibraryItems;

            foreach (var item in items)
            {
                var outputPortsConnections = new List<PortDTO>();

                foreach (var port in item.Ports)
                {
                    if (port.Type == PortType.Output)
                    {
                        if (port.connectedPort != null)
                        {
                            outputPortsConnections.Add(new PortDTO(port.connectedPort.GetComponentInParent<LibraryItem>().ItemType,
                                port.connectedPort.GetComponentInParent<LibraryItem>().Ports.ToList().IndexOf(port.connectedPort)));
                        }
                        else
                        {
                            outputPortsConnections.Add(new PortDTO(LibraryItemType.RawMaterialTankA, -1));
                        }
                    }
                }

                itemsToSave.Add(new LibraryItemDTO(new SerializableVector3(item.GameObject.transform.position), item.ItemType, outputPortsConnections));
            }

            return itemsToSave;
        }

        private void RemoveExistingElements()
        {
            var items = SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<ILibraryItem>());

            foreach (var item in items)
            {
                Destroy(item.GameObject);
            }

            var portsConnections = SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<PortsConnectionController>());

            foreach (var connection in portsConnections)
            {
                Destroy(connection.gameObject);
            }
        }

        private void PlaceElementsOnDesk(List<LibraryItemDTO> itemsDTO, List<LibraryItem> instantiatedItems)
        {
            foreach (var itemDTO in itemsDTO)
            {
                var prefab = libraryItemPrefabsContainer.LibraryItemPrefabs.Find(p => p.GetComponent<LibraryItem>().ItemType == itemDTO.ItemType);
                var newItem = Instantiate(prefab, itemDTO.Position.ToVector3(), Quaternion.identity).GetComponent<LibraryItem>();
                deskController.RegisterLibraryItem(newItem);
                instantiatedItems.Add(newItem);
            }
        }

        private void MakePortsConnections(List<LibraryItemDTO> itemsDTO, List<LibraryItem> instantiatedItems)
        {
            foreach (var itemDTO in itemsDTO)
            {
                var instantiatedItem = instantiatedItems.Find(i => i.ItemType == itemDTO.ItemType);
                int portIndex = -1;

                foreach (var portDTO in itemDTO.OutputPortsConnections)
                {
                    portIndex = instantiatedItem.Ports.FindIndex(portIndex + 1, p => p.Type == PortType.Output);

                    if (portDTO.ConnectedPortIndex != -1)
                    {
                        var connectedItem = instantiatedItems.FirstOrDefault(it => it.ItemType == portDTO.ConnectedItemType);
                        var partsConnectionController = Instantiate(portsConnectionPrefab).GetComponent<PortsConnectionController>();
                        partsConnectionController.Initialize(instantiatedItem.Ports[portIndex], connectedItem.Ports[portDTO.ConnectedPortIndex]);
                    }
                }
            }
        }
    }
}