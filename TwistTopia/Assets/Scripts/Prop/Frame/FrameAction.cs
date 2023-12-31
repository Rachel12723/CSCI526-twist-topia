using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameAction : MonoBehaviour
{
    public GameObject player; // Drag your player object here
    public Transform patrols;  // Drag your enemy object here
    private Transform patrol;
    public CameraState cameraState; 
    public Camera camera;
    public KeyCode catchEnemy;
    public float xTolerance = 0.5f; // A small value to account for minor discrepancies in z-position
    public float yTolerance = 0.5f;
    public float proximityThreshold = 5.0f; // Distance within which frame is considered "close" to player
    
    private SpriteRenderer spriteRenderer;
    public Sprite frameWithEnemy;
    public Sprite frameWithoutEnemy;
    public bool frameState = false;

    private bool needRelease = false;
    public float releaseTime = 3f;
    private float releaseLeft = 0f;
    private float shakeAngle;

    public InputManager inputManager;

    //Data
    public delegate void EnemyEventHandler(string road);
    public static event EnemyEventHandler OnEnemyCatched;

    public delegate void EnterEventHandler(Vector3 player, Vector3 frame, Vector3 enemy);
    public static event EnterEventHandler OnEnemyNotCatched;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        spriteRenderer.sprite = frameWithoutEnemy;
        shakeAngle = 200f;
    }
    
    void Update() {
        if (patrol == null)
        {
            if (inputManager.GetAllowInteraction())
            {
                int state = PlayerPrefs.GetInt("state");
                if (state == 0 && Input.GetKeyDown(catchEnemy))
                {
                    if (cameraState.facingDirection == FacingDirection.Front)
                    {
                        Debug.Log("return key pressed and the direction is front");
                        // Vector3 playerLoc = player.transform.position;
                        Vector3 frameLoc = transform.position;
                        Vector3 frameScreenPos = camera.WorldToScreenPoint(frameLoc);
                        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

                        bool isFrameOnScreen = (frameScreenPos.x >= 0 && frameScreenPos.x <= Screen.width) &&
                                          (frameScreenPos.y >= 0 && frameScreenPos.y <= Screen.height);
                        // float playerXDistanceToFrame = Math.Abs(playerLoc.x - frameLoc.x);
                        // float playerYDistanceToFrame = Math.Abs(playerLoc.y - frameLoc.y);
                        foreach (Transform patrol in patrols)
                        {
                            float enemyXDistanceToFrame = Math.Abs(patrol.position.x - frameLoc.x);
                            float enemyYDistanceToFrame = Math.Abs(patrol.position.y - frameLoc.y);

                            // Debug.Log("player" + playerLoc + "frame location" + frameLoc + "enemy" + enemyModel.transform.position);
                            // Debug.Log("playerXDistanceToFrame" + playerXDistanceToFrame + "playerYDistanceToFrame" + playerYDistanceToFrame + "enemyXDistanceToFrame" + enemyXDistanceToFrame);
                            // if (playerXDistanceToFrame <= proximityThreshold && playerYDistanceToFrame <= yTolerance &&
                            //     enemyXDistanceToFrame <= xTolerance) {
                            if (isFrameOnScreen && enemyXDistanceToFrame <= xTolerance && enemyYDistanceToFrame <= yTolerance)
                            {
                                CaptureEnemy(patrol);
                                OnEnemyCatched?.Invoke(patrol.tag);
                                break;
                            }
                            else
                            {
                                OnEnemyNotCatched?.Invoke(player.transform.position, transform.position, patrol.transform.position);
                            }
                        }

                    }
                }
            }

        }

        DelayReleaseEnemy();
    }

    void CaptureEnemy(Transform patrol) {
        // Deactivate the enemy
        patrol.gameObject.SetActive(false);
        this.patrol = patrol;
        // Destroy(enemyModel);
        frameState = true;
        spriteRenderer.sprite = frameWithEnemy; // change the frame's appearance to indicate the enemy is captured
    }
    
    public void ReleaseEnemy(bool userRelease) {
        // Reactivate the enemy
        //foreach (Transform patrol in patrols)
        //{
        //    if (!patrol.gameObject.activeSelf)
        //    {   // if not user initiated(means user died), original position
        if (patrol)
        {
            if (userRelease)
            {
                needRelease = true;
                releaseLeft = releaseTime;
                patrol.position = player.transform.position;
                patrol.gameObject.GetComponent<PatrolMovement>().UpdatePosition();
            }
            else
            {
                patrol.gameObject.GetComponent<PatrolMovement>().ResetPatrol();
                patrol.gameObject.SetActive(true);
                patrol = null;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                needRelease = false;
                spriteRenderer.sprite = frameWithoutEnemy;
            }
        }
        //    }
        //}
    }

    private void DelayReleaseEnemy()
    {
        if(gameObject.activeSelf && patrol && needRelease && cameraState.GetFacingDirection() == FacingDirection.Front && !cameraState.GetIsRotating())
        {
            if (releaseLeft > 0f)
            {
                if(transform.rotation.eulerAngles.z + shakeAngle * Time.deltaTime > 20 && transform.rotation.eulerAngles.z + shakeAngle * Time.deltaTime < 340)
                {
                    shakeAngle = -shakeAngle;
                }
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + shakeAngle * Time.deltaTime);
                releaseLeft -= Time.deltaTime;
            }
            else
            {
                patrol.gameObject.SetActive(true);
                patrol = null;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                needRelease = false;
                frameState = false;
                spriteRenderer.sprite = frameWithoutEnemy;
            }
        }
    }
}
