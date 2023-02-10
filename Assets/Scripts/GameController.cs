using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }
}
