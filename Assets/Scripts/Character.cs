using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {
    private Rigidbody2D rb;
    private float baseDrag;


	// Use this for initialization
	protected virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        baseDrag = 0.1f;
        rb.drag = baseDrag;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void Move(Vector2 vector, float maxSpeed)
    {
        rb.AddForce(vector, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    protected virtual bool Breaking(float drag, float maxSpeed)
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.drag = baseDrag;
            return false;
        }
        else if (rb.drag == drag)
        {
            return true;
        }
        else
        {
            rb.drag = drag;
            return true;
        }
    }

    protected virtual void KeepMoving(float maxSpeed)
    {
        rb.AddForce(rb.velocity, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }
}
