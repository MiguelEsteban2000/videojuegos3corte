using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] AudioClip sMuerte;
    [SerializeField] int vida;
    Animator myAnimator;
    Rigidbody2D myBody;
    CircleCollider2D cc;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player")) != null)
            //Debug.Log("Mamauevo");*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,1f,1f,0.5f);
        Gizmos.DrawSphere(transform.position, range);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bala"))
        {
            vida--;
            if (vida <= 0)
                StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        //pause = true;
        myBody.isKinematic = true;

        cc.enabled = false;
        myAnimator.SetBool("mEnemigo", true);
        AudioSource.PlayClipAtPoint(sMuerte, Camera.main.transform.position);
        yield return new WaitForSeconds(1); 
        Destroy(gameObject);
    }
}
