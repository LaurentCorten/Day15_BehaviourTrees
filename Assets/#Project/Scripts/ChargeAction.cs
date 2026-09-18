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
    private Collider2D _coll;
    private LayerMask _layerMask;
    private ContactFilter2D _contactFilter;
    private string[] layers = new string[] { "Floor" };


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

        ColliderArray2D contactColliders = _coll.GetContactColliders(_contactFilter);
        Debug.Log(contactColliders.ToLineSeparatedString());

        UpdateAnimatorSpeed();

        return Status.Running;
    }

    //public void OnCollisionEnter2D(Collision2D col)
    //{
    //    Debug.Log(col.ToString());
    //    Debug.Log("OnCollisionEnter2D");
    //}

    protected override void OnEnd()
    {
        UpdateAnimatorSpeed(0f);
        m_Animator = null;
    }

    private Status Initialize()
    {
        m_Animator = Agent.Value.GetComponentInChildren<Animator>();
        UpdateAnimatorSpeed(0f);
        _coll = Agent.Value.GetComponentInChildren<Collider2D>();
        _layerMask = LayerMask.GetMask(layers);
        _contactFilter = ContactFilter2D.noFilter;
        _contactFilter.useLayerMask = true;
        _contactFilter.layerMask = _layerMask;

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

