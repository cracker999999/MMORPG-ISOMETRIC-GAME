using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    /// <summary>
    /// A canvasgroup for the healthbar
    /// </summary>
    [SerializeField]
    private CanvasGroup healthGroup;

    private IState currentState;


    /// <summary>
    /// How much time has passed since the last attack
    /// </summary>
    public float MyAttackTime { get; set; }

    /// <summary>
    /// The enemys attack range
    /// </summary>
    public float MyAttackRange { get; set; }


    protected void Awake()
    {
        MyAttackRange = 1;
        ChangeState(new IdleState());
    }


    protected override void Update()
    {
        if (IsAlive) {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();
           
        }
        base.Update();
    }


    /// <summary>
    /// When the enemy is selected
    /// </summary>
    /// <returns></returns>
    public override Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;

        return base.Select();
    }

    /// <summary>
    /// When we deselect our enemy
    /// </summary>
    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    /// <summary>
    /// Makes the enemy take damage when hit
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage, Transform source)
    {
        base.TakeDamage(damage, source);

        OnHealthChanged(health.MyCurrentValue);
    }


    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

}
