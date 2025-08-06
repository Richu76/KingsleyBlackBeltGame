using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    protected override void Start()
    {
        base.Start();
        if(GameManager.Instance.wave >= 50){
            health = 1000;
            damage = 200;
            speed = 9;
            goldDrops = 15;
        } else {
        health = 200;
        damage = 250;
        speed = 10;
        goldDrops = 10;
        }
    }
}
