using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Spawn Above", story: "[Agent] spawns [Prefab] above [Target]", category: "Action/Boss Fight", id: "f85920266220f5e5b4bce39c3dd987be")]
public partial class SpawnAboveAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Prefab;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public BlackboardVariable<float> Height;

    protected override Status OnStart()
    {
        
        
        GameObject spawned = GameObject.Instantiate(Prefab.Value);
        spawned.transform.position = Target.Value.transform.position + Height.Value * Vector3.up;
        return Status.Running;

    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

