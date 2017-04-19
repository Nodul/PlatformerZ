using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {

    public float moveSpeed;
    private float currentMoveSpeed;
    public float diagonalMoveModifier; //go with 1/sqrt(2) = 0.71f

    private Animator anim;
    private Rigidbody2D rigid2D;

    private bool playerMoving;
    public Vector2 lastMove;


    private float axisRawHorizontal;
    private float axisRawVertical;
    private float controlThreshold = 0.5f; //how much do the axis have to move, before we accept it as a move

    private static bool playerExists;
    private bool isAttacking;
    public float attackTime;
    private float attackTimeCounter;

    public string startPoint;

    public bool canMove;

    private void Start()
    {
        canMove = true;
        anim = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();

        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        playerMoving = false;

        if (!canMove)
        {
            rigid2D.velocity = Vector2.zero;
            return;
        }


        axisRawHorizontal = Input.GetAxisRaw("Horizontal");
        axisRawVertical = Input.GetAxisRaw("Vertical");

        if (!isAttacking)
        {
            if (axisRawHorizontal > controlThreshold || axisRawHorizontal < -controlThreshold)
            {
                //transform.Translate(new Vector3(axisRawHorizontal * moveSpeed * Time.deltaTime,0f,0f)); //Old move code without using Rigidbodies
                rigid2D.velocity = new Vector2(axisRawHorizontal * currentMoveSpeed, rigid2D.velocity.y);
                playerMoving = true;
                lastMove = new Vector2(axisRawHorizontal, 0f);
            }
            if (axisRawVertical > controlThreshold || axisRawVertical < -controlThreshold)
            {
                //transform.Translate(new Vector3(0, axisRawVertical * moveSpeed * Time.deltaTime, 0f));
                rigid2D.velocity = new Vector2(rigid2D.velocity.x, axisRawVertical * currentMoveSpeed);
                playerMoving = true;
                lastMove = new Vector2(0f, axisRawVertical);
            }
            //Stop movement code
            if (axisRawHorizontal < controlThreshold && axisRawHorizontal > -controlThreshold) rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);
            if (axisRawVertical < controlThreshold && axisRawVertical > -controlThreshold) rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0f);

            if (Input.GetKeyDown(KeyCode.G))
            {
                attackTimeCounter = attackTime;
                isAttacking = true;
                rigid2D.velocity = Vector2.zero;
                anim.SetBool("isPlayerAttacking", true);
            }
            //Are we moving diagonally?
            if (Mathf.Abs(axisRawHorizontal) > controlThreshold && Mathf.Abs(axisRawVertical) > controlThreshold)
            {
                currentMoveSpeed = moveSpeed * diagonalMoveModifier;
            }
            else
            {
                currentMoveSpeed = moveSpeed;
            }

        }

        if (attackTimeCounter > 0)
        {
            attackTimeCounter -= Time.deltaTime;
        }
        else if (attackTimeCounter <= 0)
        {
            isAttacking = false;
            anim.SetBool("isPlayerAttacking", false);
        }

        anim.SetFloat("MoveX", axisRawHorizontal);
        anim.SetFloat("MoveY", axisRawVertical);
        anim.SetBool("isPlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }
}
