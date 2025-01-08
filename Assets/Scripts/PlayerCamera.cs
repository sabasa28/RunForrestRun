using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    Transform targetToFollow;
    Vector3 offset;
    bool following = true;
    void Start()
    {
        offset = transform.position - targetToFollow.position;
        StartCoroutine(FollowTarget());
    }

    // Update is called once per frame
    IEnumerator FollowTarget()
    {
        while (following)
        {
            yield return new WaitForFixedUpdate();
            Vector3 movement = Vector3.Lerp(transform.position, targetToFollow.position + offset, 0.1f);
            transform.position = movement;
        }
    }
}
