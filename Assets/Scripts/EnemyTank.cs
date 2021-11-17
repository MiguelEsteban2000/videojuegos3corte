using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] GameObject bala;
    [SerializeField] float nextFire;
    [SerializeField] int vida;
    [SerializeField] AudioClip sMuerte;
    Animator myAnimator;

    float canFire;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapArea(transform.position - new Vector3(transform.localScale.x, 0, 0), transform.position - new Vector3(transform.localScale.x * range, 0, 0), LayerMask.GetMask("Player")) != null)
            Fire();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
        Gizmos.DrawLine( transform.position-new Vector3(transform.localScale.x,0, 0) , transform.position -new Vector3(transform.localScale.x*range, 0, 0));
    }

    void Fire() 
    {
            if (Time.time >= canFire)
            {
              canFire = Time.time + nextFire;
              Instantiate(bala, transform.position - new Vector3(transform.localScale.x * 2, 0, 0), transform.rotation).transform.localScale = new Vector3(transform.localScale.x, 1f, 1f); ;
            }   
            // myAnimator.SetTrigger("shootidleTG");
            // myAnimator.SetBool("shootBool", true);
            //myAnimator.SetLayerWeight(1, 1);
        

        // else
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bala"))
        {
            vida--;
            if (vida <= 0) { 
                AudioSource.PlayClipAtPoint(sMuerte, Camera.main.transform.position);
                myAnimator.SetBool("dead", true);
            }
        }
    }
}
