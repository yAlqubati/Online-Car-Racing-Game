using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Car : MonoBehaviour
{
    public float speed = 10f;
    public float increaseSpeed = 0.1f;
    public int turnDirection;
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {  
            speed += increaseSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.Rotate(0f,turnDirection * Time.deltaTime,0f);
        }
        
    }

    public void turn(int direction)
    {
        //Debug.Log("Turning to " + direction + " direction.");
        turnDirection = direction;
    }

    // on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            Debug.Log("Car hit an obstacle");
        }
    }
}
