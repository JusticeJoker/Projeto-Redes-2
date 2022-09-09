using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        RandomFoodPosition();
    }

    private void RandomFoodPosition()
    {
        Bounds bounds = this.gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Snake")
        {
           
            RandomFoodPosition();
        }       
    }

    //if (PhotonNetwork.IsMasterClient)
    //{
    //    ScoreSystem.appleScore1 += 1;
    //}

    //if (!PhotonNetwork.IsMasterClient)
    //{
    //    ScoreSystem.appleScore2 += 1;
    //}

    //Scoring();

}
