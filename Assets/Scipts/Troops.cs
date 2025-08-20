using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troops : HumanoidAI
{
    // Update is called once per frame
    public static Troops Instance;

    //public Collider col = .GetComponent<BoxCollider>();

    public void Awake(){
        Instance = this;
    }

    public override void TakeDamage(int Tdamage)
    {
        health -= Tdamage;
        if (health <= 0)
        {
            HealthBar.fillAmount = 0;
            TroopSpawner.Instanc.HeroActive = false;
            //Debug.Log("TS Instanc Boolean Change");
            GameManager.Instance.Troops.Remove(gameObject);
            Destroy(gameObject);
            GameManager.Instance.TTroops--;    
        }
        else
        {
            HealthBar.fillAmount = (float)health / maxHealth;
        }
    }

     new protected virtual void Update()
    {
        {
            if (!GameManager.Instance.wallsDead)
            {
                if (curTarget != null && canAttack)
                {
                    MoveToTarget(curTarget);
                }

                if (isAttacking && canAttack)
                {
                    if (curTarget != null && Vector3.Distance(curTarget.gameObject.transform.position, transform.position) < 3)
                    {
                        DoDamage();
                    }
                    
                }
                if (curTarget == null)
                {
                    foreach (GameObject t in GameManager.Instance.enemies)
                    {
                        if (curTarget == null)
                        {
                            isAttacking = false;
                            curTarget = t;
                        }
                        if (Vector3.Distance(t.gameObject.transform.position, transform.position) < Vector3.Distance(curTarget.transform.position, transform.position))
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
        //Debug.Log("Attacking");
        //AudioManager.Instan.Attack.Play();
        curTarget.transform.GetComponent<Enemy>().TakeDamage(damage);
        canAttack = false;
        StartCoroutine(WaitOnAttack());
    }

    new protected virtual void OnTriggerEnter(Collider c)
    {
        //Debug.Log("Collided");
        if (c.gameObject.CompareTag("Enemy") && canAttack)
        {
            //Debug.Log("Collidied and attacking");
            curTarget = c.gameObject;
            GameManager.Instance.SwingAnimation.Play();
            DoDamage();
            isAttacking = true;
            //col.isTrigger = false;
        }

        if(c.gameObject.CompareTag("Border")){
            //make the troop/enemy who touches this wall tp back to the centre of the map and reset their target
        };
    }


    public void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Enemy") && canAttack)
        {
            curTarget = null;
            isAttacking = false;
        }
    }

    new protected virtual void Start()
    {
        base.Start();
        GameManager.Instance.Troops.Add(gameObject);
    }
}
