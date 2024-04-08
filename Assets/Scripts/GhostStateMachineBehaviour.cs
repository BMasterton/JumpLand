using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateMachineBehaviour : StateMachineBehaviour
{
    protected Ghost enemy;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(this + ".OnStateEnter()");
        enemy = animator.gameObject.GetComponent<Ghost>();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
