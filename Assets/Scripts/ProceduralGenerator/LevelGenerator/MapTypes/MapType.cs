using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapType", menuName = "Scriptable Objects/MapType")]
public class MapType : ScriptableObject
{
    public List<GameObject> IntermediateRooms;
    public List<GameObject> Connector;
    public GameObject Objective;
    public GameObject Spawn;
}
