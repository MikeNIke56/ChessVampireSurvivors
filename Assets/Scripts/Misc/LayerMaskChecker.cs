using UnityEngine;

/**
* checks if object's layermask matches the one being checked
*/
public class LayerMaskChecker : MonoBehaviour
{
    public static LayerMaskChecker i;

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

    public bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
