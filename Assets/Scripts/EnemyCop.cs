using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCop : Character {
    private float maxSpeed;
    private float maxSpeedToPlayer;
    private float acceleration;
    private float accelerationToPlayer;
    private float breakingDrag;
    private bool hasSeenPlayer;
    private bool breaking;
    private Vector2 velocity;
    private GameObject player;
    private Vector2 playerDirection;
    private Vector3 playerLastSeen;

    public LayerMask collisionLayer;

    // Use this for initialization
    protected override void Start () {
        breaking = false;
        maxSpeed = 0.5f;
        maxSpeedToPlayer = 1.5f;
        acceleration = 0.05f;
        accelerationToPlayer = 0.2f;
        breakingDrag = 0.5f;
        player = GameObject.FindGameObjectWithTag("Player");
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        var direction = new Vector2(x, y) * acceleration;
        base.Start();
        base.Move(direction, maxSpeed);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (CanSeePlayer(out playerDirection))
        {
            velocity = new Vector2(playerDirection.x, playerDirection.y) * accelerationToPlayer;
            base.Move(velocity, maxSpeedToPlayer);
        }
        else if (hasSeenPlayer)
        {
            if (breaking || (playerLastSeen - transform.position).magnitude < 0.5f)
            {
                hasSeenPlayer = false;
                breaking = true;
                breaking = base.Breaking(breakingDrag, maxSpeed);
            }
            else
            {
                Vector2 lastSeenHeading = playerLastSeen - transform.position;
                float lastSeenDistance = lastSeenHeading.magnitude;
                Vector2 lastSeenDirection = lastSeenHeading / lastSeenDistance;
                velocity = new Vector2(lastSeenDirection.x, lastSeenDirection.y) * accelerationToPlayer;
                base.Move(velocity, maxSpeedToPlayer);
            }
        }
        else
        {
            base.KeepMoving(maxSpeed);
        }
    }

    private bool CanSeePlayer(out Vector2 playerDirection)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, collisionLayer);
        GetComponent<BoxCollider2D>().enabled = true;
        if (hit.collider != null && hit.collider.tag == "Player")
        {
            Vector2 playerHeading = player.transform.position - transform.position;
            float playerDistance = playerHeading.magnitude;
            playerDirection = playerHeading / playerDistance;

            playerLastSeen = player.transform.position;
            hasSeenPlayer = true;
            breaking = false;
            return true;
        }
        playerDirection = new Vector2(0, 0);
        return false;
    }
}
