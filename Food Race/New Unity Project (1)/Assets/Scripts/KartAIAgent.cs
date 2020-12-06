using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class KartAIAgent : Agent
{
    Rigidbody rBody;
    CartController controller;
    LapTracker lt;
    WaypointManager wpm;
    public Transform sp;

    void Start()
    {
        controller = GetComponent<CartController>();
        rBody = controller.theRB;  //gameObject.GetComponentInChildren
        lt = GetComponent<LapTracker>();
        wpm = GetComponent<WaypointManager>();
    }
    
    //reset
    public override void OnEpisodeBegin()
    {
        rBody.position = sp.position;
        controller.gameObject.transform.eulerAngles = Vector3.zero;
        lt.ResetAll();
        wpm.ResetWPs();
    }

    //get extra information which isnt picked up by the raycastsensors
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        //sensor.AddObservation(Target.localPosition);
        //sensor.AddObservation(this.transform.localPosition);
        //sensor.AddObservation(this.transform.position);

        // Agent velocity
        //sensor.AddObservation(rBody.velocity.x);
        //sensor.AddObservation(rBody.velocity.z);

        Vector3 diff = wpm.nextWayPointToReach.transform.position - transform.position;
        sensor.AddObservation(diff / 20f); //Divide by 20 to normalize

        AddReward(-0.001f); //Promote faster driving
    }

    //process actions recieved
    public override void OnActionReceived(ActionBuffers actions)
    {
        var input = actions.ContinuousActions;

        controller.AccelerationInput(input[1]);
        controller.SteerInput(input[0]);

        if (input[2] == 1)
        {
            AddReward(0.0001f);     //0.0005f
            controller.DriftInput(true);
        }
        else
        {
            controller.DriftInput(false);
        }

        if (rBody.position.y <= -5f)
        {            
            AddReward(-0.5f);
            EndEpisode();
        }

        //just for AI training
        /*
        */
        if (lt.finished)
        {
            SetReward(1f);
            EndEpisode();
        }
    }

    //human input
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.ContinuousActions;

        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxisRaw("Vertical");
        action[2] = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;

    }

}