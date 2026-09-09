using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TargetPositionComparaison", story: "[Target] is to the left of [Agent]", category: "Conditions", id: "a7376c5ec70f5628ce68f40322060c4e")]
public partial class TargetPositionComparaisonCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<Transform> Agent;

    public override bool IsTrue()
    {
        return Target.Value.position.x < Agent.Value.position.x;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
