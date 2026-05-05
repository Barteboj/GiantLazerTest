using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class LayoutSaveController : MonoBehaviour, ILayoutSaveController
{
    [SerializeField]
    private LibraryItem[] libraryItemPrefabs;
    [SerializeField]
    private GameObject portsConnectionPrefab;

    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void SaveLayout()
    {
        List<LibraryItemDTO> itemsToSave = new List<LibraryItemDTO>();
        var items = FindObjectsByType<LibraryItem>(FindObjectsSortMode.None);

        foreach (var item in items)
        {
            var outputPortsConnections = new List<PortDTO>();

            foreach (var port in item.Ports)
            {
                if (port.Type == PortType.Output)
                {
                    if (port.connectedPort != null)
                    {
                        outputPortsConnections.Add(new PortDTO
                        {
                            ConnectedItemType = port.connectedPort.GetComponentInParent<LibraryItem>().ItemType,
                            ConnectedPortIndex = port.connectedPort.GetComponentInParent<LibraryItem>().Ports.ToList().IndexOf(port.connectedPort)
                        });
                    }
                    else
                    {
                        outputPortsConnections.Add(new PortDTO
                        {
                            ConnectedPortIndex = -1
                        });
                    }
                }
            }

            itemsToSave.Add(new LibraryItemDTO
            {
                ItemType = item.ItemType,
                Position = new SerializableVector3(item.transform.position),
                OutputPortsConnections = outputPortsConnections.ToArray()
            });
        }

        string json = JsonConvert.SerializeObject(itemsToSave, Formatting.Indented);
        Debug.Log(json);

        try
        {
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Saved successfully to: {saveFilePath}");
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

            List<LibraryItem> instantiatedItems = new List<LibraryItem>();
            foreach (var itemDTO in itemsDTO)
            {
                var prefab = libraryItemPrefabs.FirstOrDefault(p => p.ItemType == itemDTO.ItemType);
                if (prefab != null)
                {
                    var newItem = Instantiate(prefab, itemDTO.Position.ToVector3(), Quaternion.identity);
                    instantiatedItems.Add(newItem);
                }
            }

            foreach (var itemDTO in itemsDTO)
            {
                var instantiatedItem = instantiatedItems.FirstOrDefault(i => i.ItemType == itemDTO.ItemType);

                if (instantiatedItem != null)
                {
                    int portIndex = -1;

                    for (int i = 0; i < itemDTO.OutputPortsConnections.Length; i++)
                    {
                        var portDTO = itemDTO.OutputPortsConnections[i];
                        portIndex = instantiatedItem.Ports.ToList().FindIndex(portIndex + 1, p => p.Type == PortType.Output);

                        if (portDTO.ConnectedPortIndex != -1)
                        {
                            var connectedItem = instantiatedItems.FirstOrDefault(it => it.ItemType == portDTO.ConnectedItemType);
                            var partsConnectionController = Instantiate(portsConnectionPrefab).GetComponent<PortConnectionController>();
                            partsConnectionController.Initialize(instantiatedItem.Ports[portIndex], connectedItem.Ports[portDTO.ConnectedPortIndex]);
                        }
                    }
                }
            }
        }
    }
}