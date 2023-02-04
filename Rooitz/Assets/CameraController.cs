using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton
public class CameraController : MonoBehaviour
{
    #region Singleton instance
    private static CameraController instance;
    #endregion

    #region References and variables
    [Header("References")]
    [SerializeField] private Transform player;
    private Camera cam;
    [Header("Variables")]
    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;
    [SerializeField] private float camSize = 6;
    [SerializeField] private float smoothingSpeed = 0.2f;
    private float targetXOffset, targetYOffset, targetCamSize;
    #endregion

    #region Initialization and update
    private void Awake() {
        instance = this;
        cam = transform.GetComponent<Camera>();
        targetXOffset = xOffset;
        targetYOffset = yOffset;
        targetCamSize = camSize;
    }
    private void Update() {
        FollowPlayer(xOffset, camSize);

        if(xOffset != targetXOffset)
        {
            xOffset = Mathf.Lerp(xOffset, targetXOffset, smoothingSpeed * Time.deltaTime);
        }
        if(yOffset != targetYOffset)
        {
            yOffset = Mathf.Lerp(yOffset, targetYOffset, smoothingSpeed * Time.deltaTime);
        }
        if(camSize != targetCamSize)
        {
            camSize = Mathf.Lerp(camSize, targetCamSize, smoothingSpeed * Time.deltaTime);
        }
        

    }
    private static void FollowPlayer(float _xOffset, float _camSize)
    {
        Vector3 newPos = instance.player.transform.position + new Vector3(_xOffset, instance.yOffset, 0f);
        instance.transform.position = new Vector3(newPos.x, newPos.y, instance.transform.position.z); // Don't set z by player pos
        instance.cam.orthographicSize = _camSize;
    }
    #endregion

    #region Set variables
    public static void SetXOffset(float _xOffset, bool _useSmoothing)
    {
        if(_useSmoothing) instance.targetXOffset = _xOffset;
        else
        {
            instance.targetXOffset = _xOffset;
            instance.xOffset = _xOffset;
        }
    }
    public static void SetYOffset(float _yOffset, bool _useSmoothing)
    {
        if(_useSmoothing) instance.targetYOffset = _yOffset;
        else
        {
            instance.targetYOffset = _yOffset;
            instance.yOffset = _yOffset;
        }
    }
    public static void SetCamSize(float _camSize, bool _useSmoothing)
    {
        if(_useSmoothing) instance.targetCamSize = _camSize;
        else
        {
            instance.targetCamSize = _camSize;
            instance.camSize = _camSize;
        }
    }
    #endregion
}