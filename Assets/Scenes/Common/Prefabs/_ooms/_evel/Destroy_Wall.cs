using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Wall : MonoBehaviour
{
    public GameObject destroyWall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetMouseButtonDown(0) == true)
        {
            destroyWall.SetActive(false);
           print("hi"); 
        }
        
        
    }


}
