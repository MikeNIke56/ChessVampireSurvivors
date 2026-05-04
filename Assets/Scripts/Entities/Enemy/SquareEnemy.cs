using UnityEngine;

public class SquareEnemy : EnemyBaseClass
{
    private void Start()
    {
        currentEnemyStates.Add(EnemyStates.Chasing);
    }

    public override void RunBehavior()
    {
        base.RunBehavior();
    }

    public override void HandleMovement()
    {
        if (currentEnemyStates.Contains(EnemyStates.Chasing))
        {
            ChasePlayer();
        }
    }
}
