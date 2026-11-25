
using UnityEngine;

public class UFOPawn : Pawn
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;

    public void MoveToward(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        // Turn toward the player.
        Quaternion targetRotation =
            Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );

        // Move directly by changing the Transform position.
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerPawn player = other.GetComponentInParent<PlayerPawn>();

        if (player == null)
            return;

        // Destroy the player pawn so GameManager detects the loss.
        Destroy(player.gameObject);

        if (GameManager.instance != null)
        {
            GameManager.instance.EndGame(false);
        }
    }

    // These methods are required by the abstract Pawn class.
    // The UFO's AI uses MoveToward instead.

    public override void MoveForward() { }
    public override void MoveBackward() { }
    public override void YawClockwise() { }
    public override void YawCounterclockwise() { }
    public override void RollClockwise() { }
    public override void RollCounterclockwise() { }
    public override void PitchClockwise() { }
    public override void PitchCounterclockwise() { }
    public override void Shoot() { }
}
