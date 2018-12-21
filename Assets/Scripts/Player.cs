using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    private float maxSpeed;
    private float acceleration;
    private Vector2 velocity;
    private bool queuedMove;
    private Ally ally;
    private Transform lightPurple;
    private Transform lightPink;
    private int timer;
    private bool startUp;
    private bool purple;

    // Use this for initialization
    protected override void Start () {
        maxSpeed = 2f;
        acceleration = 0.2f;
        queuedMove = false;
        lightPurple = transform.Find("PlayerLight1");
        lightPink = transform.Find("PlayerLight2");
        lightPurple.gameObject.SetActive(false);
        lightPink.gameObject.SetActive(false);
        timer = 0;
        startUp = true;
        purple = false;
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if (!queuedMove && canMove)
        {
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
            float horizontal = 0;
            float vertical = 0;

            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            velocity = new Vector2(horizontal, vertical).normalized * acceleration;
            queuedMove = true;
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            Touch lastTouch;
            if (Input.touches.Length > 0)
            {
                lastTouch = Input.touches[Input.touches.Length - 1];
                Vector2 position = transform.position;
                Vector2 direction = lastTouch.position - position;
                velocity = direction.normalized * acceleration;
                queuedMove = true;
            }
        #endif
        }
    }

    void FixedUpdate()
    {
        if (startUp && timer > 60)
        {
            lightPurple.gameObject.SetActive(true);
            startUp = false;
            purple = true;
            timer = 0;
        }
        else if (timer % 10 == 0)
        {
            if (purple)
            {
                lightPurple.gameObject.SetActive(false);
                lightPink.gameObject.SetActive(true);
                purple = false;
                timer = 0;
            }
            else
            {
                lightPurple.gameObject.SetActive(true);
                lightPink.gameObject.SetActive(false);
                purple = true;
                timer = 0;
            }
        }
        else
        {
            timer++;
        }

        if (queuedMove && canMove)
        {
            base.Move(velocity, maxSpeed);
            queuedMove = false;
        }
        //base.SetRotation();
    }
    public override void setCanMove(bool canIMove)
    {
        base.setCanMove(canIMove);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Civilian")
        {
            Civilian civilian = collision.gameObject.GetComponent<Civilian>();
            if (civilian != null)
            {
                civilian.setCanMove(false);
                Instantiate(ally, civilian.transform.position, Quaternion.identity);
                civilian.gameObject.SetActive(false);
            }
        }
    }
}
