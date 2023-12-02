using UnityEngine;

public class SnakePiece : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Wall" || collider.tag == "Player" )
        {
            GameObject.FindObjectOfType<GameManager>().gameOver = true;
        }
    }
}
