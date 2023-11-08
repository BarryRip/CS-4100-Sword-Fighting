using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

// Template for agent script for demo.
public class MoveToGoalAgent : Agent
{
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private Agent opponent;
    private float moveSpeed = 3f;

    // Everytime an episode starts or ends, this runs.
    // - basically reset positions and stuff
    public override void OnEpisodeBegin()
    {
        transform.localPosition = GenerateRandomPosition();

        Vector2 pos = GenerateRandomPosition();
        while (Mathf.Abs(Vector2.Distance(pos, transform.localPosition)) < 2)
        {
            pos = GenerateRandomPosition();
        }

        targetTransform.localPosition = pos;
    }

    private Vector2 GenerateRandomPosition()
    {
        float randX = Random.Range(0f, 5f) * (Random.Range(0, 2) == 1 ? 1 : -1);
        float randY = Random.Range(0f, 3f) * (Random.Range(0, 2) == 1 ? 1 : -1);
        return new Vector2(randX, randY);
    }

    // Define the observations that will be made by the agent
    // - add these observations to sensor with .AddObservation()
    // - set up what specific observations in inspector
    public override void CollectObservations(VectorSensor sensor)
    {

        // observe agent position
        sensor.AddObservation(Normalize(transform.localPosition, -10f, 10f));
        // observe target position
        sensor.AddObservation(Normalize(targetTransform.localPosition, -10f, 10f));
    }

    private Vector2 Normalize(Vector2 feature, float min, float max)
    {
        float normX = (feature.x - min) / (max - min);
        float normY = (feature.y - min) / (max - min);
        return new Vector2(normX, normY);
    }

    // On a given action, do something.
    // - get the continuous / discrete actions from actions
    // - set how many actions of each in inspector
    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontalMovement = actions.ContinuousActions[0];
        float verticalMovement = actions.ContinuousActions[1];
        float rotationalMovement = actions.ContinuousActions[2];

        Vector3 movement = new Vector3(horizontalMovement, verticalMovement) * Time.deltaTime * moveSpeed;

        transform.localPosition += movement;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    // This just checks if we have entered the goal trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            // This or AddReward() to grant a reward upon something happening
            SetReward(1f);
            // This "stops" a single episode of the game
            EndEpisode();
            opponent.EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            SetReward(-1f);
            EndEpisode();
            opponent.EndEpisode();
        }
    }
}
