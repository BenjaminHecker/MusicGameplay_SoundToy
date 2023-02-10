using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SimulationManager simManager;

    [SerializeField] private float width, height;
    [SerializeField] private float padding = 0.5f;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int count;
    [SerializeField] private TextMeshProUGUI countTxt;

    private List<Transform> spawnedObjects;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        spawnedObjects = new List<Transform>();

        for (int i = 0; i < count; i++)
        {
            int attemptsAllowed = 10;
            int attempts = 0;

            while (attempts < attemptsAllowed)
            {
                Vector2 pos = new Vector2(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2));

                if (intersectsObject(pos))
                {
                    attempts++;
                    continue;
                }

                Transform newObj = Instantiate(prefab, pos, Quaternion.identity).transform;
                newObj.GetComponent<Agent>().simManager = simManager;
                spawnedObjects.Add(newObj);

                break;
            }
        }

        simManager.Setup(spawnedObjects);
    }

    private bool intersectsObject(Vector3 pos)
    {
        foreach (Transform t in spawnedObjects)
            if ((t.position - pos).magnitude <= padding)
                return true;

        return false;
    }

    public void ResetSim()
    {
        foreach (Transform t in spawnedObjects)
            Destroy(t.gameObject);

        Setup();
    }

    public void UpdateCount(System.Single value)
    {
        int val = (int)value;

        count = val;
        countTxt.text = val.ToString();
    }
}
