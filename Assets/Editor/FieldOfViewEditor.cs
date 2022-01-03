using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;

        // Far radius can't go smaller then near radius
        if (fov.FarRadius < fov.NearRadiusCurrent)
        {
            fov.FarRadius = fov.NearRadiusCurrent;
        }
        
        Handles.color = Color.red;

        // Get the directions of each fov side
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        // Draw the near sight radius circle
        Handles.DrawWireArc(fov.transform.position, 
                            Vector3.up, 
                            viewAngle01, 
                            fov.angle, 
                            fov.NearRadiusCurrent);

        // Draw the near sighted viewing angles 
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.NearRadiusCurrent);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.NearRadiusCurrent);

        // Draw the far sight radius circle
        Handles.color = Color.yellow;
        Handles.DrawWireArc(fov.transform.position,
                            Vector3.up,
                            viewAngle01,
                            fov.angle,
                            fov.FarRadius);

        // Draw the far sighted viewing angles 
        Handles.DrawLine(fov.transform.position + viewAngle01 * fov.NearRadiusCurrent, fov.transform.position + viewAngle01 * fov.FarRadius);
        Handles.DrawLine(fov.transform.position + viewAngle02 * fov.NearRadiusCurrent, fov.transform.position + viewAngle02 * fov.FarRadius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}