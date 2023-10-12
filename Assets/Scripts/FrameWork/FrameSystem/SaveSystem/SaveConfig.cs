using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveSystem/SaveConfig")]
public class SaveConfig : ScriptableSingleton<SaveConfig>
{
    // Control flag
    [SerializeField] private bool bAllowRewrite = true;
    public bool AllowRewrite { get { return bAllowRewrite; } }

    // Data
    [SerializeField]
    private PlayerSaveData playerSaveData = new PlayerSaveData();

    [SerializeField]
    private List<NPCSaveData> npcSaveDataList = new List<NPCSaveData>();

    // Datagrams
    [System.Serializable]
    public class PlayerSaveData
    {
        public string PlayerName;
        public Vector3 Position;
        public string Scene;
    }

    [System.Serializable]
    public class NPCSaveData
    {
        public string NpcName;
        public Vector3 Position;
        public List<int> interactionIDs;
        public string Scene;
    }

    // Methods:
    public void SetPlayer(string name, Vector3 position, string scene)
    {
        playerSaveData.PlayerName = name;
        playerSaveData.Position = position;
        playerSaveData.Scene = scene;
    }

    public PlayerSaveData GetPlayer()
    {
        return playerSaveData;
    }

    public void LockSave()
    {
        bAllowRewrite = false;
    }
}

