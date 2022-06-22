using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager mInstance;
    public static GameManager GetInstance()
    {
        return mInstance;
    }

    public PlayerController player;

    private void Awake()
    {
        mInstance = this;
    }
}
