using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTrigger : MonoBehaviour
{
    [SerializeField] private TriggerBehavior triggerBehavior;
    [Header("Parameters for trigger behavior")]
    [SerializeField] CamSettings camSettings;
    [SerializeField] GameObject startCanvas, endCanvas;
    [SerializeField] Animator endAnimator;
    [SerializeField] PlayerControl player;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] float newGravity;

    bool triggered = false;

    public enum TriggerBehavior
    {
        PauseMusic,
        ResumeMusic,
        PlayMusic0,
        PlayMusic1,
        PlayWindSound,
        StartWindNoise,
        ChangeCam,
        StartEndAnimation,
        RemoveStartCanvas,
        SetPlayerGravityScale,
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(triggered) return;
        triggered = true;

        switch(triggerBehavior)
        {
            case TriggerBehavior.PauseMusic:
                AudioController.ToggleMusic(false);
                break;
            case TriggerBehavior.ResumeMusic:
                AudioController.ToggleMusic(true);
                break;
            case TriggerBehavior.PlayMusic0:
                AudioController.PlayMusicByID(0);
                AudioController.ToggleMusic(true);
                break;
            case TriggerBehavior.PlayMusic1:
                AudioController.PlayMusicByID(1);
                AudioController.ToggleMusic(true);
                break;
            case TriggerBehavior.PlayWindSound:
                AudioController.PlayRandomWindSound();
                break;
            case TriggerBehavior.StartWindNoise:
                AudioController.StartWindSoundLoop(7f, 12f, 30f);
                break;
            case TriggerBehavior.ChangeCam:
                CameraController.SetCamSize(camSettings.camSize, camSettings.useSmoothing);
                CameraController.SetXOffset(camSettings.xOffset, camSettings.useSmoothing);
                CameraController.SetYOffset(camSettings.yOffset, camSettings.useSmoothing);
                break;
            case TriggerBehavior.StartEndAnimation:
                endAnimator.SetTrigger("play_end");
                player.activeState = PlayerControl.PlayerState.CutScene;
                player.playerAnimator.enabled = false;
                endCanvas.SetActive(true);
                break;
            case TriggerBehavior.RemoveStartCanvas:
                startCanvas.SetActive(false);
                break;
            case TriggerBehavior.SetPlayerGravityScale:
                playerRB.gravityScale = newGravity;
                break;
        }
    }


    [System.Serializable]
    public struct CamSettings
    {
        public float camSize, xOffset, yOffset;
        public bool useSmoothing;
    }

}