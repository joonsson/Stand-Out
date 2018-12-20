using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Character
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
        if (canMove && CanSeeTarget(out targetDirection))
        {
            velocity = new Vector2(targetDirection.x, targetDirection.y) * accelerationToTarget;
            base.Move(velocity, maxSpeedToTarget);
        }
        else if (canMove && hasSeenPlayer)
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
        else if (canMove)
        {
            base.KeepMoving(maxSpeed);
        }
        base.SetRotation();
    }

    private bool CanSeeTarget(out Vector2 targetDirection)
    {
        bool didHit = false;
        Vector3 targetPosition = new Vector3(100000, 100000, 100000);
        targetDirection = new Vector2(0, 0);

        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, collisionLayer);

        if (hit.collider != null && hit.collider.tag == "Player")
        {
            targetPosition = player.transform.position;
            didHit = true;
        }

        if (gameManager.enemies.Count > 0)
        {
            for (int i = 0; i < gameManager.allies.Count; i++)
            {
                var newTarget = gameManager.enemies[i].gameObject.transform.position;
                hit = Physics2D.Raycast(transform.position, newTarget - transform.position, collisionLayer);
                if (hit.collider != null && hit.collider.tag == "Enemy" && ((newTarget - transform.position).magnitude < 5))
                {
                    targetPosition = newTarget;
                    didHit = true;
                }
            }
        }
        Vector2 targetHeading = targetPosition - transform.position;
        float targetDistance = targetHeading.magnitude;
        if (targetDistance > 10)
        {
            targetDirection = new Vector2(0, 0);
            didHit = false;
        }
        if (didHit)
        {
            targetDirection = targetHeading / targetDistance;

            targetLastSeen = player.transform.position;
            hasSeenPlayer = true;
            breaking = false;
        }
        GetComponent<BoxCollider2D>().enabled = true;
        return didHit;
    }

    public override void setCanMove(bool canIMove)
    {
        base.setCanMove(canIMove);
    }
}
