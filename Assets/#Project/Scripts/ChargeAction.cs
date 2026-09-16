using System;
using Unity.Behavior;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Charge", story: "[Agent] charges toward [Target]", category: "Action/Boss Fight", id: "5d253cc5643baf9ed428640e52f90a52")]
public partial class ChargeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);
    [SerializeReference] public BlackboardVariable<float> ChargingDistance = new BlackboardVariable<float>(8f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    private Animator m_Animator;
    private float m_CurrentSpeed;
    private Vector3 _startMovePosition;
    private Vector3 _endMovePosition;
    private Collider2D coll;


    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        float distance = Vector3.Distance(_endMovePosition, Agent.Value.transform.position);
        bool destinationReached = distance <= DistanceThreshold; 

        if (destinationReached)
        {
            return Status.Success;
        }
        else // transform-based movement
        {
            m_CurrentSpeed = NavigationUtility2D.SimpleMoveTowardsLocation(Agent.Value.transform, _endMovePosition,
                Speed, distance);
        }

        UpdateAnimatorSpeed();

        return Status.Running;
    }

    public Status OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.ToString());
        return Status.Success;
    }
    protected override void OnEnd()
    {
        UpdateAnimatorSpeed(0f);
        m_Animator = null;
    }

    private Status Initialize()
    {
        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        UpdateAnimatorSpeed(0f);

        _startMovePosition = Agent.Value.transform.position;
        Vector3 moveDir = (_endMovePosition - _startMovePosition).normalized;
        _endMovePosition = new Vector3(_startMovePosition.x + moveDir.x * ChargingDistance, _startMovePosition.y);

        return Status.Running;
    }

    private void UpdateAnimatorSpeed(float explicitSpeed = -1)
    {
        NavigationUtility2D.UpdateAnimatorSpeed(m_Animator, AnimatorSpeedParam, null, m_CurrentSpeed, explicitSpeed: explicitSpeed);
    }
}

