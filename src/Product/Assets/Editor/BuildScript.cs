using UnityEditor;
using UnityEngine;
using System.Collections;
 
public class BuildScript : MonoBehaviour
{
     static void PerformBuild ()
     {
         string[] scenes = { "Assets/Scene.unity" };
         string error = BuildPipeline.BuildPlayer(scenes, "../../build/Product.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
     }
}