using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject snakePiece;
    public GameObject foodPiece;
    int startingCount = 20;
    List<Vector3> positions = new List<Vector3>();
    List<GameObject> snake = new List<GameObject>();
    public GameObject obstacle;

    public bool gameOver = false;
    List<Vector3> extensions = new List<Vector3>();
    int levelWidth = 16;
    int levelHight = 24;
    public int eatedFoodCount = 0;
    public TextMeshProUGUI text;

    void Start()
    {
        for (int i = 0; i < startingCount; i++)
        {
            positions.Add(new Vector3(0, 0, (i - startingCount) * 0.10f));
            GameObject newSnakePiece = Instantiate(snakePiece);
            newSnakePiece.transform.position = positions[i];
            if (i == startingCount - 1)
            {
                newSnakePiece.AddComponent<SnakePiece>();
                Camera.main.transform.parent = newSnakePiece.transform;
                Camera.main.transform.eulerAngles = new Vector3(25, 0, 0);
                Camera.main.transform.localPosition = new Vector3(0, 5, -8);
            }
            else if (1 > startingCount - 20)
            {
                newSnakePiece.tag = "Untagged";
            }

            snake.Add(newSnakePiece);
        }

        int x, z;
        for (int i = 0; i < 20; i++)
        {
            GameObject newObstacle = Instantiate(obstacle);
            bool valid;
            do
            {
                valid = true;
                x = UnityEngine.Random.Range(-8, 8);
                z = UnityEngine.Random.Range(-12, 12);
                if (x > -levelWidth / 4 && x < levelWidth / 4 &&
                    z > -levelHight / 4 && z < levelHight / 4)
                    valid = false;
            } while (valid == false);
            newObstacle.transform.position = new Vector3(x, 0, z);
            newObstacle.transform.localScale = new Vector3(UnityEngine.Random.Range(1, 4),
                1, UnityEngine.Random.Range(1, 4));
        }

        StartCoroutine(MoveSnake());
        StartCoroutine(CreateFood());
    }

    IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(0.02f);
        if (gameOver) yield break;

        bool growSnake = false;
        if (extensions.Count > 0 && extensions[0] == positions[0])
        {
            growSnake = true;
        }

        positions.RemoveAt(0);
        positions.Add(positions[positions.Count - 1] + snake[snake.Count - 1].transform.forward * 0.10f);
        for (int i = 0; i < positions.Count; i++)
        {
            snake[i].transform.position = positions[i];
        }

        if (growSnake)
        {
            positions.Insert(0, extensions[0]);
            GameObject newSnakePiece = Instantiate(snakePiece);
            newSnakePiece.transform.position = positions[0];
            snake.Insert(0, newSnakePiece);
            extensions.RemoveAt(0);
        }

        StartCoroutine(MoveSnake());
    }

    IEnumerator CreateFood()
    {
        yield return new WaitForSeconds(3f);

        int x, z;
        x = UnityEngine.Random.Range(-8, 8);
        z = UnityEngine.Random.Range(-12, 12);
        GameObject newFood = Instantiate(foodPiece);
        newFood.transform.position = new Vector3(x, 0, z);

        if (!gameOver) StartCoroutine(CreateFood());
    }

    public void eatFood(Vector3 position)
{
    extensions.Add(position);

    // Increment the eatedFoodCount
    eatedFoodCount++;

    // Update the text immediately after incrementing the count
    if (text != null)
    {
        text.text = $"{eatedFoodCount}";
        TMPro.TMP_TextInfo textInfo = text.textInfo;
        text.ForceMeshUpdate();
    }
}



    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            snake[snake.Count - 1].transform.Rotate(new Vector3(0, -Time.deltaTime * 260, 0));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            snake[snake.Count - 1].transform.Rotate(new Vector3(0, Time.deltaTime * 260, 0));
        }
        if (Input.GetKeyDown(KeyCode.Backspace)) SceneManager.LoadScene("SampleScene");
    }
    void OnDisable()
    {
        // Reset the eatedFoodCount to 0 when the script is disabled (e.g., when the game stops)
        eatedFoodCount = 0;
    }

}

