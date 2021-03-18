using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public GameControls game;
    public Transform puck;
    public Transform player;
    public Transform ai;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isEnd = false;
        Vector3 puckSpawnPosition = transform.position.y < 0 ? new Vector3(2.1f, -2f, 0) : new Vector3(2.1f, 3.2f, 0);

        if (transform.position.y < 0)
        {
            game.AddPlayer2Point();
            isEnd = game.CheckIsWinner();
        }
        else
        {
            game.AddPlayer1Point();
            isEnd = game.CheckIsWinner();
        }

        game.SetPlayersPoints();
        if (!isEnd)
        {
            puck.position = puckSpawnPosition;
            puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.position = new Vector3(2.1f, -3f, 0);
            ai.position = new Vector3(2.1f, 4.2f, 0);
            StartCoroutine(game.WaitBeforeStart());
        }
    }
}
