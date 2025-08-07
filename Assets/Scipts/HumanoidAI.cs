using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanoidAI : MonoBehaviour
{
    public int maxHealth;

    public Image HealthBar;

    public int health;

    public int damage;

    public int speed;

    public int waitTime;

    public bool canAttack = true;

    public bool isAttacking = false;

    [SerializeField]
    protected GameObject curTarget;

    //make the enemy recognize the nearest box collider 2d and target it
    protected virtual void Start()
    {
        health = maxHealth;
    }

    public virtual void MoveToTarget(object target)
    {
        if (target.GetType() == typeof(Target))
        {
            Target t = (Target)target;
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.transform.position, Time.deltaTime * speed);
            }
        }
        else if (target.GetType() == typeof(GameObject))
        {
            GameObject t = (GameObject)target;
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.transform.position, Time.deltaTime * speed); 
            }
        }


        }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            HealthBar.fillAmount = 0;
            
            Destroy(gameObject);
        }
        else
        {
            HealthBar.fillAmount = (float)health / maxHealth;
        }
    }

    public virtual void DoDamage()
    {
    }



    public void Healing()
    {

    }

    //public void Drops()




    //public void GoldDrops()



    //public void VoiceLine()



    // Start is called before the first frame update


    public IEnumerator WaitOnAttack()
    {
        yield return new WaitForSeconds(waitTime);
        //Troops.Instance.col.isTrigger = true;
        canAttack = true;
    }

}


