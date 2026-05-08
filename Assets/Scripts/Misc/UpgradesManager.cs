using System.Collections.Generic;
using UnityEngine;

/**
 * manages all upgrades related to the player (stats and abilities)
 */
public class UpgradesManager : MonoBehaviour
{
    public enum StatsUpgrades
    {
        Attack,
        MaxHealth,
        HealthRegen,
        MoveSpeed,
        AttackSpeed
    }

    [Header("UpgradesManager Variables")]
    public StatsUpgrades statsUpgradesType;


    public static UpgradesManager i { get; private set; }

    private void Awake()
    {
        if (i != null)
        {
            Destroy(gameObject);
        }
        else
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
