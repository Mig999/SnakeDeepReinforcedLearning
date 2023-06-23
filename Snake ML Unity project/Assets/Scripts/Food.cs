using UnityEngine;

public class Food : MonoBehaviour
{
    public Collider2D gridArea;
    [SerializeField] GameObject agent;
    AgentControler agentControler;
    
    private Vector2 previousPosition;

    protected void OnEnable(){
        Snake.HitWall += HitWall;
    }
    
    protected void OnDisable(){
        Snake.HitWall -= HitWall;
    }

    protected void Awake(){
        agentControler = agent.GetComponent<AgentControler>();
    }


    protected void Start()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        previousPosition = transform.position;
        Bounds bounds = gridArea.bounds;

        // Pick a random position inside the bounds
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        // Round the values to ensure it aligns with the grid
        x = Mathf.Round(x);
        y = Mathf.Round(y);

        transform.position = new Vector2(x, y);

        agentControler.map[(int)Mathf.Round(previousPosition.x)+(agentControler.mapLenght-1)/2][(int)Mathf.Round(previousPosition.y)+(agentControler.mapHeight-1)/2][1] = false;
        agentControler.map[(int)Mathf.Round(transform.position.x)+(agentControler.mapLenght-1)/2][(int)Mathf.Round(transform.position.y)+(agentControler.mapHeight-1)/2][1] = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();
    }

    private void HitWall()
    {
        RandomizePosition();
    }

}
