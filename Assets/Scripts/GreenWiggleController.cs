using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenWiggleController : MonoBehaviour
{

    public Transform leftPoint;
    public Transform rightPoint;

    public float moveSpeed;

    private Rigidbody2D myRigidbody;

    public bool movingRight;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (movingRight && transform.position.x > rightPoint.position.x)
            {
                movingRight = false;
            }
            if (!movingRight && transform.position.x < leftPoint.position.x)
            {
                movingRight = true;
            }

            if (movingRight)
            {
                myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);
            }
            else
            {
                myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);
            }
        }
    }
    void OnBecameVisible()
    {
        canMove = true;
    }

    private void OnEnable()
    {
        canMove = false;
    }
}
