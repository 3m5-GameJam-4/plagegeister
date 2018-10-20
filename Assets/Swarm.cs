using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    public Player player;

    void Start()
    {
        if (!player) return;

        var renderer = GetComponent<SpriteRenderer>();
        if (renderer) updateRenderer(renderer);

        var moveByForce = GetComponent<MoveByForce>();
        if (moveByForce) updateMoveByForce(moveByForce);
    }

    private void updateRenderer(SpriteRenderer renderer)
    {
        if (renderer.color != player.color)
        {
            renderer.color = player.color;
        }
    }

    private void updateMoveByForce(MoveByForce move)
    {
        move.horizontalAxis = player.inputHorizontalAxis;
        move.verticalAxis = player.inputVerticalAxis;
    }
}