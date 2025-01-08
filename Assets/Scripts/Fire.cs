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
    [SerializeField] float minTimeToSpawn;
    [SerializeField] float maxTimeToSpawn;
    [SerializeField] float timeToExpand;
    int[] orderToCheck = new int[(int)DirectionsToExpandTo.Num];
    void Start()
    {
        for (int i = 0; i < orderToCheck.Length; i++)
        {
            orderToCheck[i] = i;
        }
        timeToExpand = Random.Range(minTimeToSpawn, maxTimeToSpawn);
        StartCoroutine(ExpandOnTimer());
    }

    IEnumerator ExpandOnTimer()
    {
        while (true) //no hagan esto en casa niños
        { 
            yield return new WaitForSeconds(timeToExpand);
            timeToExpand = Random.Range(minTimeToSpawn, maxTimeToSpawn);
            Expand();
        }
    }

    void Expand()
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
            Vector2 posToSpawn = new(transform.position.x, transform.position.z);
            switch ((DirectionsToExpandTo)dir)
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
            Vector2Int intPostToSpawn = new((int)posToSpawn.x, (int)posToSpawn.y);
            if (fireManager.CheckIfAvailable(intPostToSpawn))
            {
                fireManager.SpawnFire(intPostToSpawn);
                break;
            }
        }
    }

    public void DespawnFire()
    {
        fireManager.DespawnFire(new Vector2Int((int)transform.position.x, (int)transform.position.z));
    }
}
