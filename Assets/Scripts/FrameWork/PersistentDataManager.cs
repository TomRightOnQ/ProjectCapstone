using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Core Object lives through the entire game run-time
/// Manager the game's persisitent data
/// </summary>

public class PersistentDataManager : MonoBehaviour
{
    private static PersistentDataManager instance;
    public static PersistentDataManager Instance => instance;
    private long MObjectID = -1;

    // Reference
    // Player
    [SerializeField] private Player mainPlayer;
    public Player MainPlayer => mainPlayer;

    // MainCamera
    [SerializeField] private PlayerCamera mainCamera;
    public PlayerCamera MainCamera => mainCamera;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Return a unique id
    public long GetMObjectID()
    {
        return ++MObjectID;
    }

    // Set Player
    public void SetPlayer(Player player)
    {
        mainPlayer = player;
        InputManager.Instance.SetController(player.Controller);
    }

    // Set Camera
    public void SetCamera()
    {
        mainCamera = FindObjectOfType<PlayerCamera>();
        if (mainCamera != null)
        {
            mainCamera.SetCameraToTarget(mainPlayer.gameObject, mainPlayer);
        }
    }
}
