using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentControler : Agent
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] GameObject player;
    Snake snake;

    private float distanceToObject;
    

    protected override void Awake()
    {
        base.Awake();
        snake = player.GetComponent<Snake>();

    }
    
    protected override void OnEnable(){
        base.OnEnable();
        Snake.HitFood += HitFood;
        Snake.HitWall += HitWall;
    }
    protected override void OnDisable(){
        base.OnDisable();
        Snake.HitFood -= HitFood;
        Snake.HitWall -= HitWall;
    }


    
    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

        sensor.AddObservation(DistanceToObject(Vector2.up));
        sensor.AddObservation(DistanceToObject(Vector2.right));
        sensor.AddObservation(DistanceToObject(Vector2.down));
        sensor.AddObservation(DistanceToObject(Vector2.left));        
    }
    /*
    
    public override void CollectObservations(VectorSensor sensor){
        //UpdateMap();
        for(int i= 0; i< mapLenght; i++){
            for(int x=0; x<mapHeight; x++){
                sensor.AddObservation(map[i][x][0]);
                sensor.AddObservation(map[i][x][1]);
                sensor.AddObservation(map[i][x][2]);
                sensor.AddObservation(map[i][x][3]);
            }
        }
    }
    */
    /*
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        if(snake.selectedOption == Snake.Options.agentControl){
            // Only allow turning up or down while moving in the x-axis
            if (snake.direction.x != 0f)
            {
                if (moveX > 0) {
                    snake.input = Vector2.up;
                } else if (moveX < 0) {
                    snake.input = Vector2.down;
                }
            }
            // Only allow turning left or right while moving in the y-axis
            else if (snake.direction.y != 0f)
            {
                if (moveZ > 0) {
                    snake.input = Vector2.right;
                } else if (moveZ < 0) {
                    snake.input = Vector2.left;
                }
            }    
        }
        
    }
    */
    
    public override void OnActionReceived(ActionBuffers actions){
        int moveX = actions.DiscreteActions[0];
        //int moveZ = actions.DiscreteActions[1];

        if(snake.selectedOption == Snake.Options.agentControl){
            // Only allow turning up or down while moving in the x-axis
            if (snake.direction.x != 0f)
            {
                if (moveX == 0) {
                    snake.input = Vector2.up;
                } else if (moveX == 1) {
                    snake.input = Vector2.down;
                }
            }
            // Only allow turning left or right while moving in the y-axis
            else if (snake.direction.y != 0f)
            {
                if (moveX == 2) {
                    snake.input = Vector2.right;
                } else if (moveX == 3) {
                    snake.input = Vector2.left;
                }
            }    
        }
        
    }

    private void HitFood()
    {
        AddReward(1f);
    }
    
    private void HitWall()
    {
        AddReward(-1f);
        EndEpisode();
    }
    
    
    private float DistanceToObject(Vector2 directionRay)
    {
        Debug.Log("Funcionand");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionRay);

        if (hit.collider != null)
        {
            Debug.Log("Funcionando2");
            if (hit.collider.CompareTag("Obstacle"))
            {
                float distanceToObject = hit.distance; // Get the distance to the hit object
                Debug.Log("Distance to obstacle: " + distanceToObject);
            }
        }

        return distanceToObject;

    }
    
    
}
