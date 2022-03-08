using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumCamera : MonoBehaviour
{
    [Header("Look Target")]
    public Transform target;

    [Header("Cam Attributes")] 
    public int distance;
    
    public int height;

    public float sensitivity = 6f;

    [Header("Release Speed")] 
    [Range(150, 450)]
    public int release_Speed;
    
    private float sensitivity_Decrease;
    
    private float rotate_Horizontal; // Mouse.X input
    
    private bool movement_Started; // Allows camera rotate slowly when button released

    private void Start()
    {
        this.transform.position = new Vector3(distance, height, distance); // Set camera position
        
        transform.LookAt(target);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Get the mouse button click
        {
            movement_Started = true;

            rotate_Horizontal = Input.GetAxis("Mouse X");
            
            if (rotate_Horizontal < 0)
                sensitivity_Decrease = -release_Speed; // After we release mouse, sensitivity_Decrease will turn cam

            else if(rotate_Horizontal > 0)
                sensitivity_Decrease = release_Speed;

            // For rotating
            transform.RotateAround(target.transform.position, Vector3.up, rotate_Horizontal * sensitivity);
            
            transform.LookAt(target); // Make camera look at target
        }

        else
        {
            if (movement_Started)
            {
                if (sensitivity_Decrease > 0)
                {
                    // Added Time.deltaTime to add smoothness
                    transform.RotateAround(target.transform.position, Vector3.up, sensitivity_Decrease * Time.deltaTime);
                    sensitivity_Decrease--;
                }
                else
                {
                    movement_Started = false;
                }
            }

            else if (sensitivity_Decrease < 0)
            {
                transform.RotateAround(target.transform.position, Vector3.up, sensitivity_Decrease * Time.deltaTime);
                sensitivity_Decrease++;
            }
            
            else
            {
                movement_Started = false;
            }
            
            transform.LookAt(target);
        }
    }
}
