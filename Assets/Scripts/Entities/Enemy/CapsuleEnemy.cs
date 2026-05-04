using UnityEngine;

public class CapsuleEnemy : EnemyBaseClass
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
