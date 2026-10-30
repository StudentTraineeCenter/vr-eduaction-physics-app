using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Field_Lines : MonoBehaviour
{
    [Header("Charges")]
    public Transform[] charge_point;
    public float[] charge_value;

    [Header("Line Settings")]
    public Material line_material;
    public Color start_line_color = Color.red;
    public Color end_line_color = Color.blue;
    public float line_width = 0.015f;

    [Header("Forcelines Settings")]
    public int lines_per_charge = 8;
    public float start_radius = 0.15f;
    public float step_lenght = 0.05f;
    public int max_steps = 150;
    public float stop_distance = 0.1f;
    public int layers = 4;

    readonly List<LineRenderer> lines = new List<LineRenderer>();
    readonly List<int> sourceIdx = new List<int>();
    readonly List<Vector3> offsets = new List<Vector3>();
    LineRenderer lr;
    Vector3[] buffer;
    Vector3[] last_pos;
    bool first_frame = true;

    void Awake()
    {
        last_pos = new Vector3[charge_point.Length];  
    }

    void Update()
    {
        bool changed = first_frame;
        first_frame = false;

        for (int i = 0; i < charge_point.Length; i++)
        {
            Vector3 p = charge_point[i].position;
            if (p != last_pos[i])
            {
                last_pos[i] = p;
                changed = true;
            }    
        }
        if (changed) CreateLine();
    }

    private void CreateLine () 
    {
        if (charge_point == null || charge_value == null) return;
        ClearLines();
        buffer = new Vector3[max_steps];
        for (int c = 0; c < charge_point.Length; c++)
        {
            if (charge_value[c] < 0f) continue;
            for (int l = -layers; l < layers; l++)
            {
                int count = Mathf.RoundToInt(lines_per_charge * charge_value[c]);
                for (int k = 0; k < count; k++)
                {
                    float a = 2f*Mathf.PI*k / count;
                    offsets.Add(start_radius * new Vector3((float)l / layers, Mathf.Cos(a), Mathf.Sin(a)));
                    sourceIdx.Add(c);
                    lines.Add(NewLine());
                }
            }
        }
        Rebuild();
    }

    LineRenderer NewLine()
    {
        GameObject game_object = new GameObject("FieldLine");
        game_object.transform.SetParent(transform, false);
        LineRenderer lr = game_object.AddComponent<LineRenderer>();
        lr.material = line_material;
        lr.startColor = start_line_color;
        lr.endColor = end_line_color;
        lr.startWidth = line_width;
        lr.endWidth = line_width;

        lr.useWorldSpace = true;
        lr.shadowCastingMode = ShadowCastingMode.Off;
        lr.receiveShadows = false;

        return lr;
    }
    public void Rebuild()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Vector3 start = charge_point[sourceIdx[i]].position + offsets[i];
            int n = Field.Trace(start, charge_point, charge_value, buffer, step_lenght, stop_distance);
            lines[i].positionCount = n;
            lines[i].SetPositions(buffer);
        }
    }

    private void ClearLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i] != null)
                Destroy(lines[i].gameObject);
        }
    
        lines.Clear();
        sourceIdx.Clear();
        offsets.Clear();
    }

}