using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Car : MonoBehaviourPun, IPunObservable
{
    public float speed = 10f;
    public float increaseSpeed = 0.1f;
    private int turnDirection = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false; // Disable script if it's not owned by the local player
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            HandleInput();
            Move();
        }
        else
        {
            SmoothMove();
        }
    }

    // Handle input for turning
    private void HandleInput()
    {
        turnDirection = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
    }

    // Move the car locally
    private void Move()
    {
        speed += increaseSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        transform.Rotate(0f, turnDirection * Time.deltaTime, 0f);
    }

    // Smoothly move the car for remote players
    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10);
        transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10);
    }

    // Synchronize position, rotation, and speed across the network
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(speed);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            speed = (float)stream.ReceiveNext();
        }
    }

    // Network position and rotation variables for remote players
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    // On trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Debug.Log("Car hit an obstacle");
        }
    }
}
