using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using System.IO;

public class GestureAI : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //Inference();
    }
    [MenuItem("Python/Inference")]
    public static void Inference()
    {
        PythonRunner.RunFile($"{Application.dataPath}/Scripts/Ultraleap/Inference.py");
    }
}