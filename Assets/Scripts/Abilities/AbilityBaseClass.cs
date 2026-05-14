using UnityEngine;

/**
* base class for all abilities
*/
public class AbilityBaseClass : MonoBehaviour
{
    //keeps track of the ability's current level
    protected int currentAbilityLevel = 1;

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void SetUp()
    {

    }

    /**
     * upgrades ability based on its current level
     */
    public virtual void UpgradeAbility(int level)
    {
        currentAbilityLevel++;
        Debug.Log(name + " upgraded to " + currentAbilityLevel);
    }

    public int GetCurrentLevel()
    {
        return currentAbilityLevel;
    }
}
