using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        Intermediate,
        Hallway
    }

    private Bounds _combinedBounds;

    [SerializeField] 
    List<ConnectorStatus> _connectors;

    [SerializeField] 
    RoomType _roomType;

    void Awake()
    {


        CalculateCombinedBounds();

    }


    private void Start()
    {
        
    }

    public bool CheckSpaceForRoomSpawning(GameObject roomPrefab, Vector3 spawnPosition, Quaternion spawnRotation)
    {

        Bounds allPrefabBounds = GetRoomBounds(roomPrefab);
        Vector3 halfExtents = allPrefabBounds.extents;
        Collider[] hits = Physics.OverlapBox(spawnPosition + allPrefabBounds.center, halfExtents, spawnRotation);

        if (hits.Length == 0) { return true; } else { return false; }

    }

    public GameObject SpawnRoom(GameObject roomPrefab, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject spawnedRoom = Instantiate(roomPrefab, spawnPosition, spawnRotation);
        return spawnedRoom;
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

    void CalculateCombinedBounds()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            _combinedBounds = new Bounds(transform.position, Vector3.zero);
            return;
        }

        _combinedBounds = colliders[0].bounds;
        for (int i = 1; i < colliders.Length; i++)
        {
            _combinedBounds.Encapsulate(colliders[i].bounds);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_combinedBounds.center, _combinedBounds.size);
    }

    public List<ConnectorStatus> GetConnectors() { 
    
        return _connectors;
    
    }

    public RoomType GetRoomType() { return _roomType; }
}
