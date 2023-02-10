using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Agent;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    private List<Transform> spawnedObjects;
    private Dictionary<Transform, Transform> parent = new Dictionary<Transform, Transform>();

    [SerializeField] private float springFrequency = 1f;
    [SerializeField] private float explosionForce = 1f;

    public void Setup(List<Transform> objects)
    {
        spawnedObjects = objects;

        foreach (Transform t in spawnedObjects)
            parent[t] = t;
    }

    public Transform Find(Transform t)
    {
        if (parent[t] == t)
            return t;

        return Find(parent[t]);
    }

    public void Union(Transform a, Transform b)
    {
        Transform x = Find(a);
        Transform y = Find(b);

        if (parent[x] != y)
        {
            int newGroupSize = GroupSize(a) + GroupSize(b);

            SeparateAgents(x);
            JoinAgents(x, y, newGroupSize);

            foreach (Transform t in spawnedObjects)
                if (parent[t] == x)
                    parent[t] = y;

            switch (newGroupSize % 3)
            {
                case 1:
                    soundManager.PlayGroup(AgentType.Cyan);
                    break;
                case 2:
                    soundManager.PlayGroup(AgentType.Orange);
                    break;
                case 0:
                    soundManager.PlayGroup(AgentType.Purple);
                    break;
            }
        }
    }

    public void Split(Transform split, int diff)
    {
        Transform p = parent[split];

        List<Transform> splitAgents = new List<Transform>();

        int i = 0;
        foreach (Transform t in spawnedObjects)
        {
            if (t != p && parent[t] == p)
            {
                splitAgents.Add(t);

                i++;
                if (i >= diff) break;
            }
        }

        SeparateAgents(p, splitAgents);
        ExplodeAgents(p, splitAgents);

        foreach (Transform s in splitAgents)
            parent[s] = s;

        for (int k = 1; k < splitAgents.Count; k++)
            Union(splitAgents[k], splitAgents[0]);
    }

    private void JoinAgents(Transform a, Transform b, int newGroupSize)
    {
        foreach (Transform t in spawnedObjects)
        {
            if (parent[t] == a)
            {
                SpringJoint2D spring = b.gameObject.AddComponent<SpringJoint2D>();
                spring.connectedBody = t.GetComponent<Rigidbody2D>();
                spring.enableCollision = true;
                spring.autoConfigureDistance = false;
                spring.frequency = springFrequency;
            }
        }

        foreach (SpringJoint2D spring in b.GetComponents<SpringJoint2D>())
            spring.distance = Mathf.Log(newGroupSize, 2) * 0.2f;
    }

    private void SeparateAgents(Transform t)
    {
        SpringJoint2D[] springs = t.gameObject.GetComponents<SpringJoint2D>();

        foreach (SpringJoint2D spring in springs)
            Destroy(spring);
    }

    private void SeparateAgents(Transform p, List<Transform> splitAgents)
    {
        SpringJoint2D[] springs = p.gameObject.GetComponents<SpringJoint2D>();

        foreach (SpringJoint2D spring in springs)
            if (splitAgents.Contains(spring.connectedBody.transform))
                Destroy(spring);
    }

    private void ExplodeAgents(Transform p, List<Transform> splitAgents)
    {
        foreach (Transform t in splitAgents)
            p.GetComponent<Rigidbody2D>().AddForce((t.position - p.position) * explosionForce, ForceMode2D.Impulse);
    }

    public int GroupSize(Transform check)
    {
        int count = 0;
        Transform p = parent[check];

        foreach (Transform t in spawnedObjects)
            if (parent[t] == p)
                count++;

        return count;
    }
}
