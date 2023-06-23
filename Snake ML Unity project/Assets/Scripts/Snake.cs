using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public enum Options
    {
        playerControl,
        agentControl
    }

    [Header("Options")]
    [SerializeField]
    public Options selectedOption;


    [SerializeField] GameObject agent;
    AgentControler agentControler;
    
    private Vector2 previousPositionHead;
    private Vector2 previousPositionLastSegment;

    
    public delegate void MyEventHandler();
    public static event MyEventHandler HitFood;
    public static event MyEventHandler HitWall;


    public List<Transform> segments = new List<Transform>();
    public Transform segmentPrefab;
    public Vector2 direction = Vector2.right;
    public Vector2 input;
    public int initialSize = 4;
    public int timeToReset = 100;
    private int contadorToReset = 0;

    protected void Awake(){
        agentControler = agent.GetComponent<AgentControler>();
    }

    private void Start()
    {
        ResetState();
    }

    /*
    private void Update()
    {
        // Only allow turning up or down while moving in the x-axis
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                input = Vector2.up;
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                input = Vector2.down;
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                input = Vector2.right;
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                input = Vector2.left;
            }
        }
    }
    */

    private void FixedUpdate()
    {
        
        // Set the new direction based on the input
        if (input != Vector2.zero) {
            direction = input;
        }

        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        agentControler.map[(int)Mathf.Round(previousPositionLastSegment.x)+(agentControler.mapLenght-1)/2][(int)Mathf.Round(previousPositionLastSegment.y)+(agentControler.mapHeight-1)/2][2] = false;
        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i - 1].position;
            agentControler.map[(int)Mathf.Round(segments[i].position.x)+(agentControler.mapLenght-1)/2][(int)Mathf.Round(segments[i].position.y)+(agentControler.mapHeight-1)/2][2] = true;
        }
        previousPositionLastSegment = segments[segments.Count-1].position;
        // Move the snake in the direction it is facing
        // Round the values to ensure it aligns to the grid
        float x = Mathf.Round(transform.position.x) + direction.x;
        float y = Mathf.Round(transform.position.y) + direction.y;

        previousPositionHead = transform.position;

        transform.position = new Vector2(x, y);

        agentControler.map[(int)Mathf.Round(previousPositionHead.x)+(agentControler.mapLenght-1)/2][(int)Mathf.Round(previousPositionHead.y)+(agentControler.mapHeight-1)/2][3] = false;
        agentControler.map[(int)Mathf.Round(transform.position.x)+(agentControler.mapLenght-1)/2][(int)Mathf.Round(transform.position.y)+(agentControler.mapHeight-1)/2][3] = true;

        /*
        if(transform.position.x > 21 || transform.position.x < -21 || transform.position.y > 11 || transform.position.y < -11){
            contadorToReset = 0;
            ResetState();
        }
        */

        contadorToReset += 1;
        if (contadorToReset > timeToReset){
            contadorToReset = 0;
            ResetState();
        }
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    public void ResetState()
    {
        if(HitWall != null)
            HitWall();
        
        direction = Vector2.right;
        transform.position = Vector3.zero;

        // Start at 1 to skip destroying the head
        for (int i = 1; i < segments.Count; i++) {
            Destroy(segments[i].gameObject);
        }

        for(int i= 0; i< agentControler.mapLenght; i++){
            for(int x=0; x<agentControler.mapHeight; x++){
                agentControler.map[i][x][2] = false; // Snake Segments
                agentControler.map[i][x][3] = false; // Head of Snake
            }
        }
        agentControler.map[22][12][3]= true;

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);

        // -1 since the head is already in the list
        for (int i = 0; i < initialSize - 1; i++) {
            Grow();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Colisiooooon");
        if (other.gameObject.CompareTag("Food")) {
            contadorToReset = 0;
            if(HitFood != null)
                HitFood();
            Grow();
        } else if (other.gameObject.CompareTag("Obstacle")) {
            contadorToReset = 0;
            ResetState();
        }
    }

}
