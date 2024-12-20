using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeForLevel : MonoBehaviour
{
    public TextMeshProUGUI Timer;
    public Transform Player;
    public int endX = 253;

    private float TimeAtStart;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2))
        {
            TimeAtStart = Time.time;
            StartCoroutine(TimeCount());
        }
    }

    IEnumerator TimeCount()
    {
        while (Player.position.x < endX)
        {
            Timer.text = (Time.time - TimeAtStart).ToString("F2");
            yield return null;
        }
    }
}

