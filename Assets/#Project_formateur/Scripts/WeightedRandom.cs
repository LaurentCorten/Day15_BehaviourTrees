using System;
using Unity.Properties;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using System.Linq;

namespace Unity.Behavior
{
    /// <summary>
    /// Executes a random branch.
    /// </summary>
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Weighted Random", story: "Select using [Weights]", category: "Flow", id: "aaa")]
    internal partial class WeightedRandomComposite : Composite
    {
        [SerializeReference] public BlackboardVariable<List<int>> Weights;


        int m_RandomIndex = 0;

        /// <inheritdoc cref="OnStart" />
        protected override Status OnStart()
        {
            int rnd = UnityEngine.Random.Range(0, Weights.Value.Sum());
            int total = 0;
            m_RandomIndex = 0;
            for  (m_RandomIndex = 0; m_RandomIndex < Children.Count; m_RandomIndex++)
            {
                total += Weights.Value[m_RandomIndex];
                if (total > rnd) break;
            }
            if (m_RandomIndex < Children.Count)
            {
                var status = StartNode(Children[m_RandomIndex]);
                if (status == Status.Success || status == Status.Failure)
                    return status;

                return Status.Waiting;
            }

            return Status.Success;
        }

        /// <inheritdoc cref="OnUpdate" />
        protected override Status OnUpdate()
        {
            var status = Children[m_RandomIndex].CurrentStatus;
            if (status == Status.Success || status == Status.Failure)
                return status;

            return Status.Waiting;
        }
    }
}
