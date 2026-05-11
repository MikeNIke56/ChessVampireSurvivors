using System.Security.Cryptography;
using UnityEngine;

/**
* base class for all pickupable items
*/
public class ItemBaseClass : MonoBehaviour
{
    [Header("Item Base Variables")]
    private Vector3 tempPos;
    public float frequency;
    public float amplitude;
    [SerializeField] protected GameObject childObj;

    //the layers of objects this object is allowed to apply physics to
    public LayerMask targetLayers;

    protected void BobUpDown()
    {
        //float up/down with a Sin()
        tempPos = childObj.transform.localPosition;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        childObj.transform.localPosition = tempPos;
    }

    /**
     * checks if object's layermask matches the one being checked
     */
    protected bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
