using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class MovingCube : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    private MoveDirection MoveDirection { get; set; }


    private void OnEnable()
    {
        if (LastCube == null)
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();

        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        Color randomColor = new Color(
            Random.Range(0, 1f),
            Random.Range(0, 1f),
            Random.Range(0, 1f)
        );
        return randomColor;
    }

    internal void Stop()
    {
        moveSpeed = 0f;
        float hangover = GetHangover();

        float max = MoveDirection == MoveDirection.Z
            ? LastCube.transform.localScale.z
            : LastCube.transform.localScale.x;
        
        if (Mathf.Abs(hangover) >= max)
        {
            LastCube = null;
            CurrentCube = null;
            GameManager.Instance.RestartGame();
        }
        
        float direction = hangover > 0 ? 1f : -1f;

        if (MoveDirection == MoveDirection.Z)
            SplitCubeOnZ(hangover, direction);
        else
            SplitCubeOnX(hangover, direction);
            
        LastCube = this;
    }

    private void SetScore()
    {
        var g = GameManager.Instance;
        g.score += 1;
        g.scoreText.text = "Score: " + g.score;

        if (g.score > g.highScore)
        {
            g.highScore = g.score;
            g.highScoreText.text = "High Score: " + g.highScore;
            PlayerPrefs.SetInt("highScore", g.highScore);
        }
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newSizeX = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newSizeX;
        
        float newPositionX = LastCube.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newSizeX, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newPositionX ,transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newSizeX / 2f * direction);
        float fallingBlockPositionX = cubeEdge + (fallingBlockSize / 2f * direction);

        SpawnDropCube(fallingBlockPositionX, fallingBlockSize);
        
        GameManager.Instance.percentText.text = "% " + (newSizeX * 100).ToString(".#");
        
        if(newSizeX != 1)
            SetScore();
    }
    
    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newSizeZ = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newSizeZ;
        
        float newPositionZ = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newSizeZ);
        transform.position = new Vector3(transform.position.x, transform.position.y, newPositionZ);

        float cubeEdge = transform.position.z + (newSizeZ / 2f * direction);
        float fallingBlockPositionZ = cubeEdge + (fallingBlockSize / 2f * direction);

        SpawnDropCube(fallingBlockPositionZ, fallingBlockSize);
        
        GameManager.Instance.percentText.text = "% " + (newSizeZ * 100).ToString(".#");
        
        if(newSizeZ != 1)
            SetScore();
    }

    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
        }

        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z);
        }
        
        cube.gameObject.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube.gameObject, 2f);
    }

    private void FixedUpdate()
    {
        if(MoveDirection == MoveDirection.Z)
            transform.position += transform.forward * (moveSpeed * Time.deltaTime); 
        else
            transform.position += transform.right * (moveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if(transform.position.x > 1.8f)
            GameManager.Instance.RestartGame();
    }

    private float GetHangover()
    {
        if(MoveDirection == MoveDirection.Z)
            return transform.position.z - LastCube.transform.position.z;
        else
            return transform.position.x - LastCube.transform.position.x;
    }
}
