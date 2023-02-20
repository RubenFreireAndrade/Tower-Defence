using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Color pathColor;
    public GameObject cube;

    public int gridX;
    public int gridZ;
    public float gridSpacingOffset;

    public static GameObject startCube;
    public static GameObject finishCube;
    private GameObject currentCube;

    public static List<GameObject> cubeGrids = new List<GameObject>();
    public static List<GameObject> pathGrids = new List<GameObject>();

    private int nextIndex;
    private int currentIndex;
    private int loopCount = 0;
    private bool hasReachedX = false;
    private bool hasReachedY = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateMap()
    {
        for (int z = 0; z < gridZ; z++)
        {
            for (int x = 0; x < gridX; x++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + this.transform.position;
                Spawn(spawnPosition, Quaternion.identity);
            }
        }

        List<GameObject> topEdgeGrids = GetTopEdgeGrids();
        List<GameObject> bottomEdgeGrids = GetBottomEdgeGrids();

        int randX = Random.Range(0, gridX);
        int randZ = Random.Range(0, gridX);

        startCube = topEdgeGrids[randX];
        finishCube = bottomEdgeGrids[randZ];

        currentCube = startCube;

        // Calling MoveDown() to avoid the path hitting edges of the map.
        MoveDown();

        while (!hasReachedX)
        {
            loopCount++;
            if (loopCount > 100)
            {
                Debug.Log("Loop ran too long and now broke out");
                break;
            }

            if (currentCube.transform.position.x > finishCube.transform.position.x) MoveLeft();
            else if(currentCube.transform.position.x < finishCube.transform.position.x) MoveRight();
            else hasReachedX = true;
        }

        while (!hasReachedY)
        {
            if (currentCube.transform.position.z > finishCube.transform.position.z) MoveDown();
            else hasReachedY = true;
        }

        pathGrids.Add(finishCube);

        foreach (GameObject obj in pathGrids)
        {
            obj.GetComponent<Renderer>().material.color = pathColor;
        }

        SetStartColor(startCube);
        SetFinishColor(finishCube);
    }

    private void Spawn(Vector3 spawnPos, Quaternion spawnRot)
    {
        GameObject clone = Instantiate(cube, spawnPos, spawnRot);
        cubeGrids.Add(clone);
    }

    private List<GameObject> GetTopEdgeGrids()
    {
        List<GameObject> edgeGrids = new List<GameObject>();

        for (int i = gridX * (gridZ - 1); i < gridX * gridZ; i++)
        {
            edgeGrids.Add(cubeGrids[i]);
        }
        return edgeGrids;
    }

    private List<GameObject> GetBottomEdgeGrids()
    {
        List<GameObject> edgeGrids = new List<GameObject>();

        for (int i = 0; i < gridX; i++)
        {
            edgeGrids.Add(cubeGrids[i]);
        }
        return edgeGrids;
    }

    private void MoveDown()
    {
        pathGrids.Add(currentCube);
        currentIndex = cubeGrids.IndexOf(currentCube);
        nextIndex = currentIndex - gridX;
        currentCube = cubeGrids[nextIndex];
    }

    private void MoveLeft()
    {
        pathGrids.Add(currentCube);
        currentIndex = cubeGrids.IndexOf(currentCube);
        nextIndex = currentIndex - 1;
        currentCube = cubeGrids[nextIndex];
    }

    private void MoveRight()
    {
        pathGrids.Add(currentCube);
        currentIndex = cubeGrids.IndexOf(currentCube);
        nextIndex = currentIndex + 1;
        currentCube = cubeGrids[nextIndex];
    }

    private void SetStartColor(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.green;
    }

    private void SetFinishColor(GameObject obj)
    {
        obj.GetComponent<Renderer>().material.color = Color.red;
    }
}
