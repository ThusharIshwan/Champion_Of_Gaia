using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Vector2 MouseChange;
    private Vector2 MouseDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseChange = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        MouseDirection += MouseChange;
        
        this.gameObject.GetComponent<Transform>().localRotation = Quaternion.AngleAxis(-MouseDirection.y, Vector3.right);
    }
}
