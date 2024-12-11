using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas component

    public bool lockOnZAxis=false;
    public int Zval=0;
    void Start()
    {
        if (canvas != null)
        {
            canvas.enabled = false; // Start with the Canvas component disabled
        }
    }

    void Update()
    {
        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            // If the raycast hits an object, move the GameObject to the hit point and enable the Canvas
            if(lockOnZAxis){
                transform.position = new Vector3(hit.point.x,hit.point.y,Zval);
            }else{
                transform.position = hit.point;
            }
            

            if (canvas != null)
            {
                canvas.enabled = true; // Enable the Canvas component if hitting something
            }
        }
        else
        {
            // If the raycast doesn't hit anything, disable the Canvas component
            if (canvas != null)
            {
                canvas.enabled = false; // Disable the Canvas component
            }
        }
    }
}
