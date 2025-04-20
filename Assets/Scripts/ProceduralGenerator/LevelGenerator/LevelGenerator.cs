using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField, Min(1), Tooltip("Number of Rooms & Connectors to be Generator. *Not including Spawn & Objective")] 
    int _levelSize = 1;

    [SerializeField]
    MapType _mapType;



    public void GenerateLevel()
    {
        validateMapData();
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
