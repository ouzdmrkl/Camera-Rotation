using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumTouch : MonoBehaviour
{
        // Screen width variable
        float width;
        float height;
    
        // To control which side we rotate our camera (-1 and 1, left and right)
        float axis = 0f;
    
        [Header("Release Speed")]
        [Range(75, 450)]
        public float release_Speed_Value;
        float release_Speed;
    
        // Set a vector target
        [Header("Vector Variables")]
        public Transform target;
        public int distance;

        void Awake()
        {
            width = (float)Screen.width / 2.0f;
            height = (float) Screen.height / 2.0f;
            
            // Set camera position 
            transform.position = new Vector3(distance, distance, distance);
            
            transform.LookAt(target);
        }
    
        private void Update() {
    
            // If we are touching the screen,
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
    
                // Where is touch on world space
                Vector2 pos = touch.position;
    
                // This purePos is something between (0, x) (x is your max screen size)
                float purePos = pos.x;
    
                // Get y value and limit the touchable screen space
                float y_Pos = pos.y;
                
                // This pos.x is something between (-1, 1)
                pos.x = (pos.x - width) / width;
                
                y_Pos = (pos.y - height) / height;
    
                // Get where the touch begins, then set the axis and turn your camera to the right direction
                if(touch.phase == TouchPhase.Began){
    
                    if(Mathf.Sign(pos.x) > 0){
    
                        axis = -1;
                    }
    
                    else if(Mathf.Sign(pos.x) < 0){
                        
                        axis = 1;
                    }
                }
    
                // Move the camera if the screen has the finger moving
                if (touch.phase == TouchPhase.Moved)
                {
                    if (y_Pos < 0.4f && y_Pos > -0.4f)
                    {
                        float absPosX = Mathf.Abs(pos.x);
    
                        CheckTouch(purePos);
    
                        // For rotating
                        transform.RotateAround(target.transform.position, Vector3.up, axis * /*absPosX * */ 3f);
                            
                        transform.LookAt(target);
    
                        release_Speed = release_Speed_Value;
                    }
                }
            }
    
            // If there is no touch, start the slowly stop coroutine
            else if (Input.touchCount == 0){
    
                // If our releaseSpeed is 0, that means we don't have to keep Coroutine working, stop it
                if(release_Speed < 0.1f){
    
                    StopCoroutine(SlowlyStop());
                }
    
                else if(release_Speed == release_Speed_Value){
    
                    StartCoroutine(SlowlyStop());
                }
            }
        }
    
        // See the pervious posx value, this means you can change the turn axis at any point on the screen
        float perv_Value = 0;
        void CheckTouch(float currentValue){
    
            if(currentValue > perv_Value){
    
                axis = 1f;
            }
    
            else if(currentValue < perv_Value){
    
                axis = -1f;
            }
    
            perv_Value = currentValue;
        }    
    
        // When you stop rotating with touch, rotation will stops slowly
        IEnumerator SlowlyStop(){
    
            while(release_Speed > 0){
    
                transform.RotateAround(target.transform.position, Vector3.up, axis * release_Speed * Time.deltaTime);
    
                release_Speed = release_Speed - (250f * Time.deltaTime);
    
                yield return null;
            }
        }
}
