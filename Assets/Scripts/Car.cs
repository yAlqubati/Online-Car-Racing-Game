using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Car : MonoBehaviourPun
{
    public float speed = 10f;
    public float increaseSpeed = 0.1f;
    private int turnDirection = 0;
    private bool isLocalPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            isLocalPlayer = true;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
            Move();
        }
    }

    // Handle input for turning
    private void HandleInput()
    {
        turnDirection = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
    }

    // Move the car
    private void Move()
    {
        speed += increaseSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        transform.Rotate(0f, turnDirection * Time.deltaTime, 0f);
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_Move", RpcTarget.Others, transform.position, transform.rotation, speed);
        }
    }

    // Synchronize position, rotation, and speed across the network
    [PunRPC]
    void RPC_Move(Vector3 position, Quaternion rotation, float speed)
    {
        transform.position = position;
        transform.rotation = rotation;
        this.speed = speed;
    }

    // Handle turning
    public void Turn(int direction)
    {
        turnDirection = direction;
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_UpdateTurnDirection", RpcTarget.Others, direction);
        }
    }

    // Update turn direction for remote players
    [PunRPC]
    void RPC_UpdateTurnDirection(int direction)
    {
        Debug.Log("Updating turn direction");
        turnDirection = direction;
    }

    // On trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Debug.Log("Car hit an obstacle");
        }
    }
}
