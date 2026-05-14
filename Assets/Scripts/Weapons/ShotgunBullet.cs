using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ShotgunBullet : ProjectileBaseClass
{
    public override float GetSpeed()
    {
        //random speed per bullet
        return Random.Range(speed - 5, speed + 5);
    }
}
