﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    /// <summary>
    /// A reference to the state's parent
    /// </summary>
    private Enemy parent;

    private float attackCooldown = 3;

    private float extraRange = .1f;

    /// <summary>
    /// The state's constructor
    /// </summary>
    /// <param name="parent"></param>
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        Debug.Log("Attacking");

        //Makes sure that we only attack when we are off cooldown
        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            //Resets the attack timer
            parent.MyAttackTime = 0;

            //Starts the attack
            parent.StartCoroutine(Attack());
        }

       
            parent.ChangeState(new IdleState());
        
    }

    /// <summary>
    /// Makes the enemy attack the player
    /// </summary>
    /// <returns></returns>
    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }

}
