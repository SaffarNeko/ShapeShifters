using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roundy : MonoBehaviour {

    public float speed, smooth, jumps;

    GameManager GM;
    cuby cub;
    spiky spik;

    float targetSpeed, currentSpeed;

    Rigidbody2D rb;

    bool onG = true, doubleJ = false;
    

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        GM = FindObjectOfType<GameManager>();
        cub = FindObjectOfType<cuby>();
        spik = FindObjectOfType<spiky>();
    }

    private void FixedUpdate()
    {
        if (GM.from == 1)
        {
            move();
            if (Input.GetKeyDown(KeyCode.Space) && onG == true)
            {
                jump();
            }

            if (Input.GetKeyDown(KeyCode.Space) && doubleJ == true)
            {
                jump();
            }
        }
    }

    void move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        targetSpeed = h * speed;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * smooth);

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
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

        if(doubleJ == true)
        {
            rb.velocity += Vector2.up * jumps;
            //rb.AddForce(new Vector2(rb.velocity.x * 2, jumps * 1.5f));
            doubleJ = false;
        }
    }

    void Djump()
    {
        doubleJ = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "ground")
        {
            onG = true;
            doubleJ = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "ground")
        {
            onG = false;
        } 
    }

}
