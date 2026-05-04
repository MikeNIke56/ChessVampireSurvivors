using UnityEngine;

public class DiamondEnemy : EnemyBaseClass
{
    public override void RunBehavior()
    {
        
    }

    public override void HandleMovement()
    {
        base.HandleMovement();
        ChasePlayer();
    }
}
