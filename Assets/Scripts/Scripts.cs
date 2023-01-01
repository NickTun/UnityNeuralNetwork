using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Scripts : Agent
{
    string pointName;
    public Transform aim;
    public GameObject drone, r1, r2, r3, r4, Mesh;
    Rigidbody rb;
    EnvironmentParameters defaultParameters;
    public float speed = 0.2f;
    Vector3 movement;

    public int RayCount, RaySpread;

    public override void Initialize()
    {
        rb = drone.GetComponent<Rigidbody>();
        defaultParameters = Academy.Instance.EnvironmentParameters;
        base.Initialize();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(transform.position);
        sensor.AddObservation(aim.position);
        for(int i = 0; i < RayCount; i++)
        {
            Vector3 newVector = Quaternion.AngleAxis(i * (RaySpread / 8) - (RaySpread / 2), new Vector3(0, 1, 0)) * transform.forward;
            RaycastHit hit;
            Ray ray = new Ray(transform.position, newVector);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 9))
            {
                Debug.DrawRay(transform.position, newVector * hit.distance, Color.yellow);
            }
            else
            {
                Debug.DrawRay(transform.position, newVector * 1000, Color.white);
            }
            sensor.AddObservation(hit.distance);
        }

        for(int i = 0; i < RayCount; i++)
        {
            Vector3 newVector = Quaternion.AngleAxis(i * (RaySpread / 8) - (RaySpread / 2), new Vector3(1, 0, 0)) * transform.forward;
            RaycastHit hit;
            Ray ray = new Ray(transform.position, newVector);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 9))
            {
                Debug.DrawRay(transform.position, newVector * hit.distance, Color.yellow);
            }
            else
            {
                Debug.DrawRay(transform.position, newVector * 1000, Color.white);
            }
            sensor.AddObservation(hit.distance);
        }
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        r1.transform.Rotate(0, 0, 40);
        r2.transform.Rotate(0, 0, 40);
        r3.transform.Rotate(0, 0, 40);
        r4.transform.Rotate(0, 0, 40);

        movement.x = actionBuffers.ContinuousActions[0] * speed;
        movement.z = actionBuffers.ContinuousActions[1] * speed;
        movement.y = actionBuffers.ContinuousActions[2] * speed * 0.3f;
        transform.Translate(movement);

        transform.Rotate(0, actionBuffers.ContinuousActions[3] * 1.5f, 0);
        //drone.transform.eulerAngles = new Vector3(actionBuffers.ContinuousActions[1] * 20, 0, actionBuffers.ContinuousActions[0] * -20);
        Mesh.transform.eulerAngles = new Vector3(Mathf.Lerp(transform.rotation.x, actionBuffers.ContinuousActions[1] * 100, 0.05f), transform.eulerAngles.y, Mathf.Lerp(transform.rotation.x, actionBuffers.ContinuousActions[0] * -100, 0.05f));

        float distanceToTarget = Vector3.Distance(aim.position, transform.position);
        //Debug.Log(actionBuffers.ContinuousActions[0]);

        // Fell off platform
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndEpisode();
        }
    }
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        transform.position = new Vector3(0, 0, 0);
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[2] = Input.GetAxis("Fly");
        continuousActionsOut[3] = Input.GetAxis("Rotate") * -1;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Finish")
        {
            //Debug.Log("Victory");
            AddReward(0.15f);
            EndEpisode();
        }

        if(other.tag == "CheckPoint")
        {
            //Debug.Log("Victory");
            if(other.gameObject.name != pointName)
            {
                Debug.Log("CheckPoint");
                AddReward(0.15f);
                pointName = other.gameObject.name;
            }
        }
        
        if(other.tag == "Respawn")
        {
            AddReward(-0.05f);
            EndEpisode();
        }
    }
}