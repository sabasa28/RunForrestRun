using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    enum DirectionsToExpandTo
    {
        UpLeft,
        Up,
        UpRight,
        Left,
        Right,
        DownLeft,
        Down,
        DownRight,
        Num
    }
    public FireManager fireManager;
    [SerializeField] protected float minTimeToSpawn;
    [SerializeField] protected float maxTimeToSpawn;
    [SerializeField] protected float timeToExpand;
    public Vector2Int debugGridPos;
    protected int[] orderToCheck;
    protected int posibleDirections = (int)DirectionsToExpandTo.Num;
    protected virtual void Start()
    {
        orderToCheck = new int[posibleDirections];
        for (int i = 0; i < orderToCheck.Length; i++)
        {
            orderToCheck[i] = i;
        }
        timeToExpand = Random.Range(minTimeToSpawn, maxTimeToSpawn);
        StartCoroutine(ExpandOnTimer());
    }

    protected IEnumerator ExpandOnTimer()
    {
        while (true) //no hagan esto en casa niños
        { 
            yield return new WaitForSeconds(timeToExpand);
            timeToExpand = Random.Range(minTimeToSpawn, maxTimeToSpawn);
            Expand();
        }
    }

    protected void Expand()
    {
        //randomizo el array
        for (int i = 0; i < orderToCheck.Length; i++)
        {
            int tmp = orderToCheck[i];
            int r = Random.Range(i, orderToCheck.Length);
            orderToCheck[i] = orderToCheck[r];
            orderToCheck[r] = tmp;
        }

        foreach (int dir in orderToCheck)
        {
            Vector2 posToSpawn = GetNewPositionFromDirection(dir);
            Vector2Int intPosToSpawn = new((int)posToSpawn.x, (int)posToSpawn.y);
            if (fireManager.CheckIfAvailable(intPosToSpawn))
            {
                fireManager.SpawnFire(intPosToSpawn);
                break;
            }
        }
    }

    public virtual Vector2 GetNewPositionFromDirection(int directionsEnumIndex)
    {
        Vector2 posToSpawn = new(transform.position.x, transform.position.z);
        switch ((DirectionsToExpandTo)directionsEnumIndex)
        {
            case DirectionsToExpandTo.UpLeft:
                posToSpawn += Vector2.up + Vector2.left;
                break;
            case DirectionsToExpandTo.Up:
                posToSpawn += Vector2.up;
                break;
            case DirectionsToExpandTo.UpRight:
                posToSpawn += Vector2.up + Vector2.right;
                break;
            case DirectionsToExpandTo.Left:
                posToSpawn += Vector2.left;
                break;
            case DirectionsToExpandTo.Right:
                posToSpawn += Vector2.right;
                break;
            case DirectionsToExpandTo.DownLeft:
                posToSpawn += Vector2.down + Vector2.left;
                break;
            case DirectionsToExpandTo.Down:
                posToSpawn += Vector2.down;
                break;
            case DirectionsToExpandTo.DownRight:
                posToSpawn += Vector2.down + Vector2.right;
                break;
        }
        return posToSpawn;
    }

    public void DespawnFire()
    {
        fireManager.DespawnFire(new Vector2Int((int)transform.position.x, (int)transform.position.z));
    }
}
