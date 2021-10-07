using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] BoxCollider2D pies;
    [SerializeField] float jumpSpeed;
    [SerializeField] float speedDash;
    [SerializeField] float dashSeconds;
    [SerializeField] GameObject bala;
    [SerializeField] float nextFire;

    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    float canFire;
    float isDashing=0;
    bool canJump=true;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Mover();
        Saltar();
        Falling();
        Fire();
        Dash();
        DobleSalto();
    }

    void Dash()
    {
        
        if (Input.GetKey(KeyCode.C))
        {
            if (isDashing <= dashSeconds){
                myAnimator.SetBool("dashing", true);
                transform.Translate(new Vector2(transform.localScale.x * speedDash * Time.deltaTime, 0));
                isDashing = Time.deltaTime + isDashing;
            }else{
                myAnimator.SetBool("dashing", false);
            }
        }
        else{
            myAnimator.SetBool("dashing", false);
            // isDashing = isDashing - Time.deltaTime;
            isDashing = 0;
        }
            
    }

    void Fire() 
    {
        if (Input.GetKey(KeyCode.X)) 
        {
            if(Time.time >= canFire){
                canFire = Time.time + nextFire;
                Instantiate(bala, transform.position - new Vector3(-transform.localScale.x*2, 0, 0), transform.rotation).transform.localScale = new Vector3 (transform.localScale.x, 1f, 1f);;
            }
            // myAnimator.SetTrigger("shootidleTG");
            // myAnimator.SetBool("shootBool", true);
            myAnimator.SetLayerWeight(1, 1);
        }else{
            myAnimator.SetLayerWeight(1, 0);
            // myAnimator.SetBool("shootBool", false);
        }
        // else
            
    }

    void Mover() 
    {
        float mov = Input.GetAxis("Horizontal");
        if (mov != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(mov), 1);
            myAnimator.SetBool("running", true);
            transform.Translate(new Vector2(mov * speed * Time.deltaTime, 0));
        }
        else
            myAnimator.SetBool("running", false);
    }

    void Saltar()
    {
        //bool isGrounded=pies.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGrounded())
        {
            myAnimator.SetBool("falling", false);
            myAnimator.SetBool("jumping", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canJump = true;
                myAnimator.SetTrigger("takeOf");
                myAnimator.SetBool("jumping",true);
                if (Input.GetKey(KeyCode.C)){
                    // if (isDashing <= dashSeconds){{
                    myBody.AddForce(new Vector2(0, jumpSpeed+8),ForceMode2D.Impulse);
                    // }else{

                    // }
                }else{
                    myBody.AddForce(new Vector2(0, jumpSpeed),ForceMode2D.Impulse);
                }
                
            }
        }
    }

    void Falling() 
    {
        if (myBody.velocity.y < 0 && !myAnimator.GetBool("jumping"))
            myAnimator.SetBool("falling", true);
    }
    //ray cast:
    bool isGrounded() 
    {
        //return pies.IsTouchingLayers(LayerMask.GetMask("Ground"));
        RaycastHit2D myRay= Physics2D.Raycast(myCollider.bounds.center,Vector2.down,myCollider.bounds.extents.y + 0.2f,LayerMask.GetMask("Ground"));
        // Debug.DrawRay(myCollider.bounds.center,new Vector2(0, (myCollider.bounds.extents.y + 0.2f))*-1,Color.red);
        return myRay.collider != null;
    }

    void AfterTakeOfEvent() 
    {
        myAnimator.SetBool("jumping", false);
        myAnimator.SetBool("falling", true);
    }

    void DobleSalto() 
    {
        if (!isGrounded() && canJump)
        {
            if (canJump)
            {
                myAnimator.SetBool("falling", true);
                myAnimator.SetBool("jumping", false);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    canJump = false;
                    print(canJump);
                    myAnimator.SetTrigger("takeOf");
                    myAnimator.SetBool("jumping", true);
                    myBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                }
            }
        }
    }
}
