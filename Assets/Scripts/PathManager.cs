using System;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector] public List<Waypoint> path;
    public GameObject prefab;
    [HideInInspector] public int currentPointIndex = 0;

    public List<GameObject> prefabPoints;

    public void Start()
    {
        prefabPoints = new List<GameObject>();

        foreach (Waypoint p in path)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = p.GetPos();
            prefabPoints.Add(go);
        }
    }

    public void Update()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Waypoint p = path[i];
            GameObject g = prefabPoints[i];
            g.transform.position = p.GetPos();
        }
    }
    
    public List<Waypoint> GetPath()
    {
        if (path == null) return new List<Waypoint>();

        return path;
    }

    public void CreateAddPoint()
    {
        Waypoint point = new Waypoint();
        if (path.Count > 0) point.SetPos(path[^1].GetPos());
        path.Add(point);
    }

    public Waypoint GetNextTarget()
    {
        int nextPointIndex = (currentPointIndex + 1) % path.Count;
        currentPointIndex = nextPointIndex;
        return path[nextPointIndex];
    }
}

[Serializable]
public class Waypoint
{
    [SerializeField] private Vector3 pos;

    public void SetPos(Vector3 newPos)
    {
        pos = newPos;
    }

    public Vector3 GetPos()
    {
        return pos;
    }

    public Waypoint()
    {
        pos = new Vector3(0, 0, 0);
    }
}