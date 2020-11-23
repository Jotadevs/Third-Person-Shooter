using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public bool inWall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            inWall = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            inWall = false;
        }
    }
}
