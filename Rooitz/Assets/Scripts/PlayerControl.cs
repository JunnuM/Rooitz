using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public enum PlayerState
    {
        Walking,
        Standing,
        CutScene,
        Interacting
    }

    // Object public variables
    public PlayerState activeState;
    public double minimumTilt = 0.1;
    public float WalkingSpeed = 2;

    // This is used as a reference to see where character moves when controlled
    public Vector2 GroundNormal;

    public Animation walkingAnimation;

    // Object private variables/references
    private GameObject playerObject;
    private Animator playerAnimator;
    private Transform animatorTransform;
    private Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
        activeState = PlayerState.Standing;
        playerObject = gameObject;
        playerBody = (Rigidbody2D)playerObject.GetComponent("Rigidbody2D");
        animatorTransform = playerObject.transform.GetChild(0);
        playerAnimator = (Animator)playerObject.GetComponentInChildren(typeof(Animator));
        // This might end up unneeded.
        GroundNormal = new Vector2(1, 0);
    }

    float GetAngle(Vector2 v1, Vector2 v2) {
        return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * (180/Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {
        ContactPoint2D[] contactList = new ContactPoint2D[4] { new ContactPoint2D(), new ContactPoint2D(), new ContactPoint2D(), new ContactPoint2D() };
        int numberOfContacts = playerBody.GetContacts(contactList);
        foreach (ContactPoint2D contact in contactList)
        {
            //Debug.DrawRay(contact.point, contact.normal, Color.white);
            //Debug.Log("Touching point: " + contact.point);
            if(contact.point.x != 0 && contact.point.y != 0) {
                Vector2 collisionTangent = new Vector2(contact.normal.y, -contact.normal.x);
                Vector2 playerAngle = new Vector2(playerObject.transform.eulerAngles.x, playerObject.transform.eulerAngles.y);
                float angle = GetAngle(playerAngle, collisionTangent);
                playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, Quaternion.Euler(0,0,angle), Time.deltaTime);
            }
        }
        // First let get relevant inputs
        // I think we mostly need horizontal inputs AKA left to right
        if (activeState == PlayerState.Standing || activeState == PlayerState.Walking)
        {
            float horizontal = Input.GetAxis("Horizontal");
            // Moves to the right
            if (horizontal >= minimumTilt)
            {
                activeState = PlayerState.Walking;
                RotateCharacter(true);
                playerAnimator.enabled = true;
                playerAnimator.Play("Walking");
                playerBody.velocity = Vector2.Scale(GroundNormal, new Vector2(WalkingSpeed, WalkingSpeed));
            }
            // Might be better withouth this left walking section
            /*
            else if (horizontal <= -minimumTilt)
            {
                activeState = PlayerState.Walking;
                RotateCharacter(false);
                playerAnimator.enabled = true;
                playerAnimator.Play("Walking");
                playerBody.velocity = Vector2.Scale(GroundNormal, new Vector2(-WalkingSpeed, WalkingSpeed));
            }*/
            else
            {
                playerAnimator.enabled = false;
                activeState = PlayerState.Standing;
                playerBody.velocity = new Vector2(0, 0);
            }
        }
    }
    void RotateCharacter(bool isFacingRight)
    {
        if (isFacingRight)
        {
            animatorTransform.localPosition = new Vector3(0.75f, 0, 0);
            animatorTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            animatorTransform.localPosition = new Vector3(0.75f, 0, 0);
            animatorTransform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
