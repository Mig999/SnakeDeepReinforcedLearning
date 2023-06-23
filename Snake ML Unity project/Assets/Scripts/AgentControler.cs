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

    private bool[][][] map;
    private int mapLenght = 45;
    private int mapHeight = 25;
    private Vector3 foodPosition;
    private Vector3[] snakePositions;

    protected override void Awake()
    {
        base.Awake();
        snake = player.GetComponent<Snake>();
        CreateMap();

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


    /*
    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    */
    public override void CollectObservations(VectorSensor sensor){
        //UpdateMap();
        for(int i= 0; i< mapLenght; i++){
            for(int x=0; x<mapHeight; x++){
                sensor.AddObservation(map[i][x][0]);
                sensor.AddObservation(map[i][x][1]);
                sensor.AddObservation(map[i][x][2]);
            }
        }
    }

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
    

    private void CreateMap(){
        map = new bool[mapLenght][][];
        for(int i= 0; i< mapLenght; i++){
            map[i] = new bool[mapHeight][];
            for(int x=0; x<mapHeight; x++){
                map[i][x] = new bool[3];
            }
        }

        for(int i= 0; i< mapLenght; i++){
            for(int x=0; x<mapHeight; x++){
                if(i==0 || x==0 || i==mapLenght-1 || x==mapHeight-1){
                    map[i][x][0] = true;
                }
            }
        }

    }

    private void UpdateMap(){
        for(int i= 0; i< mapLenght; i++){
            for(int x=0; x<mapHeight; x++){
                map[i][x][1] = false;
                map[i][x][2] = false;
            }
        }

        foodPosition = targetTransform.position;
        map[(int)Mathf.Round(foodPosition.x)+(mapLenght+1)/2][(int)Mathf.Round(foodPosition.y)+(mapHeight+1)/2][1] = true;

        snakePositions = new Vector3[snake.segments.Count];
        snakePositions[0] = snake.transform.position;
        for(int i= 1; i< snake.segments.Count; i++){
            snakePositions[i] = snake.segments[i].position;
        }
        for(int i=0; i<snakePositions.Length; i++){
            map[(int)Mathf.Round(snakePositions[i].x)+(mapLenght+1)/2][(int)Mathf.Round(snakePositions[i].y)+(mapHeight+1)/2][2] = true;
        }    
    }

}
