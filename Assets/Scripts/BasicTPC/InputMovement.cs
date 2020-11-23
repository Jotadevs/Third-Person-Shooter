using System;
using UnityEngine;

[Serializable]
public struct InputData
{
    //Basic Movement
    public float hMovement;
    public float vMovement;

    //Mouse Rotation
    public float verticalMouse;
    public float horizontalMouse;

    //Extra Movement
    public bool dash;
    public bool jump;

    public void getInput()
    {
        //Basic Movement
        hMovement = Input.GetAxis("Horizontal");
        vMovement = Input.GetAxis("Vertical");

        //Mouse/Joystick rotation
        verticalMouse = Input.GetAxis("Mouse Y");
        horizontalMouse = Input.GetAxis("Mouse X");

        //Extra Movement
        dash = Input.GetButton("Dash");
        jump = Input.GetButtonDown("Jump");
    } 
}
public class InputMovement : MonoBehaviour
{

}
