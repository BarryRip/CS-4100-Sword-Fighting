using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FighterAgent : Agent
{
    [SerializeField]
    private GameObject opponent;
    [SerializeField]
    private GameObject respawnPoint;
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private float movementStrength;
    [SerializeField]
    private float rotationalStrength;

    private Rigidbody2D rb;
    // TODO: add to observations? may introduce new behaviors :)
    private float currentHP;
    private float maxHP = 3f;

    private float swordDamage = 1f;

    private float scoredPointReward = 1f;
    private float tookDamageReward = -1f;
    private float deathReward = -5f;
    // TODO: add timer penalty to increase speed of battles???
    private float timeReward = -0.01f;

    private float initialRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation.eulerAngles.z;
        rb.centerOfMass = Vector2.zero;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = respawnPoint.transform.position;
        currentHP = maxHP;
        rb.totalTorque = 0f;
        rb.totalForce = Vector2.zero;
        rb.SetRotation(initialRotation);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Normalize(transform.localPosition, -10, 10));
        sensor.AddObservation(((transform.rotation.eulerAngles.z % 360) + 360) % 360);
        sensor.AddObservation(Normalize(opponent.transform.localPosition, -10, 10));
        sensor.AddObservation(((opponent.transform.rotation.eulerAngles.z % 360) + 360) % 360);
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

        Debug.Log(xAxisMovement + " / " + yAxisMovement + " / " + rotationalMovement);

        Vector2 force = new Vector2(xAxisMovement * movementStrength, yAxisMovement * movementStrength);
        rb.AddForce(force);
        rb.AddTorque(rotationalMovement * rotationalStrength);
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
        opponent.GetComponent<FighterAgent>().OnTakeDamage(swordDamage);
    }

    public void OnTakeDamage(float damageTaken)
    {
        AddReward(tookDamageReward);
        currentHP -= damageTaken;
        if (currentHP <= 0)
        {
            AddReward(deathReward);
            gameController.EndGame();
        }
    }
}
