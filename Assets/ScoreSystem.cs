using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ScoreSystem : MonoBehaviour
{
    public GameObject scoreP1;
    public GameObject scoreP2;
    public static int appleScore1;
    public static int appleScore2;

    void Update()
    {
        Scoring();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu();
        }
    }

    public void Scoring()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            scoreP1.GetComponent<Text>().text = "PLAYER 1\nSCORE: " + appleScore1;
        }
        
        if(!PhotonNetwork.IsMasterClient)
        {
            scoreP2.GetComponent<Text>().text = "PLAYER 2\nSCORE: " + appleScore2;
        }

        WinnerWinnerChickenDinner();
    }

    public void WinnerWinnerChickenDinner()
    {
        if (appleScore1 > appleScore2 && appleScore1 >= 20)
        {
            //player 1 wins
            if (PhotonNetwork.IsMasterClient)
            {
                SceneManager.LoadScene(3); //win
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                SceneManager.LoadScene(4); //Lose
            }        
        }
        else if (appleScore2 > appleScore1 && appleScore2 >= 20)
        {
            //player 2 wins
            if (PhotonNetwork.IsMasterClient)
            {
                SceneManager.LoadScene(4);
            }            
            else if (!PhotonNetwork.IsMasterClient)
            {
                SceneManager.LoadScene(3);
            }
        }
        else if (appleScore1 >= 20 && appleScore2 >= 20)
        {
            //draw
            if (PhotonNetwork.IsMasterClient && !PhotonNetwork.IsMasterClient)
            {
                SceneManager.LoadScene(5);
            }       
        }
    }

    public void MainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
