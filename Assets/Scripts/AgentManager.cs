using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Agent;

public class AgentManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private AgentType type = AgentType.Cyan;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Instantiate(prefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
    }
}
