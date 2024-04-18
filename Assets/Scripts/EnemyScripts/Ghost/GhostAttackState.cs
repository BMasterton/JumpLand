using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GhostAttackState : GhostStateMachineBehaviour
{
    float attackTimer;
    float attackTimeThreshold = 3.0f;
    float speed = 2.0f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        attackTimer = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTimer += Time.deltaTime;
        if(attackTimer > attackTimeThreshold)
        {
            enemy.ShootEvent();
            attackTimer = 0;
        }

        float step = speed * Time.deltaTime;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, enemy.Player.transform.position, step);
        if (enemy.GetDistanceFromPlayer() > enemy.AttackRangeStop)
        {
            animator.SetTrigger("chase");

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
