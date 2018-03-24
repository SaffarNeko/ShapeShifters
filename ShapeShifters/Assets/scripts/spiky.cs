using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiky : MonoBehaviour
{

    public float speed, smooth, booster, gravityMultiplier;

    public GameManager GM;
    public cuby cub;
    public roundy round;

    float targetSpeed, currentSpeed, x;

    Rigidbody2D rb;
    public KeyCode Jump;
    public string Horizontal;
    public string Vertical;
    public KeyCode boost;
    public Vector2 boostDirection;
    public float maxBoostValue, jumps;
    private float currentBoostValue = 0;
    public float boostAdditiveValue;
    private float initialGravityScale;

    bool canMove = true, onG = true;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        //GM = FindObjectOfType<GameManager>();
        //cub = FindObjectOfType<cuby>();
        //round = FindObjectOfType<roundy>();
        initialGravityScale = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        if (GM.from == 2 && canMove)
        {
            move();
          
        }
        else if(GM.from != 2)
        {
            transform.position = GM.shapes[GM.activePos].transform.position;
        }

        if (Input.GetKeyDown(Jump) && onG)
        {
            jump();
        }

        if (Input.GetKey(boost))
        {
            canMove = false;
            boostDirection = new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical));
            if (currentBoostValue < maxBoostValue)
            {
                currentBoostValue += boostAdditiveValue;

            }

        }

        if (Input.GetKeyUp(boost))
        {
            rb.velocity += boostDirection * currentBoostValue;
            //rb.velocity = Vector2.Lerp(rb.velocity, boostDirection * currentBoostValue, Time.deltaTime * smooth);
            StartCoroutine(moveDelay());    
            //canMove = true;
            //currentBoostValue = 0;
        }
    }

    void move()
    {
        float h = Input.GetAxisRaw(Horizontal);
        targetSpeed = h * speed;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * smooth);

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float targetB = booster * h;
            x = Mathf.Lerp(x, targetB, Time.deltaTime * smooth);
            rb.velocity = new Vector2(x, rb.velocity.y);
        }
    }


    IEnumerator moveDelay()
    {
        while (rb.velocity!=Vector2.zero)
        {
            yield return null;
        }
        canMove = true;
    }

    void jump()
    {
        if (onG == true)
        {
            onG = false;
            rb.velocity += Vector2.up * jumps;
            //rb.AddForce(new Vector2(rb.velocity.x, jumps));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            onG = true;
            rb.gravityScale = initialGravityScale;
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
