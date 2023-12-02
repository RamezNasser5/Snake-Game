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
        if (collider.tag == "Food")
        {
            GameObject.FindObjectOfType<GameManager>().eatFood(transform.position);
            Destroy(collider.gameObject);
        }
    }
}
