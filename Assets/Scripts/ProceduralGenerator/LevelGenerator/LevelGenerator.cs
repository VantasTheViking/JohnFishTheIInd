using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField, Min(1), Tooltip("Number of Rooms & Connectors to be Generator. *Not including Spawn & Objective")] 
    int _levelSize = 1;

    [SerializeField]
    MapType _mapType;

    [SerializeField]
    List<GameObject> rooms = new List<GameObject>();


    private void Start()
    {

        SpawnRoom(_mapType.Spawn, transform.position, transform.rotation);
        if (CheckSpaceForRoomSpawning(_mapType.IntermediateRooms[0], transform.position, transform.rotation)) { Debug.Log("CANSPAWN"); SpawnRoom(_mapType.IntermediateRooms[0], transform.position, transform.rotation); } else { Debug.Log("CANTSPAWN"); }

    }


    public void GenerateLevel()
    {
        validateMapData();

        
    }

    bool CheckSpaceForRoomSpawning(GameObject roomPrefab, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        
        Bounds allPrefabBounds = GetRoomBounds(roomPrefab);
        Vector3 halfExtents = allPrefabBounds.extents;
        Collider[] hits = Physics.OverlapBox(spawnPosition + allPrefabBounds.center, halfExtents, spawnRotation);
        
        if(hits.Length == 0 ) {return true;} else {return false;}

    }

    void SpawnRoom(GameObject roomPrefab, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject spawnedRoom = Instantiate(roomPrefab,spawnPosition,spawnRotation);
        rooms.Add(spawnedRoom);
    }

    /// <summary>
    /// Returns the bounds of the room using all the colliders of the prefab.
    /// </summary>
    Bounds GetRoomBounds(GameObject roomPrefab)
    {
        Collider[] colliders = roomPrefab.GetComponentsInChildren<Collider>();

        //If there are no colliders return nothing
        if (colliders.Length == 0) { return new Bounds(roomPrefab.transform.position, Vector3.zero); }

        Bounds allBounds = colliders[0].bounds;
        for (int i = 1; i < colliders.Length; i++)
        {
            allBounds.Encapsulate(colliders[i].bounds);
        }

        return allBounds;
    }

    void validateMapData()
    {
        foreach(GameObject room in _mapType.IntermediateRooms)
        {
            if (room.GetComponent<Room>() == null)
            {
                throw new System.Exception($"{room.name} is missing a Room Component");
            }
        }

        foreach (GameObject room in _mapType.Connector)
        {
            if (room.GetComponent<Room>() == null)
            {
                throw new System.Exception($"{room.name} is missing a Room Component");
            }
        }

        if (_mapType.Spawn.GetComponent<Room>() == null)
        {
            throw new System.Exception($"{_mapType.Spawn.name} is missing a Room Component");
        }

        if (_mapType.Objective.GetComponent<Room>() == null)
        {
            throw new System.Exception($"{_mapType.Objective.name} is missing a Room Component");
        }
    }
}
