using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Car : MonoBehaviourPun
{
    public float speed = 10f;
    public float increaseSpeed = 0.1f;
    private int turnDirection = 0;
    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        
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


    public void Turn(int direction)
    {
        if (photonView.IsMine)
        {
            // Update turn direction locally
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
