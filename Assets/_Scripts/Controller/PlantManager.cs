using System;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : PersistentSingleton<PlantManager>
{
    public Dictionary<int, PlantDataController> DctPlantData = new Dictionary<int, PlantDataController>();

    private PlantListItemView _currentPlantItem;

    public PlantListItemView CurrentPlantItem
    {
        get => _currentPlantItem;
        set => _currentPlantItem = value;
    }

    public void InstantiateDataController(string plantName)
    {
        //var dataController = Instantiate(ResourceManager.Instance.PlantDataController, transform);
        //dataController.Init(plantName);
        //DctPlantData.Add(plantName, dataController);
        //_plantNames.Add(plantName);
    }
}