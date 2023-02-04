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

    // Update is called once per frame
    void Update()
    {
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
