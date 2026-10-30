using UnityEngine;

public class Field : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static Vector3 OfOne(Vector3 p, Vector3 charge_pos, float q) 
    {
        Vector3 r = p - charge_pos;
        float d = r.magnitude;
        if (d < 1e-4f) return Vector3.zero;
        return q*r/(d*d*d);

    }   

    public static Vector3 At(Vector3 p, Transform[] pos, float[] q)
    {
        Vector3 E = Vector3.zero;
        for (int i = 0; i < pos.Length; i++)
        {
            if (pos[i] == null) continue;
            E += OfOne(p, pos[i].position, q[i]);
        }
        return E;
    }

    public static bool NearNegative(Vector3 p, Transform[] pos, float[] q, float stop_distance)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if(pos[i] != null && q[i] < 0f && Vector3.Distance(p, pos[i].position) < stop_distance) return true;
        }
        return false;
        
    }

    public static int Trace(Vector3 start, Transform[] pos, float[] q, Vector3[] buffer, float step, float stopDist)
    {
           // zastavovací zóna musí být aspoň jako krok
        int cap = Mathf.Min(buffer.Length, 1000);   // tvrdý strop počtu bodů
        step = Mathf.Max(step, 0.005f);  
        float stop = Mathf.Max(stopDist, step);           // krok nesmí být ~0

        Vector3 p = start;
        int n = 0;
        while (n < cap)
        {
            buffer[n++] = p;
            if (NearNegative(p, pos, q, stop)) break;

            Vector3 E = At(p, pos, q);
            if (E.sqrMagnitude < 1e-12f ||
                float.IsNaN(E.x) || float.IsInfinity(E.x)) break;   // ochrana proti nesmyslu

            p += E.normalized * step;
        }
        return n;
    }
}
