using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMove : MonoBehaviour
{
     Vector3 pointA = new Vector3(0f, 0f, 0f);
     Vector3 pointB = new Vector3(0f, 0.14f, 0f);

    public float speed;
     

     void Update() {
         
        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }
         transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time * speed, 1));
     } 
}
