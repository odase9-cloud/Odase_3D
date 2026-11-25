
using UnityEngine;

public class CameraFollowPawn : MonoBehaviour
{
    private Transform target;

    private Vector3 localOffset;
    private Quaternion relativeRotation;
    private bool offsetInitialized;

    private void LateUpdate()
    {
        // Find the pawn through your existing GameManager.
        if (!offsetInitialized)
        {
            if (GameManager.instance == null ||
                GameManager.instance.playerController == null ||
                GameManager.instance.playerController.pawn == null)
            {
                return;
            }

            target = GameManager.instance.playerController.pawn.transform;

            // Preserve the camera's current position relative to the pawn.
            localOffset = target.InverseTransformPoint(transform.position);

            // Preserve the current camera angle relative to the pawn.
            relativeRotation =
                Quaternion.Inverse(target.rotation) * transform.rotation;

            offsetInitialized = true;
        }

        // A destroyed Unity object compares equal to null.
        // Do nothing when the pawn is gone, keeping the last camera view.
        if (target == null)
            return;

        // Follow the pawn using the saved local-space offset.
        transform.position = target.TransformPoint(localOffset);

        // Follow its rotation without changing the original relative angle.
        transform.rotation = target.rotation * relativeRotation;
    }
}
