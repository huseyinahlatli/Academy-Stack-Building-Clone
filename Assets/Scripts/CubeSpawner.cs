using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private MoveDirection moveDirection;
    
    [HideInInspector] public Vector3 cameraOffset = Vector3.zero;
    private Vector3 _offsetAmount = new Vector3(0, 0.05f, 0);
    private MoveDirection MoveDirection { get; set; }

    public static CubeSpawner Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);
        
        if (MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("Start"))
        {
            cube.transform.position = new Vector3
            (
                transform.position.x, 
                MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y,
                transform.position.z
            ); 
        }
        else
        {
            cube.transform.position = transform.position;
        }

        // cube.MoveDirection = moveDirection;
        
        SetCamHeight();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }

    private void SetCamHeight()
    {
        cameraOffset += _offsetAmount;
    }
}