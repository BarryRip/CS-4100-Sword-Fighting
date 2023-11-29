using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Diagnostics;
using System.ComponentModel.Design;

public class FighterAgent : Agent
{
    [SerializeField]
    private GameObject opponent;
    [SerializeField]
    private GameObject respawnPoint = null;
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private float movementStrength;
    [SerializeField]
    private float rotationalStrength;

    private Rigidbody2D rb;
    // TODO: add to observations? may introduce new behaviors :)
    private float currentHP;
    private float maxHP = 1f;
    private float currentTime;
    private float startTime;
    private float timeElapsed;

    private float swordDamage = 1f;

    private float scoredPointReward = 30f;
    private float tookDamageReward = -3f;
    private float deathReward = -3f;

    private float timeRewardFactor = 0.05f;
    private float timeOutReward = -8f;

    private float initialRotation;
    private AgentData data;

    private float sceneWidth = 8f;
    private float sceneHeight = 4f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation.eulerAngles.z;
        rb.centerOfMass = Vector2.zero;
        data = DataManager.Instance.RegisterAgent();
    }



    public override void OnEpisodeBegin()
    {
        RandomizeRespawn();
        currentHP = maxHP;
        rb.totalTorque = 0f;
        rb.totalForce = Vector2.zero;
        startTime = Time.time;
    }

    public void ReSpawn()
    {
        transform.position = respawnPoint.transform.position;
        rb.SetRotation(initialRotation);
    }

    public void RandomizeRespawn()
    {
        Vector2 randomPosition = new Vector2(Random.Range(-sceneWidth, sceneWidth), Random.Range(-sceneHeight, sceneHeight));
        transform.position = randomPosition;
        // set random rotation
        float randomRotation = Random.Range(0f, 360f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // self position
        sensor.AddObservation(Normalize(transform.localPosition, -10, 10));
        sensor.AddObservation(((transform.rotation.eulerAngles.z % 360) + 360) % 360);

        // opponent position
        sensor.AddObservation(Normalize(opponent.transform.localPosition, -10, 10));
        sensor.AddObservation(((opponent.transform.rotation.eulerAngles.z % 360) + 360) % 360);

        // time elapsed
        timeElapsed = Time.time - startTime;
        sensor.AddObservation(timeElapsed);
        
    }

    private Vector2 Normalize(Vector2 feature, float min, float max)
    {
        float normX = (feature.x - min) / (max - min);
        float normY = (feature.y - min) / (max - min);
        return new Vector2(normX, normY);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int xAxisMovement = actions.DiscreteActions[0] - 1;
        int yAxisMovement = actions.DiscreteActions[1] - 1;
        int rotationalMovement = actions.DiscreteActions[2] - 1;
        UnityEngine.Debug.Log(transform.name + ' ' + actions.DiscreteActions[0] + ' ' + actions.DiscreteActions[1] + ' ' + actions.DiscreteActions[2]);

        // Debug code to log the action taken by the agent:
        // Debug.Log(xAxisMovement + " / " + yAxisMovement + " / " + rotationalMovement);

        Vector2 force = new Vector2(xAxisMovement * movementStrength, yAxisMovement * movementStrength);
        rb.AddRelativeForce(force);
        rb.AddTorque(rotationalMovement * rotationalStrength);

        // float distance = Vector2.Distance(transform.position, opponent.transform.position);
        // float reward = -timeRewardFactor * (distance);
        // AddReward(reward);

        float distance = Vector2.Distance(transform.position, opponent.transform.position);
        float reward = timeRewardFactor * (1/distance);
        AddReward(reward);


    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        
        discreteActions[0] = 1+Mathf.CeilToInt(Input.GetAxisRaw("Horizontal"));
        discreteActions[1] = 1+Mathf.CeilToInt(Input.GetAxisRaw("Vertical"));
        int discreteRotation = 1;
        if (Input.GetKey(KeyCode.Q))
        {
            discreteRotation = 0;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            discreteRotation = 2;
        }
        discreteActions[2] = discreteRotation;
    }

    public void OnSwordHit()
    {
        AddReward(scoredPointReward);
        data.LogHitDealt(swordDamage);
        opponent.GetComponent<FighterAgent>().OnTakeDamage(swordDamage);
    }

    public void OnTakeDamage(float damageTaken)
    {
        AddReward(tookDamageReward);
        data.LogHitTaken(damageTaken);
        currentHP -= damageTaken;
        if (currentHP <= 0)
        {
            AddReward(deathReward);
            gameController.EndGame(CompletedEpisodes);
        }
    }

    public void TimeOut()
    {   
        AddReward(timeOutReward);

    }

    /// <summary>
    /// Ends the current episode for this Agent only, and logs any
    /// relevant data at the end of the episode.
    /// <para/>
    /// Fundamentally, this is the same as calling EndEpisode() on 
    /// this agent, except we want to update the recorded data before reseting.
    /// </summary>
    public void EndEpisodeAndLogData()
    {
     //   UnityEngine.Debug.Log("agent: " + transform.name);
     //   UnityEngine.Debug.Log("eposide reward: " + GetCumulativeReward());
        data.rewardAtEnd = GetCumulativeReward();
        data.healthAtEnd = currentHP;
        EndEpisode();
    }
}
