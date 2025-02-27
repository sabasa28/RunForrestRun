using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    List<CoolerTree> SpawnedTrees = new List<CoolerTree>();
    [SerializeField] float yToSpawnTrees;
    [SerializeField] float minDistanceBetweenTrees;
    [SerializeField] float minDistanceToHouse;
    [SerializeField] CoolerTree treePrefab;
    [SerializeField] Vector2 minLimit;
    [SerializeField] Vector2 maxLimit;
    static TreeManager instance;
    Vector3 housePosition;

    public static TreeManager Get()
    {
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        SpawnedTrees.AddRange(FindObjectsOfType<CoolerTree>());
        housePosition = FindObjectOfType<House>().transform.position;
    }

    // devuelve true si se pudo spawnear el arbol
    public bool TrySpawnTree(Vector3 PosToSpawn)
    {
        PosToSpawn = new Vector3(PosToSpawn.x, yToSpawnTrees, PosToSpawn.z);
        foreach (CoolerTree SpawnedTree in SpawnedTrees)
        {
            if (Vector3.Distance(SpawnedTree.transform.position, PosToSpawn) < minDistanceBetweenTrees)
            {
                //play 
                return false;
            }
        }
        if (Vector3.Distance(PosToSpawn, housePosition) < minDistanceToHouse || !PosIsWithinLimits(PosToSpawn))
        {
            return false;
        }
        SpawnTree(PosToSpawn);
        return true;
    }

    bool PosIsWithinLimits(Vector3 pos)
    {
        return (pos.x > minLimit.x && pos.x < maxLimit.x && pos.z > minLimit.y && pos.z < maxLimit.y);
    }

    void SpawnTree(Vector3 PosToSpawn)
    {
        SpawnedTrees.Add(Instantiate<CoolerTree>(treePrefab, PosToSpawn, Quaternion.identity));
    }

    public void DespawnTree(CoolerTree tree)
    {
        SpawnedTrees.Remove(tree);
        Destroy(tree.gameObject);
    }
    public float GetMinDistanceBetweenTrees()
    {
        return minDistanceBetweenTrees;
    }
}
