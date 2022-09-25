using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationInput : MonoBehaviour
{
    public Animator anim;
    public string animationName;
    public string prevInput;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        string keyPressed;
        if (!Input.inputString.Equals(""))
        {
            keyPressed = Input.inputString;
            prevInput = keyPressed;
        } else
        {
            keyPressed = prevInput;
        }
        Debug.Log(keyPressed);

        switch(keyPressed)
        {
            case "1":
                animationName = "idle";
                break;
            case "2":
                animationName = "run";
                break;
            case "3":
                animationName = "attack";
                break;
            case "4":
                animationName = "damage";
                break;
            case "5":
                animationName = "climb";
                break;
            case "6":
                animationName = "jump";
                break;
            case "7":
                animationName = "fall";
                break;
            case "8":
                animationName = "death";
                break;
            default:
                animationName = "idle";
                break;
        }

        anim.Play(animationName);
    }
}
