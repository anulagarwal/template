using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnData", menuName = "Custom/Spawn List", order = 1)]
public class DataList : ScriptableObject
{
    public List<StackSpawnData> dataList = new List<StackSpawnData>();
}