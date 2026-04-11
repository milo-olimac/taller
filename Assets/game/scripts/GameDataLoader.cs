using UnityEngine;
using System.IO;

public class GameDataLoader : MonoBehaviour
{
    public static GameData CargarDatos()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "gamedata.json");

        if (!File.Exists(path))
        {
            Debug.LogError("No se encontró gamedata.json en StreamingAssets");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json);
    }
}