using System.IO;
using UnityEngine;

/// <summary>
/// Resource Loading
/// </summary>
public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;
    public static ResourceManager Instance => instance;

    private void Awake()
    {
        gameObject.tag = "Manager";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public:
    // Get a hint text
    public string GetStringText(int stringID)
    {
        if (stringID == -1)
        {
            return "HINT_PLACE_HOLDER";
        }
        StringConstData.StringConstDataStruct stringData = StringConstData.GetData(stringID);
        if (!stringData.bLocked || SaveManager.Instance.CheckHintUnlocked(stringID))
        {
            return stringData.Content;
        }
        return stringData.AlterContent;
    }

    // Load Text
    public TextAsset LoadText(string path = Constants.NOTES_SOURCE_PATH, string fileName = "None")
    {
        if (fileName == "None")
        {
            return null;
        }
        string resourcePath = Path.Combine(path, fileName);
        resourcePath = resourcePath.Replace(".txt", "");

        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);

        if (textAsset != null)
        {
            return textAsset;
        }
        else
        {
            Debug.LogWarning("ResourceManager: Text Resource file not found: " + resourcePath);
            return null;
        }
    }

    // Load Image
    public Sprite LoadImage(string path = Constants.IMAGES_SOURCE_PATH, string fileName = "None")
    {
        if (fileName == "None")
        {
            return null;
        }
        string resourcePath = Path.Combine(path, fileName);
        resourcePath = resourcePath.Replace(".png", "").Replace(".jpg", "");

        Sprite imageSprite = Resources.Load<Sprite>(resourcePath);

        if (imageSprite != null)
        {
            return imageSprite;
        }
        else
        {
            Debug.LogWarning("ResourceManager: Image Resource file not found: " + resourcePath);
            return null;
        }
    }
}
