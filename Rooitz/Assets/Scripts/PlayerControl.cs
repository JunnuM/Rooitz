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
        Interacting,
        StartUp
    }

    // Object public variables
    public PlayerState activeState;
    public double minimumTilt = 0.1;
    public float WalkingSpeed = 2;

    // This is used as a reference to see where character moves when controlled
    public Vector3 GroundNormal;

    // Object private variables/references
    private GameObject playerObject;
    public Animator playerAnimator;
    private Transform animatorTransform;
    private Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
        activeState = PlayerState.StartUp;
        playerObject = gameObject;
        playerBody = (Rigidbody2D)playerObject.GetComponent("Rigidbody2D");
        animatorTransform = playerObject.transform.GetChild(0);
        playerAnimator = (Animator)playerObject.GetComponentInChildren(typeof(Animator));
        playerAnimator.enabled = false;
        // This might end up unneeded.
        GroundNormal = new Vector3(1, 0, 0);
    }

    public void PlayWakeUp()
    {
        playerAnimator.enabled = true;
        activeState = PlayerState.CutScene;
        playerAnimator.Play("WakeUp");
    }

    bool AnimatorIsPlaying()
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(0).length >
               playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    float GetAngle(Vector2 v1, Vector2 v2)
    {
        return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * (180 / Mathf.PI);
    }


    bool wokenUp = false;
    // Update is called once per frame
    void Update()
    {
        if(!wokenUp)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                PlayWakeUp();
                wokenUp = true;
            }
        }

        if (activeState == PlayerState.CutScene && !AnimatorIsPlaying())
        {
            activeState = PlayerState.Standing;
        }
        ContactPoint2D[] contactList = new ContactPoint2D[4] { new ContactPoint2D(), new ContactPoint2D(), new ContactPoint2D(), new ContactPoint2D() };
        int numberOfContacts = playerBody.GetContacts(contactList);
        foreach (ContactPoint2D contact in contactList)
        {
            //Debug.DrawRay(contact.point, contact.normal, Color.white);
            //Debug.Log("Touching point: " + contact.point);
            if (contact.point.x != 0 && contact.point.y != 0)
            {    
                Vector2 collisionTangent = new Vector2(contact.normal.y, -contact.normal.x);
                GroundNormal = collisionTangent;
                Vector2 playerAngle = new Vector2(playerObject.transform.eulerAngles.x, playerObject.transform.eulerAngles.y);
                float angle = GetAngle(playerAngle, collisionTangent);
                playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime);
                break;
            }
        }
        // First let get relevant inputs
        // I think we mostly need horizontal inputs AKA left to right
        if (activeState == PlayerState.Standing || activeState == PlayerState.Walking)
        {
            float horizontal = 0; // Input.GetAxis("Horizontal");
            if(Input.GetKey(KeyCode.Space)) horizontal = 1f;
            // Moves to the right
            if (horizontal >= minimumTilt)
            {
                activeState = PlayerState.Walking;
                RotateCharacter(true);
                playerAnimator.enabled = true;
                playerAnimator.Play("Walking");
                playerObject.transform.position = Vector3.Lerp(playerObject.transform.position, playerObject.transform.position + GroundNormal, Time.deltaTime * WalkingSpeed);
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
