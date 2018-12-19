using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    private float maxSpeed;
    private float acceleration;
    private Vector2 velocity;
    private bool queuedMove;

    // Use this for initialization
    protected override void Start () {
        maxSpeed = 2f;
        acceleration = 0.2f;
        queuedMove = false;
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if (!queuedMove)
        {
            float horizontal = 0;
            float vertical = 0;

            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            velocity = new Vector2(horizontal, vertical) * acceleration;
            queuedMove = true;
        }
	}

    void FixedUpdate()
    {
        if (queuedMove)
        {
            base.Move(velocity, maxSpeed);
            queuedMove = false;
        }

       
    }
}
