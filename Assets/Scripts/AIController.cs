using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AIController : Agent
{
    private Rigidbody _rb;

    private const float _speed = 10f;
    private const float _rotSpeed = 10f;

    public int TeamID;

    public AIManager Manager { set; get; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(new Vector2(transform.localPosition.x, transform.localPosition.z));
        sensor.AddObservation(new Vector2(Manager.Ball.transform.localPosition.x, Manager.Ball.transform.localPosition.z));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var forward = actions.DiscreteActions[0];
        var right = actions.DiscreteActions[1];
        var rot = actions.DiscreteActions[2];

        var forwardDir = 0f;
        var rightDir = 0f;
        var rotDir = 0f;

        if (forward == 1) forwardDir = _speed;
        else if (forward == 2) forwardDir = -_speed;
        if (right == 1) rightDir = _speed;
        else if (right == 2) rightDir = -_speed;
        if (rot == 1) rotDir = _rotSpeed;
        else if (rot == 2) rotDir = -_rotSpeed;

        transform.Rotate(transform.up, rotDir * Time.deltaTime);
        _rb.AddForce(transform.forward * forwardDir + transform.right * rightDir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Manager.TouchBall(TeamID);
        }
    }
}
