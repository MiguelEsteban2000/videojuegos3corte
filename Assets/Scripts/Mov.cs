using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mov : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] BoxCollider2D pies;
    [SerializeField] float jumpSpeed;
    [SerializeField] float speedDash;
    [SerializeField] float dashSeconds;
    [SerializeField] GameObject bala;
    [SerializeField] float nextFire;
    [SerializeField] GameObject vfxMuerte;
    [SerializeField] AudioClip sDisparar;
    [SerializeField] AudioClip sSaltar;
    [SerializeField] AudioClip sCaer;
    [SerializeField] AudioClip sDash;
    [SerializeField] AudioClip sMuerte;
    [SerializeField] bool inmortal;

    CircleCollider2D cc;
    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    float canFire;
    float isDashing=0;
    bool canJump=true;
    bool pause = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();

        //StartCoroutine(ShowTime());
    }

    void Update()
    {
        if (!pause)
        {
            Mover();
            Saltar();
            Falling();
            Fire();
            Dash();
            DobleSalto();
        }

    }

    void Dash()
    {
        
        if (Input.GetKey(KeyCode.C))
        {
            if (isDashing <= dashSeconds){
                myAnimator.SetBool("dashing", true);
                AudioSource.PlayClipAtPoint(sDash, Camera.main.transform.position);
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
                AudioSource.PlayClipAtPoint(sDisparar, Camera.main.transform.position);
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
                AudioSource.PlayClipAtPoint(sSaltar, Camera.main.transform.position);
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
        if (myBody.velocity.y < 0 && !myAnimator.GetBool("jumping")) { 
            myAnimator.SetBool("falling", true);
        }
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
                    AudioSource.PlayClipAtPoint(sSaltar, Camera.main.transform.position);
                    myBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemigo")) 
        {
            if (!inmortal) { 
            StartCoroutine("Die");
            }
        }
    }

    IEnumerator Die()
    {
        pause = true;
        myCollider.enabled = false;
        myBody.isKinematic = true;
        myAnimator.SetBool("falling", false);
        myAnimator.SetBool("muerte", true);
        yield return new WaitForSeconds(1);
        AudioSource.PlayClipAtPoint(sMuerte, Camera.main.transform.position);
        Instantiate(vfxMuerte, transform.position, transform.rotation);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Muerte");

        
     
    }
}
