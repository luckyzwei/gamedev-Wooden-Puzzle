using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

[InitializeOnLoad]
public class Autorun
{
    static Autorun()
    {
        EditorApplication.update += InitProject;

    }

    static void InitProject()
    {
        EditorApplication.update -= InitProject;
        if (EditorApplication.timeSinceStartup < 10 || !EditorPrefs.GetBool(Application.dataPath + "AlreadyOpened"))
        {
            if (EditorSceneManager.GetActiveScene().name != "game" && Directory.Exists("Assets/NutBolts/Scenes"))
            {
                EditorSceneManager.OpenScene("Assets/NutBolts/Scenes/game.unity");

            }
            CLevelMakerEditor.Init();
            //LevelMakerEditor.ShowHelp();
            EditorPrefs.SetBool(Application.dataPath + "AlreadyOpened", true);
        }

    }
}