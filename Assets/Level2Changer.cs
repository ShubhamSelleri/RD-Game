using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Changer : MonoBehaviour
{
    public Transform playerTrans;

    // Update is called once per frame
    void Update()
    {
        if (playerTrans.position.y < -13)
        {
            SceneManager.LoadScene(1);
        }
    }
}
