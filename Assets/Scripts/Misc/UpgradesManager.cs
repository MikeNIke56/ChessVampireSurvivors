using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public enum ItemTypes
    {
        HealthPickup,
        Bomb,
        Magnet
    }

    [Header("UpgradesManager Variables")]
    //keeps track of the player's current stats buffs
    public Dictionary<StatsUpgrades, int> statsBuffRecord;

    public float[] statIncreases = new float[5];
    private const int maxLevel = 5;

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

    private void Start()
    {
        statsBuffRecord = new Dictionary<StatsUpgrades, int>();
        InitializeStartingStats();
    }

    /**
     * initializes all player stats to lvl1
     */
    private void InitializeStartingStats()
    {
        statsBuffRecord.Add(StatsUpgrades.Attack, 1);
        statsBuffRecord.Add(StatsUpgrades.MaxHealth, 1);
        statsBuffRecord.Add(StatsUpgrades.HealthRegen, 1);
        statsBuffRecord.Add(StatsUpgrades.MoveSpeed, 1);
        statsBuffRecord.Add(StatsUpgrades.AttackSpeed, 1);
    }

    /**
     * applies the appropriate buff to the player based on what upgrade they
     * chose
     */
    public void ApplyStatBuff(StatsUpgrades statType, bool carryOverLvls=false,
        int lvlToApply=1)
    {
        if (statsBuffRecord.TryGetValue(statType, out int value))
        {
            if (carryOverLvls == true)
            {
                switch (statType)
                {
                    //sets the previous weapon's buff lvl to the new one
                    case StatsUpgrades.Attack:
                        WeaponBaseClass weaponAtkCopy = PlayerController.i.
                            GetPrimaryWeapon();

                        for(int i = 0; i < lvlToApply; i++)
                        {
                            weaponAtkCopy.SetWeaponAttack(weaponAtkCopy.
                                GetWeaponAttack() * statIncreases[i]);
                        }
                        //Debug.Log("atk: " + value);
                        break;

                    //sets the previous weapon's buff lvl to the new one
                    case StatsUpgrades.AttackSpeed:
                        WeaponBaseClass weaponCDCopy = PlayerController.i.
                            GetPrimaryWeapon();

                        for (int i = 0; i < lvlToApply; i++)
                        {
                            weaponCDCopy.SetWeaponCooldown(weaponCDCopy.
                                GetWeaponFireCooldown() / statIncreases[i]);
                        }
                        //Debug.Log("atk spd: " + value);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (value < maxLevel)
                {
                    switch (statType)
                    {
                        //applies the buffs to the player's current weapon
                        case StatsUpgrades.Attack:
                            float curWeaponAttack = PlayerController.i.
                                GetPrimaryWeapon().GetWeaponAttack();

                            WeaponBaseClass weaponAtkCopy = PlayerController.i.
                                GetPrimaryWeapon();

                            weaponAtkCopy.SetWeaponAttack(curWeaponAttack *
                                statIncreases[value]);
                            Debug.Log("atk: " + value);
                            break;

                        //applies the buffs to the player's current weapon
                        case StatsUpgrades.AttackSpeed:
                            float curWeaponCooldown = PlayerController.i.
                                GetPrimaryWeapon().fireCooldown;

                            WeaponBaseClass weaponCDCopy = PlayerController.i.
                                GetPrimaryWeapon();

                            weaponCDCopy.SetWeaponCooldown(weaponCDCopy.GetWeaponFireCooldown() /
                                statIncreases[value]);
                            //Debug.Log("atk spd: " + value);
                            break;

                        case StatsUpgrades.MoveSpeed:
                            float curPlayerSpeed = PlayerController.i.GetMoveSpeed();
                            PlayerController.i.SetMoveSpeed(curPlayerSpeed *
                                statIncreases[value]);
                            //Debug.Log("move: " + value);
                            break;

                        case StatsUpgrades.HealthRegen:
                            float curPlayerHealthRegen =
                                PlayerController.i.GetHealthRegenSpeed();

                            float curPlayerMaxHealthRegenTime =
                                PlayerController.i.GetMaxHealthRegenTime();

                            PlayerController.i.SetHealthRegenSpeed(
                                curPlayerHealthRegen * statIncreases[value]);
                            PlayerController.i.SetMaxHealthRegenTime(
                                curPlayerMaxHealthRegenTime / statIncreases[value]);
                            //Debug.Log("regen spd: " + value);
                            break;

                        case StatsUpgrades.MaxHealth:
                            float curPlayerMaxHealth = PlayerController.i.GetMaxHealth();
                            PlayerController.i.SetMaxHealth(curPlayerMaxHealth *
                                statIncreases[value]);
                            //Debug.Log("max hlth: " + value);
                            break;

                        default:
                            Debug.Log("no existing stat");
                            break;
                    }

                    statsBuffRecord[statType]++;
                }
                else
                    Debug.Log(statType.ToString() + " is maxxed out");
            }
            
        }
    }
}
