using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NutBolts.Scripts.Editor
{
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
                if (SceneManager.GetActiveScene().name != "game" && Directory.Exists("Assets/NutBolts/Scenes"))
                {
                    EditorSceneManager.OpenScene("Assets/NutBolts/Scenes/game.unity");

                }
                CLevelMakerEditor.Init();
                EditorPrefs.SetBool(Application.dataPath + "AlreadyOpened", true);
            }

        }
    }
}