using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Character
{
    private float maxSpeed;
    private float maxSpeedToTarget;
    private float acceleration;
    private float accelerationToTarget;
    private float breakingDrag;
    private bool hasSeenPlayer;
    private bool breaking;
    private Vector2 velocity;
    private GameObject player;
    private Vector2 targetDirection;
    private Vector3 targetLastSeen;
    private GameManager gameManager;

    public LayerMask collisionLayer;

    // Use this for initialization
    protected override void Start()
    {
        gameManager = GameManager.instance;

        breaking = false;
        maxSpeed = 0.5f;
        maxSpeedToTarget = 1.5f;
        acceleration = 0.05f;
        accelerationToTarget = 0.2f;
        breakingDrag = 0.5f;
        player = GameObject.FindGameObjectWithTag("Player");
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        var direction = new Vector2(x, y) * acceleration;
        base.Start();
        base.Move(direction, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        
        base.KeepMoving(maxSpeed);
        
        base.SetRotation();
    }
    public override void setCanMove(bool canIMove)
    {
        base.setCanMove(canIMove);
    }
}
