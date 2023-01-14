using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMovementCollider : StateMachineBehaviour
{
    StateManager states;

    public int index;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
            states = animator.transform.GetComponentInParent<StateManager>();

        states.CloseMovementCollider(index);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
            states = animator.transform.GetComponentInParent<StateManager>();

        states.OpenMovementCollider(index);
    }
}
