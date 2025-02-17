using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookFromHead : MonoBehaviour 
{
    public Transform head;
    public float rotationSmoothing = 0.1f;
    private Quaternion targetRotation;

    void Update()
    {
        if (head != null)
        {
            targetRotation = Quaternion.LookRotation(-head.position);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime / rotationSmoothing
            );
        }
    }
}
