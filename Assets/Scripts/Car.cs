using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Car : MonoBehaviourPun
{
    public float speed = 10f;
    public float increaseSpeed = 0.1f;
    private int turnDirection = 0;
    public photonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            // Enable input control only for the local player
            // You might need to adjust this based on your input system
            EnableInputControl();
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            speed += increaseSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.Rotate(0f, turnDirection * Time.deltaTime, 0f);
        }
    }

    private void EnableInputControl()
    {
        // Enable input control for the local player
        // You might need to replace this with your input handling logic
        // For example, if you are using Unity's Input system:
        // Input.GetAxis("Horizontal") returns -1 for left and 1 for right
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput < 0)
        {
            Turn(-100);
        }
        else if (horizontalInput > 0)
        {
            Turn(100);
        }
        else
        {
            Turn(0);
        }
    }

    public void Turn(int direction)
    {
        if (photonView.IsMine)
        {
            // Update turn direction locally
            turnDirection = direction;
            // Synchronize turn direction across the network
            photonView.RPC("RPC_SetTurnDirection", RpcTarget.Others, direction);
        }
    }

    [PunRPC]
    private void RPC_SetTurnDirection(int direction)
    {
        if (!photonView.IsMine)
        {
            // Update turn direction for remote players
            turnDirection = direction;
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") && photonView.IsMine)
        {
            // Handle obstacle collision only for the local player
            Debug.Log("Car hit an obstacle");
        }
    }
}
