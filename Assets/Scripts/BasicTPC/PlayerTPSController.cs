using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTPSController : MonoBehaviour
{
    public Camera cam;
    
    private InputData input;
    private CharacterAnimBasedMovement characterMovement;
    private WallChecker WallChecker;

    // Start is called before the first frame update
    void Start()
    {
        characterMovement = GetComponent<CharacterAnimBasedMovement>();
        WallChecker = GetComponentInChildren<WallChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get input from player
        input.getInput();


        float newVertical = input.vMovement;
        float newHorizontal = input.hMovement;

        if (newHorizontal >= 1f && WallChecker.inWall)
        {
            newHorizontal = 0;
        }
        else if (newVertical >= 1f && WallChecker.inWall)
        {
            newVertical = 0;
        }


        //Apply input to character
        characterMovement.moveCharacter(newHorizontal, newVertical, cam, input.jump, input.dash);
    }
}
