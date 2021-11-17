using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVapor : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] GameObject bala;
    [SerializeField] GameObject bala1;
    [SerializeField] float nextFire;
    [SerializeField] AudioClip sMuerte;
    [SerializeField] int vida;
    float canFire;
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    { 
            if (Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player")) != null)
        {
            myAnimator.SetBool("near", true);
            Fire();
        }
        else 
        {
            myAnimator.SetBool("near", false);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
        Gizmos.DrawSphere(transform.position, range);
    }

    void Fire()
    {
        if (Time.time >= canFire)
        {
            canFire = Time.time + nextFire;
            Instantiate(bala, transform.position + new Vector3(-0.4f, 0, 0), transform.rotation);
            Instantiate(bala1, transform.position + new Vector3(0.4f, 0, 0), transform.rotation);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bala"))
        {
            vida--;
            if (vida <= 0)
            {
                AudioSource.PlayClipAtPoint(sMuerte, Camera.main.transform.position);
            myAnimator.SetBool("dead", true);
            }
        }
    }
}
