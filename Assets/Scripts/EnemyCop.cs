using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCop : Character {
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

    public LayerMask collisionLayer;

    // Use this for initialization
    protected override void Start () {
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
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (CanSeeTarget(out targetDirection))
        {
            velocity = new Vector2(targetDirection.x, targetDirection.y) * accelerationToTarget;
            base.Move(velocity, maxSpeedToTarget);
        }
        else if (hasSeenPlayer)
        {
            if (breaking || (targetLastSeen - transform.position).magnitude < 0.5f)
            {
                hasSeenPlayer = false;
                breaking = true;
                breaking = base.Breaking(breakingDrag, maxSpeed);
            }
            else
            {
                Vector2 lastSeenHeading = targetLastSeen - transform.position;
                velocity = lastSeenHeading.normalized * accelerationToTarget;
                base.Move(velocity, maxSpeedToTarget);
            }
        }
        else
        {
            base.KeepMoving(maxSpeed);
        }
        base.SetRotation();
    }

    private bool CanSeeTarget(out Vector2 targetDirection)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, collisionLayer);
        GetComponent<BoxCollider2D>().enabled = true;
        if (hit.collider != null && hit.collider.tag == "Player")
        {
            Vector2 targetHeading = player.transform.position - transform.position;
            float targetDistance = targetHeading.magnitude;
            if (targetDistance > 50)
            {
                targetDirection = new Vector2(0, 0);
                return false;
            }
            targetDirection = targetHeading / targetDistance;

            targetLastSeen = player.transform.position;
            hasSeenPlayer = true;
            breaking = false;
            return true;
        }
        targetDirection = new Vector2(0, 0);
        return false;
    }
}
