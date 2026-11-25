
using UnityEngine;

public class UFOController : Controler
{
    public override void MakeDecisions()
    {
        if (pawn == null)
            return;

        if (GameManager.instance == null)
            return;

        PlayerController playerController =
            GameManager.instance.playerController;

        if (playerController == null ||
            playerController.pawn == null)
            return;

        UFOPawn ufo = pawn as UFOPawn;

        if (ufo == null)
            return;

        Vector3 playerPosition =
            playerController.pawn.transform.position;

        ufo.MoveToward(playerPosition);
    }
}
