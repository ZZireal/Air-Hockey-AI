using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    bool wasJustClicked;
    bool canMove;
    Vector2 playerSize;
    Rigidbody2D rb;
    public Transform PlayerBoundaries1;
    Boundary playerBoundaries1;

    private bool isPlay = false;

    public void SetPlayMode(bool play)
    {
        isPlay = play;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSize = GetComponent<SpriteRenderer>().bounds.extents;
        rb = GetComponent<Rigidbody2D>();

        playerBoundaries1 = new Boundary(PlayerBoundaries1.GetChild(0).position.y,
            PlayerBoundaries1.GetChild(1).position.y,
            PlayerBoundaries1.GetChild(2).position.x,
            PlayerBoundaries1.GetChild(3).position.x);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlay) MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (wasJustClicked)
            {
                wasJustClicked = false;

                if ((mousePosition.x >= transform.position.x && mousePosition.x < transform.position.x + playerSize.x ||
                    mousePosition.x <= transform.position.x && mousePosition.x > transform.position.x - playerSize.x) &&
                    (mousePosition.y > -transform.position.y && mousePosition.y < transform.position.y + playerSize.y ||
                    mousePosition.y <= transform.position.y && mousePosition.y > transform.position.y - playerSize.y))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                Vector2 clampedMousePos = new Vector2(Mathf.Clamp(mousePosition.x, playerBoundaries1.Left, playerBoundaries1.Right),
                                                        Mathf.Clamp(mousePosition.y, playerBoundaries1.Down, playerBoundaries1.Up));
                rb.MovePosition(clampedMousePos);
            }
        }
        else
        {
            wasJustClicked = true;
        }
    } 
}
