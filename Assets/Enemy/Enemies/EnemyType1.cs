using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    public override int PointValue => 1;

    public override void Die(Vector3 deathVector)
    {
        DeathEffect();

        Utilities.Scene.findExactlyOne<ScoreManager>().AddScore(PointValue);
        Destroy(gameObject);
    }
}
