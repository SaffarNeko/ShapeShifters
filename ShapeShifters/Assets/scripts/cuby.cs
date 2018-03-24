using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuby : MonoBehaviour
{

    public float speed, jumps, gravityMultiplier;


    public Vector3 newScale, normalScale;

    public GameManager GM;
    public roundy round;
    public spiky spik;

    public KeyCode Jump;
    public string Horizontal;
    public string Vertical;
    public KeyCode Scale;

    public bool direction = true;
    public bool canRoll = true;
    public bool scaling=false;
    public bool onG = true;
    public bool rolling=false;

    bool canroll = true;
    bool canJump = true;
    float gravity;
    Vector3 leftRotationPoint;
    Vector3 rightRotationPoint;
    Bounds bounds;



    Rigidbody2D rb;

    private void Start()
    {
        normalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        //GM = FindObjectOfType<GameManager>();
        //round = FindObjectOfType<roundy>();
        //spik = FindObjectOfType<spiky>();
        bounds = GetComponent<BoxCollider2D>().bounds;
        leftRotationPoint = new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        rightRotationPoint = new Vector3(bounds.extents.x, -bounds.extents.y, 0);

        gravity = rb.gravityScale;


    }

    private void FixedUpdate()
    {
        if (GM.from == 0)
        {

            if (Input.GetKeyUp(Scale))
            {
                canroll = true;
                canJump = true;
            }

            if (rolling||scaling)
                return;


            if (canroll&& onG)
            {
                if (Input.GetAxisRaw(Horizontal)==-1)
                    StartCoroutine(Roll(leftRotationPoint));

                else if (Input.GetAxisRaw(Horizontal) == 1)
                    StartCoroutine(Roll(rightRotationPoint));
            }

            



            if (Input.GetKeyDown(Jump) && onG && canJump)
            {
                jump();
            }




            if (Input.GetKey(Scale))
            {
                canroll = false;
                canJump = false;
                scale();

            }
        }
        else if(GM.from !=0)
        {
            transform.position = GM.shapes[GM.activePos].transform.position;
        }


    }



    private IEnumerator Roll(Vector3 rotationPoint)
    {
        Vector3 point = transform.position + rotationPoint;
        Vector3 axis = Vector3.Cross(Vector3.up, rotationPoint).normalized;
        float angle = 90;
        float a = 0;
        rolling = true;
        rb.gravityScale = 0;
        yield return null;
        while (angle > 0)
        {
            rb.gravityScale = 0;
            a = Time.deltaTime * speed;
            transform.RotateAround(point, axis, a);
            angle -= a;
            yield return null;
        }
        transform.RotateAround(point, axis, angle);
        bounds = GetComponent<BoxCollider2D>().bounds;
        leftRotationPoint = new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        rightRotationPoint = new Vector3(bounds.extents.x, -bounds.extents.y, 0);
        rb.gravityScale = gravity;
        direction = !direction;
        yield return new WaitForSeconds(.2f);
        rolling = false;
    }

    void scale()
    {

        if (direction)
        {
            if (Input.GetAxisRaw(Horizontal) != 0)
            {
                StartCoroutine(ScaleX());
            }
            if (Input.GetAxisRaw(Vertical) != 0)
            {
                StartCoroutine(ScaleY());
            }
        }
        else
        {
            if (Input.GetAxisRaw(Horizontal) != 0)
            {
                StartCoroutine(ScaleY());
            }
            if (Input.GetAxisRaw(Vertical) != 0)
            {
                StartCoroutine(ScaleX());
            }
        }
        
    }

    private IEnumerator ScaleX()
    {
        
        canRoll = false;
        rb.mass = rb.mass * 5;
        scaling = true;
        while (transform.localScale.x<newScale.x)
        {
            transform.localScale += Vector3.right*.01f;
            yield return null;
        }
        bounds = GetComponent<BoxCollider2D>().bounds;
        leftRotationPoint = new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        rightRotationPoint = new Vector3(bounds.extents.x, -bounds.extents.y, 0);
        canRoll = true;
        scaling = false;

        yield return new WaitForSeconds(5);

        canRoll = false;
        rb.mass = rb.mass / 5;
        scaling = true;
        while(rolling)
        {
            yield return null;
            canRoll = false;
        }
        
            while (transform.localScale.x > normalScale.x)
            {
                transform.localScale -= Vector3.right * .01f;
                yield return null;
            }
        
        bounds = GetComponent<BoxCollider2D>().bounds;
        leftRotationPoint = new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        rightRotationPoint = new Vector3(bounds.extents.x, -bounds.extents.y, 0);
        yield return new WaitForSeconds(.3f);
        scaling = false;
        canRoll = true;
        


    }
    private IEnumerator ScaleY()
    {
        canRoll = false;
        scaling = true;
        rb.mass = rb.mass * 5;
        rb.gravityScale += 1;
        while (transform.localScale.y < newScale.y)
        {
            transform.localScale += Vector3.up * .01f;
            yield return null;
        }
        bounds = GetComponent<BoxCollider2D>().bounds;
        leftRotationPoint = new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        rightRotationPoint = new Vector3(bounds.extents.x, -bounds.extents.y, 0);
        canRoll = true;
        scaling = false;

        yield return new WaitForSeconds(5);

        canRoll = false;
        rb.mass = rb.mass / 5;
        rb.gravityScale -= 1;
        scaling = true;
        while(rolling)
        { 
            yield return null;
            canRoll = false;
        }
        
            while (transform.localScale.y > normalScale.y)
            {
                transform.localScale -= Vector3.up * .01f;
                yield return null;
            }
        
        bounds = GetComponent<BoxCollider2D>().bounds;
        leftRotationPoint = new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        rightRotationPoint = new Vector3(bounds.extents.x, -bounds.extents.y, 0);
        yield return new WaitForSeconds(.3f);
        canRoll = true;
        scaling = false;
        


    }


    void jump()
    {
        if (onG == true)
        {
            onG = false;
            rb.velocity += Vector2.up * jumps;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            onG = true;
            rb.gravityScale = 6;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            onG = false;
            rb.gravityScale = gravityMultiplier;
        }
    }
}
