using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeForLevel : MonoBehaviour
{

    public TextMeshProUGUI Timer;

    private float TimeAtStart;


    // Start is called before the first frame update
    void Start()
    {
        TimeAtStart=Time.time;
        StartCoroutine(TimeCount());
    }

    IEnumerator TimeCount()
    {
        while (true)
        {
            Timer.text=Time.time.ToString("F2");
            yield return null;
        }
    }
}
