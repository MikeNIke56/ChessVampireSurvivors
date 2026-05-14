using UnityEngine;

public class SquareEnemy : EnemyBaseClass
{
    protected override void OnEnable()
    {
        Setup(player);
        currentEnemyStates.Add(EnemyStates.Chasing);
    }

    public override void Setup(PlayerController player)
    {
        base.Setup(player);
        currentEnemyStates.Add(EnemyStates.Chasing);
    }

    public override void RunBehavior()
    {
        base.RunBehavior();
    }

    public override void HandleMovement()
    {
        if (!currentEnemyStates.Contains(EnemyStates.KnockedBack))
        {
            if (currentEnemyStates.Contains(EnemyStates.Chasing))
                ChasePlayer();
        }   
    }
}
