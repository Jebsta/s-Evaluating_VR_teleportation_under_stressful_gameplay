using UnityEngine;

public class RotationLock : MonoBehaviour
{
    public Transform target;     // Das Transform, dessen Rotation gesperrt werden soll
    private bool rotationLocked = false;
    private Quaternion savedRotation;

    void LateUpdate()
    {
        if (rotationLocked && target != null)
        {
            target.rotation = savedRotation;    // Rotation beibehalten
        }
    }

    // Wird von deinem Button aufgerufen
    public void ToggleRotationLock()
    {
        rotationLocked = !rotationLocked;

        if (rotationLocked)
            savedRotation = target.rotation;    // Rotation merken
    }
}
