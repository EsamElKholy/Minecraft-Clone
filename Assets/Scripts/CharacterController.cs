using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public bool isGrounded = false;
    public bool isSprinting = false;

    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float jumpForce = 5f;
    
    public float gravity = -9.8f;
    
    public float playerWidth = 0.15f;
    public float boundsTolerance = 0.1f;

    private float horizontal;
    private float vertical;
    private float mouseHorizontal;
    private float mouseVertical;
    private Camera mainCamera;
    private World world;
    private Vector3 velocity;
    private float verticalMomentum = 0;
    private bool jumpRequest = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (world == null)
        {
            var voxelGenerator = GameObject.FindObjectOfType<VoxelGenerator>();

            if (voxelGenerator)
            {
                world = voxelGenerator.world;
            }
        }

        UpdateVelocity();

        if (jumpRequest)
        {
            Jump();
        }
       
        transform.Translate(velocity, Space.World);
    }

    private void Update()
    {
        UpdateInput();
        
        transform.Rotate(Vector3.up * mouseHorizontal);

        mainCamera.transform.Rotate(Vector3.right * -mouseVertical);
    }

    private void UpdateVelocity() 
    {
        velocity = new Vector3();

        if (verticalMomentum > gravity)
        {
            verticalMomentum += Time.fixedDeltaTime * gravity;
        }

        if (isSprinting)
        {
            velocity = ((transform.forward * vertical) + (transform.right * horizontal)).normalized * Time.fixedDeltaTime * sprintSpeed;
        }
        else
        {
            velocity = ((transform.forward * vertical) + (transform.right * horizontal)).normalized * Time.fixedDeltaTime * walkSpeed;
        }

        velocity += (transform.up * verticalMomentum * Time.fixedDeltaTime);

        if ((velocity.z > 0 && Front) || (velocity.z < 0 && Back))
        {
            velocity.z *= -boundsTolerance;
        }

        if ((velocity.x > 0 && Right) || (velocity.x < 0 && Left))
        {
            velocity.x *= -boundsTolerance;
        }

        if (velocity.y < 0)
        {
            velocity.y = CheckDownSpeed(velocity.y);
        }
        
        if (velocity.y > 0)
        {
            velocity.y = CheckUpSpeed(velocity.y);
        }
    }

    private void Jump() 
    {
        verticalMomentum = jumpForce;
        isGrounded = false;
        jumpRequest = false;
    }


    private void UpdateInput() 
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
        }

        if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
        }
    }

    private float CheckDownSpeed(float downSpeed) 
    {
        if (world.IsWorldVoxelSolid(new Vector3(transform.position.x - playerWidth, transform.position.y + downSpeed, transform.position.z - playerWidth)) ||
            world.IsWorldVoxelSolid(new Vector3(transform.position.x - playerWidth, transform.position.y + downSpeed, transform.position.z + playerWidth)) ||
            world.IsWorldVoxelSolid(new Vector3(transform.position.x + playerWidth, transform.position.y + downSpeed, transform.position.z - playerWidth)) ||
            world.IsWorldVoxelSolid(new Vector3(transform.position.x + playerWidth, transform.position.y + downSpeed, transform.position.z + playerWidth)))
        {
            isGrounded = true;
            
            return 0;
        }
        else
        {
            return downSpeed;
        }
    }

    private float CheckUpSpeed(float upSpeed)
    {
        if (world.IsWorldVoxelSolid(new Vector3(transform.position.x - playerWidth, transform.position.y + 2f + upSpeed, transform.position.z - playerWidth)) ||
            world.IsWorldVoxelSolid(new Vector3(transform.position.x - playerWidth, transform.position.y + 2f + upSpeed, transform.position.z + playerWidth)) ||
            world.IsWorldVoxelSolid(new Vector3(transform.position.x + playerWidth, transform.position.y + 2f + upSpeed, transform.position.z - playerWidth)) ||
            world.IsWorldVoxelSolid(new Vector3(transform.position.x + playerWidth, transform.position.y + 2f + upSpeed, transform.position.z + playerWidth)))
        {
            return 0;
        }
        else
        {
            return upSpeed;
        }
    }

    private bool Front 
    {
        get 
        {
            if (world.IsWorldVoxelSolid(new Vector3(transform.position.x, transform.position.y, transform.position.z + playerWidth)) ||
                world.IsWorldVoxelSolid(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + playerWidth)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private bool Back
    {
        get
        {
            if (world.IsWorldVoxelSolid(new Vector3(transform.position.x, transform.position.y, transform.position.z - playerWidth)) ||
                world.IsWorldVoxelSolid(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z - playerWidth)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private bool Left
    {
        get
        {
            if (world.IsWorldVoxelSolid(new Vector3(transform.position.x - playerWidth, transform.position.y, transform.position.z)) ||
                world.IsWorldVoxelSolid(new Vector3(transform.position.x - playerWidth, transform.position.y + 1f, transform.position.z)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private bool Right
    {
        get
        {
            if (world.IsWorldVoxelSolid(new Vector3(transform.position.x + playerWidth, transform.position.y, transform.position.z)) ||
                world.IsWorldVoxelSolid(new Vector3(transform.position.x + playerWidth, transform.position.y + 1f, transform.position.z)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
