using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBank : MonoBehaviour
{
    public GameObject[] treeModels;
    public GameObject arborModel;
    public GameObject bankModel;

    public GameObject getTree() => treeModels[Random.Range(0, treeModels.Length)];
}
