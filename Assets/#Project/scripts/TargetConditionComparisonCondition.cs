using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Target condition comparison", story: "[Target] is to the left of [Agent]", category: "Conditions", id: "e799cbdf352ec89a5098ed4a92dd42f3")]
public partial class TargetConditionComparisonCondition : Condition
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
