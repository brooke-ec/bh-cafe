using UnityEngine;

public class God : MonoBehaviour
{
    public static God instance;

    public LevelUIManager levelUIManager;
    public PlayerController playerController;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // Added this so that reloading in play mode works
    private void OnEnable()
    {
        if (instance == null) instance = this;
    }
}
