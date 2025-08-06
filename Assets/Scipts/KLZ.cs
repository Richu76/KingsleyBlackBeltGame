using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLZ : Enemy
{
    protected override void Start()
    {
        base.Start();
        health = 100000;
        damage = 5000;
        speed = 3;
        goldDrops = 100;
    }
}
