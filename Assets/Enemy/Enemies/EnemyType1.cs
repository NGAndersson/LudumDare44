using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    public override int PointValue => 1;

    static ScoreManager scoreManager = null;

    public override void Die(Vector3 deathVector)
    {
        if (scoreManager == null)
        {
            Utilities.Scene.findExactlyOne<ScoreManager>().AddScore(PointValue);
        }

        DeathEffect(deathVector);
        gameObject.SetActive(false);
    }
}
