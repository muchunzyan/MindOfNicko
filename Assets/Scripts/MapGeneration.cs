using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private List<GameObject> roomsPrefabs;
    [SerializeField] private int numberOfRooms;
    private readonly List<GameObject> _roomsGameObjects = new List<GameObject>();

    [SerializeField]
    private List<GameObject> possibleRoomsForL, possibleRoomsForR, possibleRoomsForU, possibleRoomsForD;

    [SerializeField] private Transform roomsParent;

    private void Start()
    {
        GenerateRooms();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
    }

    private void GenerateRooms()
    {
        var firstRoom = Instantiate(roomsPrefabs[Random.Range(0, roomsPrefabs.Count - 1)], roomsParent, true);
        _roomsGameObjects.Add(firstRoom);

        for (var i = 0; i < numberOfRooms; i++)
        {
            if (i >= _roomsGameObjects.Count)
                break;
            
            var lastRoom = _roomsGameObjects[i];
            var pointsList = lastRoom.transform.Find("Points");
            foreach (Transform point in pointsList)
            {
                var skipRoomGen = false;
                foreach (var room in _roomsGameObjects)
                {
                    if (room.transform.position == point.position)
                    {
                        skipRoomGen = true;
                        break;
                    }
                }

                if (skipRoomGen)
                    continue;

                var side = point.gameObject.GetComponent<PointController>().side;

                GameObject newRoom = null;
                switch (side)
                {
                    case "L":
                        newRoom = Instantiate(possibleRoomsForL[Random.Range(0, possibleRoomsForL.Count - 1)],
                            point.position, Quaternion.identity);
                        newRoom.transform.parent = roomsParent;
                        break;
                    case "R":
                        newRoom = Instantiate(possibleRoomsForR[Random.Range(0, possibleRoomsForR.Count - 1)],
                            point.position, Quaternion.identity);
                        newRoom.transform.parent = roomsParent;
                        break;
                    case "U":
                        newRoom = Instantiate(possibleRoomsForU[Random.Range(0, possibleRoomsForU.Count - 1)],
                            point.position, Quaternion.identity);
                        newRoom.transform.parent = roomsParent;
                        break;
                    case "D":
                        newRoom = Instantiate(possibleRoomsForD[Random.Range(0, possibleRoomsForD.Count - 1)],
                            point.position, Quaternion.identity);
                        newRoom.transform.parent = roomsParent;
                        break;
                }

                _roomsGameObjects.Add(newRoom);
            }
        }

        TurnOnWalls();
    }

    private void TurnOnWalls()
    {
        foreach (var room in _roomsGameObjects)
        {
            var pointsList = room.transform.Find("Points");
            foreach (Transform point in pointsList)
            {
                var createBlock = true;

                var pSide = point.gameObject.GetComponent<PointController>().side;
                
                foreach (var room2 in _roomsGameObjects)
                {
                    if (room2.transform.position == point.position)
                    {
                        var room2Directions = new List<string>();
                        
                        var points2List = room2.transform.Find("Points");
                        foreach (Transform point2 in points2List)
                        {
                            var p2Side = point2.gameObject.GetComponent<PointController>().side;
                            room2Directions.Add(p2Side);
                        }

                        if (pSide == "L" && !room2Directions.Contains("R") ||
                            pSide == "R" && !room2Directions.Contains("L") ||
                            pSide == "U" && !room2Directions.Contains("D") ||
                            pSide == "D" && !room2Directions.Contains("U"))
                        {
                            room.transform.Find("BlockWalls").Find($"BlockWall_{pSide}").gameObject.SetActive(true);   
                        }
                        
                        createBlock = false;
                        break;
                    }
                }

                if (createBlock)
                {
                    var side = point.gameObject.GetComponent<PointController>().side;
                    room.transform.Find("BlockWalls").Find($"BlockWall_{side}").gameObject.SetActive(true);
                }
            }
        }
    }
}
