using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour
{
    // Rotate
    public KeyCode rotateKeyCode;
    private FacingDirection facingDirection = FacingDirection.Front;

    // Player
    public GameObject player;
    private PlayerMovement playerMovement;

    // Platform Cubes
    public Transform platformCubes;

    // Invisible Cubes
    public GameObject invisibleCube;
    public Transform invisibleCubes;
    private List<Transform> invisibleList = new List<Transform>();

    // World Unit
    public float WorldUnit = 1.000f;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        UpdateInvisibleCubes();
    }

    void Update()
    {
        if (Input.GetKeyDown(rotateKeyCode))
        {
            if (facingDirection == FacingDirection.Front)
            {
                facingDirection = FacingDirection.Up;
                playerMovement.UpdateFacingDirection(facingDirection, 90f);
            }
            else if (facingDirection == FacingDirection.Up)
            {
                facingDirection = FacingDirection.Front;
                playerMovement.UpdateFacingDirection(facingDirection, 0f);
            }
            UpdateInvisibleCubes();
        }
        if (OnInvisibleCube())
        {
            MovePlayerToClosestPlatformCube();
            if (facingDirection == FacingDirection.Front)
            {
                UpdateInvisibleCubes();
            }
        }
    }

    // Determines if the player is on an invisible cube
    private bool OnInvisibleCube()
    {
        foreach (Transform cube in invisibleList)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit / 2
                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit / 2
                && player.transform.position.y - cube.position.y <= WorldUnit + 0.2f
                && player.transform.position.y - cube.position.y > 0)
                {
                    return true;
                }
            }
            else if (facingDirection == FacingDirection.Up)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit
                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit
                && player.transform.position.y - cube.position.y > 0)
                {
                    return true;
                }
            }

        }
        return false;
    }

    // Move player to the closest platform cube when player on an Invisible Cube
    private void MovePlayerToClosestPlatformCube()
    {
        // Get the Vector
        float frontZ = float.MaxValue;
        float upY = float.MinValue;
        foreach (Transform cube in platformCubes)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit / 2
                && player.transform.position.y - cube.position.y <= WorldUnit + 0.2f
                && player.transform.position.y - cube.position.y > 0)
                {
                    frontZ = Mathf.Min(frontZ, cube.transform.position.z);
                }
            }
            else if (facingDirection == FacingDirection.Up)
            {
                if (Mathf.Abs(cube.position.x - player.transform.position.x) < WorldUnit
                && Mathf.Abs(cube.position.z - player.transform.position.z) < WorldUnit
                && player.transform.position.y > 1)
                {
                    upY = Mathf.Max(upY, cube.transform.position.y + 1);
                }
            }
        }

        // Change the Position
        if (facingDirection == FacingDirection.Front && frontZ != float.MaxValue)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, frontZ);
            player.GetComponent<CharacterController>().enabled = true;
        }
        else if (facingDirection == FacingDirection.Up && upY != float.MinValue)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, upY, player.transform.position.z);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    // Update the invisible cubes when the direction changes
    public void UpdateInvisibleCubes()
    {
        // Delete the invisible cubes
        foreach (Transform cube in invisibleList)
        {
            Destroy(cube.gameObject);
        }
        invisibleList.Clear();

        // Create new invisible cubes
        Vector3 newCubePosition = Vector3.zero;
        foreach (Transform cube in platformCubes)
        {
            if (facingDirection == FacingDirection.Front)
            {
                newCubePosition = new Vector3(cube.position.x, cube.position.y, GetCubeZByPlayer());
            }
            else if (facingDirection == FacingDirection.Up)
            {
                newCubePosition = new Vector3(cube.position.x, GetCubeYByPlatformCubes(), cube.position.z);
            }
            if (!ExistCube(invisibleList, newCubePosition) && !ExistCube(platformCubes, newCubePosition))
            {
                GameObject newCube = Instantiate(invisibleCube) as GameObject;
                newCube.transform.position = newCubePosition;
                invisibleList.Add(newCube.transform);
                newCube.transform.SetParent(invisibleCubes);
            }
        }
    }

    // Get z axis of cube by player
    private float GetCubeZByPlayer()
    {
        return Mathf.Round(player.transform.position.z);
    }

    // Get y axis of cube by platform cubes
    private float GetCubeYByPlatformCubes()
    {
        float platformCubeDepth = float.MaxValue;
        foreach (Transform cube in platformCubes)
        {
            platformCubeDepth = Mathf.Min(platformCubeDepth, cube.transform.position.y);
        }
        return Mathf.Round(platformCubeDepth);
    }

    // Find if exists invisible cube
    private bool ExistCube(List<Transform> list, Vector3 newCube)
    {
        foreach (Transform cube in list)
        {
            if (cube.position == newCube)
            {
                return true;
            }
        }
        return false;
    }

    // Find if exists platform cube
    private bool ExistCube(Transform transform, Vector3 newCube)
    {
        foreach (Transform cube in transform)
        {
            if (cube.position == newCube)
            {
                return true;
            }
        }
        return false;
    }
}
