using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] GameObject Model1;
    [SerializeField] GameObject Model2;
    [SerializeField] GameObject Model3;
    public void SetModel(int type)
    {
        switch (type)
        {
            case 1:
                Model1.SetActive(true);
                break;
            case 2:
                Model2.SetActive(true);
                break;
            case 3:
            default:
                Model3.SetActive(true);
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Destroy(gameObject);
        }
    }
}
