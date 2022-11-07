using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyVariablesInput : MonoBehaviour
{

    public InputField jumpSpeedI;
    public InputField massI;
    public InputField gravityI;
    public InputField linearDragI;


    public int jumpSpeed;
    public int mass;
    public int gravity;
    public int linearDrag;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateJumpSpeed()
    {
        jumpSpeed = (jumpSpeedI.text == "" ? 0 : Int16.Parse(jumpSpeedI.text));
    }

    public void updateMass()
    {
        mass = (massI.text == "" ? 0 : Int16.Parse(massI.text));
    }

    public void updateGravity()
    {
        gravity = (gravityI.text == "" ? 0 : Int16.Parse(gravityI.text));
    }

    public void updateLinearDrag()
    {
        linearDrag = (linearDragI.text == "" ? 0 : Int16.Parse(linearDragI.text));
    }
}
