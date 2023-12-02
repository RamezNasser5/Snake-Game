using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject snakePiece;
    public GameObject foodPiece;
    int startingCount = 4;
    List<Vector3> positions = new List<Vector3>();
    List<GameObject> snake = new List<GameObject>();
    Vector3 direction = new Vector3(1, 0, 0);
    bool isLocked = false;
    public bool gameOver = false;
    List<Vector3> extensions = new List<Vector3>();
    void Start()
    {
        for (int i = 0; i < startingCount; i++)
        {
            positions.Add(new Vector3(i - startingCount, 0, 0));
            GameObject newSnakePiece = Instantiate(snakePiece);
            newSnakePiece.transform.position = positions[i];
            snake.Add(newSnakePiece);
        }

        StartCoroutine(MoveSnake());
        StartCoroutine(CreateFood());
    }

    IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(0.2f);
        if (gameOver) yield break;

        bool growSnake =false;
        if (extensions.Count > 0 && extensions[0] == positions[0])
        {
            growSnake = true;
        }

        positions.RemoveAt(0);
        positions.Add(positions[positions.Count - 1] + direction);
        for (int i = 0; i < positions.Count; i++)
        {
            snake[i].transform.position = positions[i];
        }

        if (growSnake)
        {
            positions.Insert(0,extensions[0]);
            GameObject newSnakePiece = Instantiate(snakePiece);
            newSnakePiece.transform.position = positions[0];
            snake.Insert(0,newSnakePiece);
            extensions.RemoveAt(0);
        }

        isLocked = false;
        StartCoroutine(MoveSnake());
    }

    IEnumerator CreateFood()
    {
        yield return new WaitForSeconds(1f);
        bool validLocation = true;
        int x, z;
        do
        {
            x = UnityEngine.Random.Range(-8, 8);
            z = UnityEngine.Random.Range(-12, 12);

            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].x == x && positions[i].z == z)
                {
                    validLocation = false;
                }
            }
        } while (validLocation == false);
        GameObject newFood = Instantiate(foodPiece);
        newFood.transform.position = new Vector3(x, 0, z);
        


    }

    public void eatFood(Vector3 position) {
        extensions.Add(position);
        StartCoroutine(CreateFood());
    }

    void Update()
    {
        if (isLocked == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && direction.z == 0)
            {
                direction = new Vector3(0, 0, 1);
                isLocked = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && direction.z == 0)
            {
                direction = new Vector3(0, 0, -1);
                isLocked = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction.x == 0)
            {
                direction = new Vector3(-1, 0, 0);
                isLocked = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && direction.x == 0)
            {
                direction = new Vector3(1, 0, 0);
                isLocked = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace)) SceneManager.LoadScene("SampleScene");
    }


}
