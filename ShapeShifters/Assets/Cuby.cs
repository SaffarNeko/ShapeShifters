using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuby : MonoBehaviour
{
    public float speed;
    Vector3 forwardRotationPoint;
    Vector3 backRotationPoint;
    Vector3 leftRotationPoint;
    Vector3 rightRotationPoint;
    Bounds bounds;
    bool rolling;

    bool canroll=true;
    bool canScale=true;

    public KeyCode scale;
    public Vector2 scaleDirection;
    public Vector3 maxScaleValue;
    private Vector2 minScaleValue ;
    public Vector3 scaleAdditiveValue;

    void Start()
    {
        minScaleValue = new Vector2(transform.localScale.x, transform.localScale.y);
        bounds = GetComponent<BoxCollider2D>().bounds;
        leftRotationPoint = new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        rightRotationPoint = new Vector3(bounds.extents.x, -bounds.extents.y, 0);
    }

    void Update()
    {
        bounds = GetComponent<BoxCollider2D>().bounds;
        leftRotationPoint = new Vector3(-bounds.extents.x, -bounds.extents.y, 0);
        rightRotationPoint = new Vector3(bounds.extents.x, -bounds.extents.y, 0);
        if (rolling)
            return;


        if (canroll)
        {
            if (Input.GetKey("left"))
                StartCoroutine(Roll(leftRotationPoint));

            else if (Input.GetKey("right"))
                StartCoroutine(Roll(rightRotationPoint));
        }
        

        

        if (Input.GetKey(scale)&&canScale)
        {
            scaleDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            canroll = false;
            if ((transform.localScale.x < maxScaleValue.x && scaleDirection.x==1)|| (transform.localScale.x > minScaleValue.x && scaleDirection.x==-1))
            {
                transform.localScale += new Vector3( scaleAdditiveValue.x * scaleDirection.x, 0,0);
            }
            if ((transform.localScale.y < maxScaleValue.y && scaleDirection.y==1) || (transform.localScale.y > minScaleValue.y && scaleDirection.y==-1))
            {
                transform.localScale += new Vector3(0, scaleAdditiveValue.y * scaleDirection.y, 0);
            }
        }
        if (Input.GetKeyUp(scale))
        {
            canroll = true;
            
        }

        
    }

    private IEnumerator Roll(Vector3 rotationPoint)
    {
        Vector3 point = transform.position + rotationPoint;
        Vector3 axis = Vector3.Cross(Vector3.up, rotationPoint).normalized;
        float angle = 90;
        float a = 0;
        rolling = true;
        canScale = false;

        while (angle > 0)
        {
            a = Time.deltaTime * speed;
            transform.RotateAround(point, axis, a);
            angle -= a;
            yield return null;
        }
        transform.RotateAround(point, axis, angle);
        rolling = false;
        canScale = true;
    }


}
