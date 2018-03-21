using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    cuby cub;
    roundy round;
    spiky spik;

    public int from = 0;

    public int activePos;

    Vector2 morphPos;

    public GameObject[] shapes = new GameObject[3];

	// Use this for initialization
	void Start () {
        for (int i = 1; i < shapes.Length; i++)
        {
            shapes[i].SetActive(false);
        }

        cub = FindObjectOfType<cuby>();
        round = FindObjectOfType<roundy>();
        spik = FindObjectOfType<spiky>();
    
        activePos = 0;
        from = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        morphPos = shapes[activePos].transform.position;

        if (Input.GetKeyDown(KeyCode.E))
        {
            morphPos = shapes[activePos].GetComponent<Rigidbody2D>().position;
            changeShape();
        }
	}

    void changeShape()
    {
        for (int i = 0; i < shapes.Length; i++)
        {
            if(shapes[i].activeSelf == true)
            {
                activePos = i;
            }
        }

        if(activePos == 0)
        {
            from = 1;
            morphPos = shapes[activePos].transform.position;
            shapes[0].SetActive(false);
            activePos += 1;
            shapes[activePos].SetActive(true);
            shapes[activePos].transform.position = morphPos;

            shapes[activePos].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            shapes[activePos].GetComponent<Rigidbody2D>().position = morphPos;
            //shapes[activePos].transform.position = shapes[0].transform.position;
            shapes[activePos].GetComponent<Animator>().SetInteger("fromCuby", 1);
        }

        else if (activePos == 1)
        {
            from = 2;
            morphPos = shapes[activePos].transform.position;
            shapes[activePos].SetActive(false);
            activePos += 1;
            shapes[activePos].SetActive(true);
            shapes[activePos].transform.position = morphPos;

            shapes[activePos].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            shapes[activePos].GetComponent<Rigidbody2D>().position = morphPos;
            //shapes[activePos].transform.position = shapes[1].transform.position;
            shapes[activePos].GetComponent<Animator>().SetInteger("fromRoundy", 2);
        }

        else if (activePos == 2)
        {
            from = 0;
            morphPos = shapes[activePos].transform.position;
            shapes[activePos].SetActive(false);
            activePos = 0;
            shapes[activePos].SetActive(true);
            shapes[activePos].transform.position = morphPos;

            shapes[activePos].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            shapes[activePos].GetComponent<Rigidbody2D>().position = morphPos;
            //shapes[activePos].transform.position = shapes[2].transform.position;
            shapes[activePos].GetComponent<Animator>().SetInteger("fromSpiky", 0);
        }
    }

}
