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
    List<Room> rooms = new List<Room>();

    [SerializeField]
    Room startingRoom;
    private void Start()
    {
        startingRoom = GetComponent<Room>();
        rooms.Add(startingRoom);
        
        GenerateLevel();
       
    }


    public void GenerateLevel()
    {


        validateMapData();

        //Loop through all the rooms to see if all connectors are filled
        
            //If connector not filled
                //Create list of possible rooms to connect to

                //Select Random Room to Fill
                //Select Random Connector on Room
                //Check all 4 rotations to see if any won't cause an overlap
                //If can't spawn at all select another random connector
                //If no connectors can fill select another random room minus the old
                //If no room can fill it close it up and set the connector as connected





        startingRoom.SpawnRoom(
            //The Type of Room
            _mapType.IntermediateRooms[0],
            //Where they will be connecting
            startingRoom.GetConnectors()[0].connector.transform.position - _mapType.IntermediateRooms[0].GetComponent<Room>().GetConnectors()[0].connector.transform.position,
            //The rotation of where they will be connecting
            startingRoom.GetConnectors()[0].connector.transform.rotation
        );
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
