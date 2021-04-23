using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear_Blocks : MonoBehaviour
{
    public GameObject falseWall;
    private int counter = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("Player") && counter == 1)
        {
            falseWall.SetActive(false);

            counter = 0;
        }
        else
        {
            falseWall.SetActive(true);

            counter = 1;
        }
    }

}
