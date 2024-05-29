using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
using UnityEngine;
using UnityEditor;

namespace Platformer
{
    public class Setup
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            Folders.CreateDefault("_Project", "Animation", "Art", "Materials", "Prefabs", "ScriptableObjects", "Sripts", "Settings");
            Refresh();
        }

        static class Folders
        {
            public static void CreateDefault(string root, params string[] folders)
            {
                var fullpath = Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var path = Combine(fullpath, folder);
                    if (!Exists(path))
                    {
                        CreateDirectory(path);
                    }
                }
            }
        }
    
    }
}
