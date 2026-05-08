using UnityEngine;

/**
 * the main driver of the game
 */
public class GameManager : MonoBehaviour
{
    public static GameManager i { get; private set; }

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
