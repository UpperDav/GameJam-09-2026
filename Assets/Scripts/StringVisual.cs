using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class StringVisual : MonoBehaviour
{
    [Header("Refs")]
    public LimbString limbString;
    public Transform holder;

    [Header("Look")]
    public int segments = 12;
    public float slackAmplitude = 0.15f;
    public float wiggleSpeed = 1f;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = Mathf.Max(2, segments);
        line.useWorldSpace = true;

        if (limbString != null)
            limbString.OnBreak += HandleBreak;
        else
            Debug.LogWarning("LimbString reference is missing on StringVisual.", this);
    }

    void OnDestroy()
    {
        if (limbString != null)
            limbString.OnBreak -= HandleBreak;
    }

    void LateUpdate()
    {
        if (holder == null || !line.enabled)
            return;

        Vector3 start = holder.position;
        Vector3 end = transform.position;

        bool taut = limbString != null && limbString.IsPulled;
        float slack = taut ? 0f : slackAmplitude;

        Vector3 dir = (end - start).normalized;
        Vector3 perpendicular = new Vector3(-dir.y, dir.x, 0f);

        int count = line.positionCount;
        for (int i = 0; i < count; i++)
        {
            float t = i / (float)(count - 1);
            Vector3 point = Vector3.Lerp(start, end, t);

            if (slack > 0f)
            {
                float taper = Mathf.Sin(t * Mathf.PI);
                float wiggle = Mathf.Sin(Time.time * wiggleSpeed + t * 6f) * slack * taper;
                point += perpendicular * wiggle;
            }

            line.SetPosition(i, point);
        }
    }

    void HandleBreak()
    {
        line.enabled = false;
    }
}