using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private Transform playerTransform;

    //Mask denoting the layer with all unpassable objects.
    public LayerMask ground;

    //A speed modifier designed to stop the player from running into walls and of cliffs.
    private float speedPercent;

    //distance the player starts to detect impassables in front of them.
    public float smartDistance;

    //from the inspector, to make the game run in a balanced fasion.
    public float speed;
    public float playerHeight;

    //forward: the direction pressing w will take you, modified for the terrain.
    private Vector3 forward;
    //nexto: the direction pressing d will take you, modified for the terrain.
    private Vector3 nexto;
    //A unit vector or 0, making sure that the speed of travel does not exceed 1 (no 1.41 speed glitch)
    private Vector2 normalizer;

    //Some vectors to represent mouse inputs.
    private Vector2 MouseChange;
    private Vector2 MouseDirection;

    void Start()
    {
        playerTransform = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHorizontalCameraDirection();
        CalculateForward();
        AdjustSpeedForObstacles();
        Normalize();
        CompleteMovement();
    }

    void UpdateHorizontalCameraDirection() {
        MouseChange = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        MouseDirection += MouseChange;

        this.gameObject.GetComponent<Transform>().localRotation = Quaternion.AngleAxis(MouseDirection.x, Vector3.up);
    }

    private void CalculateForward()
    {

        Physics.Raycast(playerTransform.position, -Vector3.up, out RaycastHit hitInfo, 3f, ground);
        forward = (-Vector3.Cross(hitInfo.normal, playerTransform.right));
        nexto =   (Vector3.Cross(hitInfo.normal, playerTransform.forward));

    }

    private void AdjustSpeedForObstacles() {
        Vector3 dirCombo = ((Input.GetAxis("Horizontal") * playerTransform.right) + (Input.GetAxis("Vertical") * playerTransform.forward));
        Vector3 playerFeet = new Vector3 (playerTransform.position.x, playerTransform.position.y - ((5* playerHeight)/6), playerTransform.position.z);


        if (Physics.Raycast(playerFeet, dirCombo, out RaycastHit blindCane, smartDistance, ground))
        {
            speedPercent = (Vector3.Distance(blindCane.point, playerFeet) - (smartDistance / 3)) / (2 * smartDistance / 3);
        }
        else
        {
            speedPercent = 1;
        }
    }

    private void Normalize() {
        normalizer = (new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")));

        if (normalizer != new Vector2(0, 0))
        {
            normalizer /= Mathf.Sqrt(Vector2.Dot(normalizer, normalizer));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            normalizer.x *= 2;
            normalizer.y *= 2;
        }
    }

    private void CompleteMovement() {
        playerTransform.position += (speed * speedPercent * forward * normalizer.x);
        playerTransform.position += (speed * speedPercent * nexto * normalizer.y);

        if (Physics.Raycast(playerTransform.position, -Vector3.up, playerHeight, ground))
        {
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.03f, playerTransform.position.z);
        }
        else if (!(Physics.Raycast(playerTransform.position, -Vector3.up, playerHeight + 0.06f, ground)))
        {
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - 0.03f, playerTransform.position.z);
        }
    }


}
