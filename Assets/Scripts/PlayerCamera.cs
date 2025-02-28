using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Transform targetToFollow;
    [SerializeField] PlayerThreeD player;
    Vector3 offset;
    bool following = true;
    [SerializeField] Transform animationStartPosTrans;
    [SerializeField] bool playInitialAnimation;
    [SerializeField] float animationTime;
    [SerializeField] float timeBeforeAnimation;
    [SerializeField] AnimationCurve movementCurve;

    void Start()
    {
        targetToFollow = player.transform;
        offset = transform.position - targetToFollow.position;

        StartCoroutine(InitialAnimation());
    }

    IEnumerator InitialAnimation()
    {
        if (playInitialAnimation)
        {
            player.initialAnimationOver = false;
            Vector3 initialPos = animationStartPosTrans.position + offset;
            transform.position = initialPos;
            yield return new WaitForSeconds(timeBeforeAnimation);
            float timer = 0.0f;
            while (timer < animationTime)
            {
                timer += Time.deltaTime;
                yield return null;
                Vector3 movement = Vector3.Lerp(initialPos, targetToFollow.position + offset, movementCurve.Evaluate((timer / animationTime)));
                transform.position = movement;
            }
        }
        transform.position = targetToFollow.position + offset;
        player.initialAnimationOver = true;
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
