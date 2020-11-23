using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class CharacterAnimBasedMovement : MonoBehaviour
{
    public float rotationSpeed = 4f;
    public float rotationThreshold = 0.3f;

    [Header("Animators Parameters")]
    public string motionParam = "motion";
    public string mirrorIdleParam = "mirrorIdle";
    public string turn180Param = "turn180";
    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    private float Speed;
    public float degreesToTurn;
    public float distanceToStop;
    public float distanceToFall;
    private Vector3 desiredMoveDirection;
    private CharacterController characterController;
    private Animator animator;
    
    private bool turn180;
    private bool mirrorIdle;
    public bool idle;
    public bool inWall;
    public bool inJump;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.visible = false;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        RaycastHit hit;
        Ray checker = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(checker, out hit, distanceToStop))
        {
            inWall = true;
            animator.SetBool("inWall", true);
        }
        else
        {
            inWall = false;
            animator.SetBool("inWall", false);
        }
    }
    public void moveCharacter(float hInput, float vInput, Camera cam, bool jump, bool dash)
    {
        //Calculate the Input Magnitude
        Speed = new Vector2(hInput, vInput).normalized.sqrMagnitude;

        //Dash only if character has reached maxSpeed (animator parameter value)
        //if (inWall)
        //{
        //    //Speed = 0f;
        //    animator.SetFloat(motionParam, Speed, StartAnimTime, Time.deltaTime);
        //    inWall = false;
        //}
        //else
        //{
        //    Debug.Log("Sales");
        //    Speed = new Vector2(hInput, vInput).normalized.sqrMagnitude;
        //}
        if (Speed >= Speed - rotationThreshold && dash)
        {
            Speed = 1.5f;
        }
        if (jump)
        {
            inJump = true;
            animator.SetBool("jump", true);
        }
        else
        {
            animator.SetBool("jump", false);
            inJump = false;
        }

        if (dash)
        {
            animator.SetBool("bigJump", true);
        }
        else animator.SetBool("bigJump", false);
        if (Speed == 0f)
        {
            idle = true;
            animator.SetBool("standing", true);
        }
        else
        {
            animator.SetBool("standing", false);
            idle = false;
        }
        //Physically move player
        if (Speed > rotationThreshold)
        {
            animator.SetFloat(motionParam, Speed, StartAnimTime, Time.deltaTime);
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            //Rotate the character towards desired move direction  based on player input and camera positon
            desiredMoveDirection = forward * vInput + right * hInput;

            if(Vector3.Angle(transform.forward, desiredMoveDirection) >= degreesToTurn)
            {
                turn180 = true;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed * Time.deltaTime);
                turn180 = false;
            }

            //180 turning
            animator.SetBool(turn180Param, turn180);

            //Move the character
            animator.SetFloat(motionParam, Speed, StartAnimTime, Time.deltaTime);

        }else if(Speed < rotationThreshold){
            animator.SetBool(mirrorIdleParam, mirrorIdle);
            animator.SetFloat(motionParam, Speed, StopAnimTime, Time.deltaTime);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (Speed < rotationThreshold) return;

        float distanceToLeftFoot = Vector3.Distance(transform.position, animator.GetIKPosition(AvatarIKGoal.LeftFoot));
        float distanceToRightFoot = Vector3.Distance(transform.position, animator.GetIKPosition(AvatarIKGoal.RightFoot));

        //Right foot in front
        if(distanceToRightFoot > distanceToLeftFoot)
        {
            mirrorIdle = true;
        }
        else
        {
            mirrorIdle = false;
        }
    }
    

}
