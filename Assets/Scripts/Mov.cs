using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] BoxCollider2D pies;
    [SerializeField] float jumpSpeed;

    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D myCollider;
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
    }

    void Fire() 
    {
        if (Input.GetKey(KeyCode.X)) 
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
            myAnimator.SetLayerWeight(1, 0);
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
        if (isGrounded() && !myAnimator.GetBool("jumping"))
        {
            myAnimator.SetBool("falling", false);
            myAnimator.SetBool("jumping", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                myAnimator.SetTrigger("takeOf");
                myAnimator.SetBool("jumping",true);
                myBody.AddForce(new Vector2(0, jumpSpeed),ForceMode2D.Impulse);
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
        Debug.DrawRay(myCollider.bounds.center,new Vector2(0, (myCollider.bounds.extents.y + 0.2f))*-1,Color.red);
        return myRay.collider != null;
    }

    void AfterTakeOfEvent() 
    {
        myAnimator.SetBool("jumping", false);
        myAnimator.SetBool("falling", true);
    }
}
