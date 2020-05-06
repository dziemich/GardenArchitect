using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MazeConstructor))]
public class GameController : MonoBehaviour
{
    [SerializeField] private FpsMovement player;
    [SerializeField] private int mazeRows;
    [SerializeField] private int mazeCols;
    [SerializeField] private int treeCount;
    [SerializeField] private int bankCount;
    [SerializeField] private int arborCount;
    [SerializeField] private GameObject terrainPrefab;

    private GameObject trees;
    private GameObject arbors;
    private GameObject banks;
    
    private MazeConstructor generator;
    private ObjectBank objectBank;
    private List<Bounds> boundsTaken = new List<Bounds>();
    private GameObject terrain;

    private float xExt;
    private float zExt;

    void Start()
    {
        generator = GetComponent<MazeConstructor>();
        objectBank = FindObjectOfType<ObjectBank>();
        trees = new GameObject("Trees");
        arbors = new GameObject("Arbors");
        banks = new GameObject("Banks");
        StartNewGame();
    }

    private void StartNewGame()
    {
        terrain = Instantiate(terrainPrefab, Vector3.zero, terrainPrefab.transform.rotation);
        var mr = terrain.GetComponent<MeshRenderer>();
        if (mr == null)
        {
            mr = terrain.GetComponentInChildren<MeshRenderer>();
        }
        
        var terrainBoundsExtents = mr.bounds.extents;
        xExt = terrainBoundsExtents.x;
        zExt = terrainBoundsExtents.z;
        StartNewMaze();
    }

    private void StartNewMaze()
    {
        var maze = generator.GenerateNewMaze(mazeRows, mazeCols);
        var mazeX = Random.Range(0.2f * xExt * -1, 0.2f * xExt);
        var mazeZ = Random.Range(0.2f * zExt * -1, 0.2f * zExt);

        maze.transform.position = new Vector3(mazeX, maze.transform.position.y, mazeZ);
        boundsTaken.Add(maze.GetComponent<MeshRenderer>().bounds);

        for (int i = 0; i < treeCount; i++)
        {
            CreateTree();
        }

        for (int i = 0; i < arborCount; i++)
        {
            CreateArbor();
        }

        for (int i = 0; i < bankCount; i++)
        {
            CreateBank();
        }


        float x = generator.startCol * generator.hallWidth;
        float y = 1;
        float z = generator.startRow * generator.hallWidth;
        player.transform.position = new Vector3(x, y, z);

        player.enabled = true;
    }

    private void CreateBank()
    {
        CreateObject(objectBank.bankModel, trees);
    }

    private void CreateArbor()
    {
        CreateObject(objectBank.arborModel, arbors);
    }

    private void CreateTree()
    {
        CreateObject(objectBank.getTree(), banks);
    }

    void CreateObject(GameObject go, GameObject parent)
    {
        float randX = 0.0f;
        // float randY = 0.0f;
        float randZ = 0.0f;

        GenerateRandomPos(out randX, out randZ);

        while (IsInBoundsOfOtherObject(randX, randZ))
        {
            GenerateRandomPos(out randX, out randZ);
        }

        RaycastHit rh = new RaycastHit();
        Ray ray = new Ray(new Vector3(randX, 0.0f, randZ) + Vector3.up * 100, Vector3.down);

        if (Physics.Raycast(ray, out rh, Mathf.Infinity))
        {
            if (rh.collider != null)
            {
                var pos = new Vector3(randX, rh.point.y, randZ);
                // var pos = new Vector3 (randX, rh.point.y + go.GetComponent<MeshRenderer>().bounds.extents.y, randZ);
                var newObject = Instantiate(go, pos, go.transform.rotation, parent.transform);
                boundsTaken.Add(newObject.GetComponent<MeshRenderer>().bounds);
            }
        }
    }

    private bool IsInBoundsOfOtherObject(float randX, float randZ) =>
        boundsTaken.Any(b => b.Contains(new Vector3(randX, b.center.y, randZ)));

    private void GenerateRandomPos(out float randX, out float randZ)
    {
        randX = Random.Range(0.9f * xExt * -1, 0.9f * xExt);
        randZ = Random.Range(0.9f * zExt * -1, 0.9f * zExt);
    }
}