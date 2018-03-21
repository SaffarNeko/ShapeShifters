using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuby : MonoBehaviour {

    public float speed, smooth, fort, jumps, gravityMultiplier;
    private float targetSpeed, currentSpeed;

    private Vector3 AngleVelocity;

    public Vector3 newScale, normalScale;

    GameManager GM;
    roundy round;
    spiky spik;

    //public KeyCode jump;

    

    bool scaleNow = false, onG = true;

    Rigidbody2D rb;

    private void Start()
    {
        normalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        GM = FindObjectOfType<GameManager>();
        round = FindObjectOfType<roundy>();
        spik = FindObjectOfType<spiky>();

    }

    private void FixedUpdate()
    {
        if (GM.from == 0)
        {
            move();

            if (Input.GetKeyDown(KeyCode.UpArrow) && onG)
            {
                jump();
            }


            if (Input.GetKey(KeyCode.Space))
            {
                scaleNow = true;
            }

            if (scaleNow)
            {
                scale();
                Invoke("resScale", 8);
            }
        }
        else if(GM.from !=0)
        {
            transform.position = GM.shapes[GM.activePos].transform.position;
        }


    }

    void move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        targetSpeed = h * speed;

        //currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * smooth);
        AngleVelocity = new Vector3(0, 0, targetSpeed);
        //rb.gravityScale = 5;
        Quaternion deltaRotation = Quaternion.Euler(AngleVelocity * Time.deltaTime);


        rb.MoveRotation(rb.rotation + targetSpeed * -1 * Time.fixedDeltaTime);
        //rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    void scale()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * smooth);
            rb.mass = rb.mass * 5;
            scaleNow = false;
        }


        if (Input.GetAxisRaw("Vertical") != 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector2(transform.localScale.x,2), Time.deltaTime * smooth);
            rb.mass = rb.mass * 5;
            rb.gravityScale = rb.gravityScale += 1;
            scaleNow = false;
        }
    }

    void resScale()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, normalScale, Time.deltaTime * smooth);
    }

    void jump()
    {
        if (onG == true)
        {
            onG = false;
            rb.velocity += Vector2.up * jumps;
            //rb.AddForce(new Vector2(rb.velocity.x, jumps));
            Invoke("Djump", 0.1f);
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
