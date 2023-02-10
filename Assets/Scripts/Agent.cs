using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public enum AgentType { Cyan, Orange, Purple }

    [HideInInspector] public SimulationManager simManager;

    [SerializeField] private float speed = 10f;

    [Space]
    [SerializeField] private SpriteRenderer sRender;
    [SerializeField] private Color rockColor;
    [SerializeField] private Color paperColor;
    [SerializeField] private Color scissorsColor;

    private void Start()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
    }

    private void Update()
    {
        int groupSize = simManager.GroupSize(transform);

        switch (groupSize % 3)
        {
            case 1:
                sRender.color = rockColor;
                break;
            case 2:
                sRender.color = paperColor;
                break;
            default:
                sRender.color = scissorsColor;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            int selfGroup = simManager.GroupSize(transform);
            int otherGroup = simManager.GroupSize(collision.transform);

            if (selfGroup % 3 == otherGroup % 3)
                simManager.Union(transform, collision.transform);
            else if (selfGroup % 3 == (otherGroup % 3 + 1) % 3)
                simManager.Split(collision.transform, Mathf.Abs(selfGroup - otherGroup));
        }
    }
}
