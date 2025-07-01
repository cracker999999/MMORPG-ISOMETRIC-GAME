using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class IdleState : IState
{
    /// <summary>
    /// A reference to the parent
    /// </summary>
    private Enemy parent;

    /// <summary>
    /// This is called whenever we enter the state
    /// </summary>
    /// <param name="parent">The parent enemy</param>
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    /// <summary>
    /// This is called whenever we exit the state
    /// </summary>
    public void Exit()
    {
        
    }

    /// <summary>
    /// This is called as long as we are inside the state
    /// </summary>
    public void Update()
    {
       
    }
}
