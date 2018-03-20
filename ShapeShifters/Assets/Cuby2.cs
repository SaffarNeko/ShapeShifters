using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuby2 : MonoBehaviour {

    public float torque;
    public Rigidbody2D rb;
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        float turn = Input.GetAxisRaw("Horizontal");
        //rb.AddTorque(transform.forward * torque * turn);
        rb.AddTorque(torque * turn);
    }
}
