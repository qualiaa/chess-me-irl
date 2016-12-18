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

	const float gravity = -1f;
	const float height = 50f;

	const float maxForce = 80f;
	const float maxForceSqr = maxForce * maxForce;

	const float mouseToForceFiddleFactor = 75f;

	float zVel = 0f;
	float zPos = 0f;

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
		force.z = 0f;
		line_.SetPositions (new Vector3[]{transform.position, transform.position + force / 10});
	}

	void OnMouseUp() {
		line_.enabled = false;

		var force = getForce ();
		var mass = body_.mass;

		zVel += force.z / mass;
		body_.AddForce (new Vector2(force.x, force.y));

		line_.SetPositions (new Vector3[]{Vector3.zero, Vector3.zero});
	}

	void Update()
	{
		friction_.enabled = false;

		zPos += zVel / 2;
		zVel += gravity / 2;

		if (zPos > height) {
			sprite_.color = Color.red;
			collider_.isTrigger = true;
		} else {
			sprite_.color = Color.yellow;
			collider_.isTrigger = false;
		}

		zPos += zVel / 2;
		zVel += gravity / 2;

		if (zPos < 0) {
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

		Debug.Log (force);

		force.z = (float)Math.Sqrt(maxForceSqr - magnitudeSqr);

		return force;
	}
}
