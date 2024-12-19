using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeFirstlevelToSecond : MonoBehaviour
{
    public Transform playerTrans;
    public int x_value;

    // Update is called once per frame
    void Update()
    {
        if (playerTrans.position.x > x_value)
        {
            SceneManager.LoadScene(0);
        }
    }
}
