using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button startButton;
    // Scene to start

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGameFunc);
    }

    void StartGameFunc()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
