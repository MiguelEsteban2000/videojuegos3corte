using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaVapor : MonoBehaviour
{
    [SerializeField] float speedBullet;
    Animator myAnimator;
    bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        
            myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
            transform.Translate(new Vector2(-transform.localScale.x * speedBullet * Time.deltaTime,transform.localScale.x * speedBullet * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("enemigo"))
        {
            myAnimator.SetBool("destroy", true);
            pause = true;
        }
    }
}
