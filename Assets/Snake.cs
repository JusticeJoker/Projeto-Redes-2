using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;

    //[SerializeField] private float speedModifier = 1;

    private List<Transform> _tail = new List<Transform>();

    public GameObject tailPrefab;

    public int initializeSize = 4;

    PhotonView view;

    private void Start()
    {
        ResetState();
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _direction = Vector2.down;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _direction = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _direction = Vector2.right;
            }

            //_direction.x = _direction.x / speedModifier;
            //_direction.y = _direction.y / speedModifier;

            //Debug.Log(_direction);

        }
    }

    private void FixedUpdate()
    {
        for (int i = _tail.Count - 1; i > 0; i--)
        {
            _tail[i].position = _tail[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x + _direction.x),
            Mathf.Round(this.transform.position.y + _direction.y), 0.0f);
    }

    private void Grow()
    {
        GameObject tail = Instantiate(tailPrefab);
        tail.transform.position = _tail[_tail.Count - 1].position;

        _tail.Add(tail.transform);
    }

    private void ResetState()
    {
        for (int i = 1; i < _tail.Count; i++)
        {
            Destroy(_tail[i].gameObject);
        }

        _tail.Clear();
        _tail.Add(this.transform);

        for (int i = 1; i < this.initializeSize; i++)
        {
            Grow();
        }

        this.transform.position = Vector3.zero;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Food" && view.IsMine)
            {
                Grow();

                if (PhotonNetwork.IsMasterClient)
                {
                    ScoreSystem.appleScore1 += 1;
                    view.RPC("RPCUpdateScore", RpcTarget.Others, ScoreSystem.appleScore1, true);
                }

                if (!PhotonNetwork.IsMasterClient)
                {
                    ScoreSystem.appleScore2 += 2;
                    view.RPC("RPCUpdateScore", RpcTarget.MasterClient, ScoreSystem.appleScore2, true);
                }
            }
            else if (collision.tag == "Obstacle")
            {

                if (PhotonNetwork.IsMasterClient)
                {
                    ScoreSystem.appleScore1 = 0;
                    ResetState();
                    view.RPC("RPCUpdateScore", RpcTarget.Others, ScoreSystem.appleScore1, false);
                }
                else if (!PhotonNetwork.IsMasterClient)
                {
                    ScoreSystem.appleScore2 = 0;
                    ResetState();
                    view.RPC("RPCUpdateScore", RpcTarget.MasterClient, ScoreSystem.appleScore2, false);
                }
            }   
    }

    [PunRPC]
    public void RPCUpdateScore(int score, bool isGrow)
    {
        if (PhotonNetwork.IsMasterClient && !view.IsMine)
        {
            ScoreSystem.appleScore2 = score;
            if(isGrow)
            {
                Grow();
            }            
        }

        if (!PhotonNetwork.IsMasterClient && !view.IsMine)
        {
            ScoreSystem.appleScore1 = score;
            if (isGrow)
            {
                Grow();
            }
        }
    }

}
