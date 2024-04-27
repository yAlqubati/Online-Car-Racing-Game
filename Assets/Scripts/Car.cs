using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Car : MonoBehaviour
{
    //testing
    public float speed = 5f;
    public float increaseSpeed = 0.0f;
    public int turnDirection;
    public PhotonView photonView;
    // text for timer
    public TMP_Text timerText;
    public float timeLeft = 20f;
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
            timeLeft -= Time.deltaTime;
            timerText.text = timeLeft.ToString("0");
            if(timeLeft < 0)
            {
                timeLeft = 0;
            }
            
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
    if (other.gameObject.CompareTag("PowerUp")) // Check if the collider is tagged as "PowerUp"
    {
        speed += 3f; // Increase speed by 3 units
        Destroy(other.gameObject); // Destroy the power-up object
        Debug.Log("Power up collected! Speed increased.");
    }
    else if (other.gameObject.CompareTag("BigShip")) // Check if the collider is tagged as "BigShip"
    {
        if(speed > 4)
        {
            speed -= 4f; // Reduce speed by 4 units
            Debug.Log("Car hit a BigShip");
            
        }
        Destroy(other.gameObject); // Destroy the BigShip object
    }
    else if (other.gameObject.CompareTag("SmallShip")) // Check if the collider is tagged as "SmallShip"
    {
        if(speed > 4)
        {
            speed -= 2f; // Reduce speed by 2 units
            Debug.Log("Car hit a SmallShip");
            
        }
        Destroy(other.gameObject); // Destroy the SmallShip object
    }

    else if(other.gameObject.CompareTag("Ship"))
    {
        // load next scene
        PhotonNetwork.LoadLevel("LeaderBoard");
    }
}

}
