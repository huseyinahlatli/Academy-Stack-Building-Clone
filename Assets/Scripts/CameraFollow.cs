using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float lerpValue = 1f;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            CubeSpawner.Instance.cameraOffset + offset,
            lerpValue * Time.deltaTime
        );
    }
}
