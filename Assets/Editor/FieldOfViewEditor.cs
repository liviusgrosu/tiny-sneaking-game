using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;

        // Far radius can't go smaller then near radius
        if (fov.FarRadius < fov.NearRadius)
        {
            fov.FarRadius = fov.NearRadius;
        }

        // Draw the near sight radius circle
        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.NearRadius);

        // Get the directions of each fov side
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        // Draw the near sighted viewing angles 
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.NearRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.NearRadius);

        // Draw the far sight radius circle
        Handles.color = Color.yellow;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.FarRadius);

        // Draw the far sighted viewing angles 
        Handles.DrawLine(fov.transform.position + viewAngle01 * fov.NearRadius, fov.transform.position + viewAngle01 * fov.FarRadius);
        Handles.DrawLine(fov.transform.position + viewAngle02 * fov.NearRadius, fov.transform.position + viewAngle02 * fov.FarRadius);

        // Draw the far line
        if (fov.FOVRegion == 1)
        {
            Handles.color = Color.yellow;
            Handles.DrawLine(fov.transform.position, fov.PlayerRef.transform.position);
        }
        
        // Draw the near line
        else if (fov.FOVRegion == 2)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fov.transform.position, fov.PlayerRef.transform.position);
        }
        
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}