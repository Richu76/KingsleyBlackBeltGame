using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : HumanoidAI
{
    public static Enemy Insta;
    public Target curWall;
    public Rigidbody rb;
    public bool inPosition = false;
    public int goldDrops;

    enum Behaviours { WallOnly, TroopOnly, WallWhenNoTroop }
    [SerializeField]
    private Behaviours behaviour = Behaviours.WallOnly;

    void awake(){
        Insta = this;
    }

    protected override void Start()
    {
        base.Start();
        health = maxHealth;
        GameManager.Instance.enemies.Add(gameObject);
        var rb = GetComponent<Rigidbody>();
    }

    public override void TakeDamage(int Tdamage)
    {
        health -= Tdamage;
        if (health <= 0)
        {
            HealthBar.fillAmount = 0;
            GameManager.Instance.gold += goldDrops;
            GameManager.Instance.enemies.Remove(gameObject);
            //Dragnir.Instan.Update();
            Destroy(gameObject);
        }
        else
        {
            HealthBar.fillAmount = (float)health / maxHealth;
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Enemy Collided");
        if (c.gameObject.CompareTag("Wallas") && canAttack)
        {
            curWall = c.gameObject.GetComponentInChildren<Target>();
            isAttacking = true;
        }
        if (c.gameObject.CompareTag("Troop") && canAttack)
        {
            //Debug.Log("Enemy Collided and Attacking");
            curTarget = c.gameObject;
            isAttacking = true;
        }
    }

    public void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Wallas") && canAttack)
        {
            curWall = null;
            isAttacking = false;
        }
        if (c.gameObject.CompareTag("Troop") && canAttack)
        {
            curTarget = null;
            isAttacking = false;
        }
    }

   

    new protected virtual void Update()
    {
        //Debug.Log("Update running");
        if (behaviour == Behaviours.WallOnly || (behaviour == Behaviours.WallWhenNoTroop && GameManager.Instance.Troops.Count <= 0))
        {
            if (!GameManager.Instance.wallsDead)
            {
                if (canAttack && curWall != null)
                {
                    MoveToTarget(curWall);
                }

                inPosition = curWall != null && Vector3.Distance(curWall.transform.position, transform.position) < 5;

                if (isAttacking && canAttack && inPosition)
                {
                    DoDamage();
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                if (curWall == null)
                {
                    foreach (Target t in GameManager.Instance.targets)
                    {
                        //Debug.Log("Checking targets");
                        if (curWall == null)
                        {

                            curWall = t;
                        }
                        else if (t != null && Vector3.Distance(t.gameObject.transform.position, transform.position) < Vector3.Distance(curWall.transform.position, transform.position))
                        {
                            curWall = t;
                        }
                    }
                    //Debug.Log(gameObject.name + " is moving");
                }
            }
        }
        else
        {
            if (!GameManager.Instance.wallsDead)
            {
                if (curTarget != null && canAttack)
                {
                    MoveToTarget(curTarget);
                }

                if (isAttacking && canAttack)
                {
                    DoDamageToTroop();
                }
                if (curTarget == null)
                {
                    foreach (GameObject t in GameManager.Instance.Troops)
                    {
                        if (curTarget == null)
                        {
                            isAttacking = false;
                            curTarget = t;
                        }
                        else if (Vector3.Distance(t.gameObject.transform.position, transform.position) < Vector3.Distance(curTarget.transform.position, transform.position))
                        {
                            curTarget = t;
                        }
                    }
                    //Debug.Log(gameObject.name + " is moving");
                }
            }
        }

    }
    public override void DoDamage()
    {
        curWall.transform.parent.GetComponent<WallHealth>().TakeDamage(damage);
        canAttack = false;
        StartCoroutine(WaitOnAttack());
    }

    public void DoDamageToTroop()
    {
        curTarget.transform.GetComponent<Troops>().TakeDamage(damage);
        canAttack = false;
        StartCoroutine(WaitOnAttack());
    }
}
