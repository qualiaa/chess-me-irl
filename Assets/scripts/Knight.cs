using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (LineRenderer))]
[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Collider2D))]
[RequireComponent (typeof (FrictionJoint2D))]
public class Knight : MonoBehaviour {

	Rigidbody2D body_;
	LineRenderer line_;
	SpriteRenderer sprite_;
	Collider2D collider_;
	FrictionJoint2D friction_;
	Vector3 lastMousePos_;
	Color color_;

	const float gravity = -1500f;
	const float height = 0.5f;
	const float maxForce = 3500f;
	const float mouseToForceFiddleFactor = 3500f;


	const float maxForceSqr = maxForce * maxForce;

	float zVel = 0f;
	float zPos = 0f;
	//float startFlight;

	void Start () {
		body_ = GetComponent<Rigidbody2D> ();
		line_ = GetComponent<LineRenderer> ();
		sprite_ = GetComponent<SpriteRenderer> ();
		collider_ = GetComponent<Collider2D> ();
		color_ = sprite_.color;
		friction_ = GetComponent<FrictionJoint2D> ();
	}
		
	void OnMouseDown() {
		line_.enabled = true;
	}
		
	void OnMouseDrag() {
		var force = getForce ();
		var v = force.z / body_.mass;
		force.z = 0f;

		var t = -2 * (v / gravity);

		var disp = (force * Time.fixedDeltaTime) * t  / body_.mass;
		line_.SetPositions (new Vector3[]{transform.position, transform.position + disp});
		line_.material.SetFloat ("_Length", disp.magnitude);
	}

	void OnMouseUp() {
		line_.enabled = false;

		var force = getForce ();

		//startFlight = Time.time;

		zVel += force.z / body_.mass;
		body_.AddForce (new Vector2(force.x, force.y));

		line_.SetPositions (new Vector3[]{Vector3.zero, Vector3.zero});
		line_.material.SetFloat ("_Length", 1);
	}

	void FixedUpdate()
	{
		var dt = Time.fixedDeltaTime;
		friction_.enabled = false;

		bool airbourne = false;
		if (zPos > 0) {
			airbourne = true;
		}
		zPos += zVel * dt / 2;
		zVel += gravity * dt / 2;

		if (zPos > height) {
			sprite_.color = Color.red;
			collider_.isTrigger = true;
		} else {
			sprite_.color = Color.yellow;
			collider_.isTrigger = false;
		}

		zPos += zVel * dt / 2;
		zVel += gravity * dt / 2;

		if (zPos < 0) {
			/*
			if (airbourne) {
				Debug.Log (Time.time - startFlight);
			}
			*/
			zPos = 0;
			zVel = 0;
			friction_.enabled = true;
			sprite_.color = color_;
		}
	}

	private Vector3 getForce()
	{
		var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0.0f;
		var delta = mousePos - transform.position;
		var force = -delta * mouseToForceFiddleFactor;

		float magnitudeSqr = Math.Min(force.sqrMagnitude, maxForceSqr);
		float magnitude = (float)Math.Sqrt (magnitudeSqr);

		force.x = force.normalized.x * magnitude;
		force.y = force.normalized.y * magnitude;
		force.z = (float)Math.Sqrt(maxForceSqr - magnitudeSqr);

		return force;
	}
}
