using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BuildingData
{
    public string id;
    public GameObject building;
    public GameObject buildingPreview;
}

[Serializable]
public class BuildingSaveData
{
    public string id;
    public Vector2 position;
}