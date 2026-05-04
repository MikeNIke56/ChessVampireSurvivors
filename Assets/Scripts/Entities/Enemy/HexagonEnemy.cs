using UnityEngine;

public class HexagonEnemy : EnemyBaseClass
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
