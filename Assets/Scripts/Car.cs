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
            photonView.RPC("RPC_Move", RpcTarget.Others, transform.position, transform.rotation, speed);
        }
        
    }

    [PunRPC]
    void RPC_Move(Vector3 position, Quaternion rotation, float speed)
    {
        transform.position = position;
        transform.rotation = rotation;
        this.speed = speed;
    }

   public void turn(int direction)
{
    turnDirection = direction;
    if(photonView.IsMine)
    {
        photonView.RPC("RPC_UpdateTurnDirection", RpcTarget.Others, direction);
    }
}

[PunRPC]
void RPC_UpdateTurnDirection(int direction)
{
    turnDirection = direction;
}

void FixedUpdate()
{
    if(photonView.IsMine)
    {
        speed += increaseSpeed * Time.fixedDeltaTime;
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);
        transform.Rotate(0f, turnDirection * Time.fixedDeltaTime, 0f);
        photonView.RPC("RPC_Move", RpcTarget.Others, transform.position, transform.rotation, speed);
    }
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
