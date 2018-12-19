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
#if UNITY_IOS || UNITY_ANDROID
            Touch lastTouch;
            if (Input.touches.Length > 0)
            {
                lastTouch = Input.touches[Input.touches.Length - 1];
                Vector2 position = transform.position;
                Vector2 direction = lastTouch.position - position;
                velocity = direction.normalized * acceleration;
                queuedMove = true;
            }
#else
            float horizontal = 0;
            float vertical = 0;

            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            velocity = new Vector2(horizontal, vertical).normalized * acceleration;
            queuedMove = true;
#endif
        }
    }

    void FixedUpdate()
    {
        if (queuedMove)
        {
            base.Move(velocity, maxSpeed);
            queuedMove = false;
        }
        base.SetRotation();
    }
}
