using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    [SerializeField] private PathManager pathManager;

    [SerializeField] private List<Waypoint> thePath;
    private List<int> toDelete;

    private Waypoint selectedPoint = null;
    private bool doRepaint = true;

    private void OnSceneGUI()
    {
        thePath = pathManager.GetPath();
        DrawPath(thePath);
    }

    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        thePath = pathManager.GetPath();
        
        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();
        
        // Button for adding a point to a path
        if (GUILayout.Button("Add point to path"))
        {
            pathManager.CreateAddPoint();
            selectedPoint = thePath[^1];
        }
        
        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    void DrawGUIForPoints()
    {
        if (thePath != null && thePath.Count > 0)
        {
            for (int i = 0; i < thePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                Waypoint p = thePath[i];

                Color c = GUI.color;
                if (selectedPoint == p) GUI.color = Color.green;

                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);
                
                if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);
                
                // the Delete button
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i);
                }

                GUI.color = c;
                EditorGUILayout.EndHorizontal();
            }
        }

        if (toDelete.Count > 0)
        {
            foreach (int i in toDelete)
            {
                thePath.RemoveAt(i);
            }
            toDelete.Clear();
        }
    }

    public void DrawPath(List<Waypoint> path)
    {
        if (path != null)
        {
            int current = 0;
            foreach (Waypoint wp in path)
            {
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % path.Count;
                Waypoint wpNext = path[next];

                DrawPathLine(wp, wpNext);

                current += 1;
            }
        }
        
        if (doRepaint) Repaint();
    }

    public void DrawPathLine(Waypoint p1, Waypoint p2)
    {
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;
    }

    public bool DrawPoint(Waypoint p)
    {
        bool isChanged = false;

        if (selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;
            
            EditorGUI.BeginChangeCheck();
            Vector3 oldPos = p.GetPos();
            Vector3 newPos = Handles.PositionHandle(oldPos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newPos);
            
            if (Handles.Button(newPos, Quaternion.identity, 0.25f * handleSize, 0.1f * handleSize,
                    Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = null;
            }

            if (EditorGUI.EndChangeCheck())
            {
                p.SetPos(newPos);
            }

            Handles.color = c;
        }
        else
        {
            Vector3 currPos = p.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);

            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize, 0.25f * handleSize,
                    Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }
        }

        return isChanged;
    }
}
