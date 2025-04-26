using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
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

    [SerializeField]
    int generationSize;


    bool allConnectorsFilled = false;
    int currentSize = 0;

    private void Start()
    {
        startingRoom = GetComponent<Room>();
        rooms.Add(startingRoom);
        startingRoom.isSpawnRoom = true;


        validateMapData();




    }

    private void Update()
    {
        GenerateLevel();
    }
    /*
    public void GenerateLevel()
    {


        validateMapData();

        //Loop through all the rooms to see if all connectors are filled
        bool allConnectorsFilled = false;
        int currentSize = 0;
        while (!allConnectorsFilled && currentSize <= generationSize)
        {
            //Debug.Log("AGAIN");
            currentSize++;
            allConnectorsFilled = true;

            List<Room> roomsToCheck = new List<Room>(rooms);
            //Debug.Log("Room Count:" + rooms.Count);
            foreach (Room room in roomsToCheck)
            {
                //Debug.Log("A");
                

                foreach (ConnectorStatus connectorStatus in room.GetConnectors())
                {

                    //Debug.Log("B");
                    //If connector not filled
                    if (!connectorStatus.isConnected)
                    {
                        //Debug.Log("C");
                        allConnectorsFilled = false;

                        FillRoomConnections(room, connectorStatus);


                    }


                }
            }
        }


        //If connector not filled
        //Create list of possible rooms to connect to
        //Select Random Connector on Room
        //Check all 4 rotations to see if any won't cause an overlap
        //If can't spawn at all select another random connector
        //If no connectors can fill select another random room minus the old
        //If no room can fill it close it up and set the connector as connected



        /*
        Vector3 pos = NewPositionAfterYRotated(_mapType.IntermediateRooms[0].GetComponent<Room>().GetConnectors()[1].connector.transform.position, 270);

        GameObject newRoom = startingRoom.SpawnRoom(
            //The Type of Room
            _mapType.IntermediateRooms[0],
            //Where they will be connecting
            startingRoom.GetConnectors()[0].connector.transform.position - pos,
            //The rotation of where they will be connecting
            startingRoom.GetConnectors()[0].connector.transform.rotation * Quaternion.Euler(0, 270, 0)
        );
        

    }
    */

    public void GenerateLevel()
    {



        //Loop through all the rooms to see if all connectors are filled
        
        if (!allConnectorsFilled && currentSize <= generationSize)
        {
            //Debug.Log("AGAIN");
            currentSize++;
            allConnectorsFilled = true;

            List<Room> roomsToCheck = new List<Room>(rooms);
            //Debug.Log("Room Count:" + rooms.Count);
            foreach (Room room in roomsToCheck)
            {
                //Debug.Log("A");


                foreach (ConnectorStatus connectorStatus in room.GetConnectors())
                {

                    //Debug.Log("B");
                    //If connector not filled
                    if (!connectorStatus.isConnected)
                    {
                        //Debug.Log("C");
                        allConnectorsFilled = false;

                        FillRoomConnections(room, connectorStatus);


                    }


                }
            }
        } else { GetComponent<LevelGenerator>().enabled = false; }


        //If connector not filled
        //Create list of possible rooms to connect to
        //Select Random Connector on Room
        //Check all 4 rotations to see if any won't cause an overlap
        //If can't spawn at all select another random connector
        //If no connectors can fill select another random room minus the old
        //If no room can fill it close it up and set the connector as connected



        
        

    }
    

        void FillRoomConnections(Room room, ConnectorStatus connector)
    {
        //Create list of possible rooms to connect to
        List<GameObject> possibleRooms = new List<GameObject>();
        if (room.GetRoomType() == Room.RoomType.Hallway)
        {
            //Chance for hallways to be longer
            if(UnityEngine.Random.Range(0,1) == 0)
            {

                possibleRooms = new List<GameObject>(_mapType.IntermediateRooms);
            } else
            {

                possibleRooms = new List<GameObject>(_mapType.Connector);
            }
        }
        else if(room.GetRoomType() == Room.RoomType.Intermediate)
        {

            possibleRooms = new List<GameObject>(_mapType.Connector);
        }

        //Iterate through all the possible rooms randomly
        bool roomSpawned = false;
        while (possibleRooms.Count > 0 && !roomSpawned)
        {
            GameObject selectedRoom = possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Count)];
            List<ConnectorStatus> selectedRoomConnectors = selectedRoom.GetComponent<Room>().GetConnectors();
            
            //Foreach connector in the selected room
            foreach (ConnectorStatus selectedRoomConnector in selectedRoomConnectors)
            {
                
                //Debug.Log($"{ selectedRoom.name}, {room.gameObject.name}, {connector.connector.gameObject.name}");
                //Try fitting through all the possible rotations
                for (int rotationTry = 0; rotationTry < 4; rotationTry++)
                {
                    if (!roomSpawned || true)
                    {

                        //Debug.Log("RotationsTry");
                        //Position of the connector of the selected room when rotated
                        Vector3 pos = NewPositionAfterYRotated(selectedRoomConnector.connector.transform.position, rotationTry * 90);
                        
                        
                        if (room.CheckSpaceForRoomSpawning(
                                selectedRoom,
                                connector.connector.transform.position - pos,
                                pos,
                                selectedRoomConnector.connector.transform.position,
                                rotationTry
                                ))
                        {
                            Debug.Break();
                            roomSpawned = true;
                            Debug.Log("SPAWN");
                            GameObject newRoom = startingRoom.SpawnRoom(
                                //The Type of Room
                                selectedRoom,
                                //Where they will be connecting
                                connector.connector.transform.position - pos,
                                //The rotation of where they will be connecting
                                connector.connector.transform.rotation = Quaternion.Euler(0, rotationTry * 90, 0)
                            );
                            //Add new room to the list of all rooms
                            rooms.Add(newRoom.GetComponent<Room>());

                            Room newRoomComponent = newRoom.GetComponent<Room>();

                            // Find the matching connector in the new room
                            List<ConnectorStatus> newRoomConnectors = newRoomComponent.GetConnectors();
                            ConnectorStatus newConnector = FindClosestConnector(newRoomConnectors, connector.connector.transform.position);

                            // Set actual spawned connectors to connected
                            connector.isConnected = true;
                            //Debug.Log("newConnector:" + newConnector.connector.name);
                            newConnector.isConnected = true;
                            Debug.Log(selectedRoomConnector.connector.transform.position);

                        }
                        else { Debug.Log("CantSpawn"); }

                    }
                }
                
            }
            


            //If the selected room can't fit in any way. Remove it from the possible rooms
            possibleRooms.Remove(selectedRoom);




        }

        //If there are no possible rooms to fill close the connector
        if (!roomSpawned)
        {
            Debug.Log("NoPossible");
            connector.isConnected = true;
        }


    }

    ConnectorStatus FindClosestConnector(List<ConnectorStatus> connectors, Vector3 targetPos)
    {
        ConnectorStatus closest = null;
        float closestDist = float.MaxValue;

        foreach (ConnectorStatus c in connectors)
        {
            float dist = Vector3.Distance(c.connector.transform.position, targetPos);
            if (dist < closestDist)
            {
                closest = c;
                closestDist = dist;
            }
        }

        return closest;
    }


    List<ConnectorStatus> GetRandomConnectors(List<ConnectorStatus> connectors)
    {
        List<ConnectorStatus> copy = new List<ConnectorStatus>(connectors);
        for (int i = copy.Count - 1; i >= 0; i--)
        {
            if (copy[i].isConnected)
            {
                copy.RemoveAt(i);
            }
        }


        List<ConnectorStatus> result = new List<ConnectorStatus>();

        int countToPick = UnityEngine.Random.Range(1, copy.Count);

        for (int i = 0; i < countToPick; i++)
        {
            int index = UnityEngine.Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        foreach (ConnectorStatus connector in copy)
        {
            connector.isConnected = true;
        }

        return result;
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

    Vector3 NewPositionAfterYRotated(Vector3 position,float deltaAngle)
    {
        
        float angleInRadians = deltaAngle * Mathf.Deg2Rad;
        float xNew = position.x * Mathf.Cos(angleInRadians) + position.z * Mathf.Sin(angleInRadians);
        float yNew = position.y;
        float zNew = -position.x * Mathf.Sin(angleInRadians) + position.z * Mathf.Cos(angleInRadians);

        return new Vector3( xNew, yNew, zNew );


    }
}
