using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] GameObject player;
    Snake snake;

    // Start is called before the first frame update
    void Awake()
    {
        snake = player.GetComponent<Snake>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(snake.selectedOption == Snake.Options.playerControl){
            // Only allow turning up or down while moving in the x-axis
            if (snake.direction.x != 0f)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                    snake.input = Vector2.up;
                } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    snake.input = Vector2.down;
                }
            }
            // Only allow turning left or right while moving in the y-axis
            else if (snake.direction.y != 0f)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                    snake.input = Vector2.right;
                } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    snake.input = Vector2.left;
                }
            }
        }
        
    }
}
