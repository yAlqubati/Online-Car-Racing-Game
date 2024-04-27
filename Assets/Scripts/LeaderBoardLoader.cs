using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardLoader : MonoBehaviour
{
    public void LoadLeaderBoard()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LeaderBoard");
    }
}
