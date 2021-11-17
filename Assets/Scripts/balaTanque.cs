using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balaTanque : MonoBehaviour
{
    [SerializeField] float speedBullet;
    bool pause = false;
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pause)
           Mover();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        myAnimator.SetBool("destroy", true);
        pause = true;
        // WaitForSeconds.Equals = 1;
        //Destroy(this.gameObject);
    }

    void Mover()
    {
        transform.Translate(new Vector2(-transform.localScale.x * speedBullet * Time.deltaTime, 0));
    }


}
