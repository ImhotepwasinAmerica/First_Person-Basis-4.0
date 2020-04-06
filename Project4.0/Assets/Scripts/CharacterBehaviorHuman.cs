using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviorHuman : MonoBehaviour
{
    private float angular_speed, gravity_fake, time_fake, acceleration;
    private bool is_squatting, is_walking, current_grounded, previous_grounded;
    public float speed, jump_takeoff_speed, height_standing, height_squatting, lean_distance, distance_to_ground, speed_multiplier_squatting;
    public GameObject rotation_thing, data_container;
    private Vector3 velocity, velocity_endgoal;
    private Quaternion lean;

    private Transform transformation;
    private CharacterController controller;
    private CapsuleCollider collider;
    private SavedObject guy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AlterAcceleration();

        previous_grounded = current_grounded;
        current_grounded = IsGrounded();
    }

    private void AlterAcceleration()
    {
        if (IsGrounded())
        {
            acceleration = 0.15f;
        }
        else
        {
            acceleration = 0.1f;
        }
    }

    private void ApplyGravity()
    {
        if (IsGrounded())
        {
            velocity_endgoal.y = (gravity_fake * time_fake);
        }
        else
        {
            if (Time.timeScale > 0.1f)
            {
                velocity_endgoal.y += (gravity_fake * time_fake);
            }
        }
    }

    private void BetterMovement()
    {
        // Better jumping and falling
        if (velocity_endgoal.y < -0.00327654 && Time.timeScale > 0.1f)
        {
            velocity_endgoal.y += gravity_fake * time_fake;
        }
        else if (velocity_endgoal.y > -0.00327654 && !(Input.GetButton(PlayerPrefs.GetString("Jump"))))
        {
            velocity_endgoal.y += (0.5f * gravity_fake * time_fake);
        }
    }

    private bool IsBeneathSomething()
    {
        return Physics.Raycast(transform.position, Vector3.up, controller.height - distance_to_ground + 0.3f);
    }

    private bool IsGrounded()
    {
        if (controller.isGrounded)
        {
            return true;
        }

        Vector3 bottom = controller.transform.position - new Vector3(0, controller.height / 2, 0);

        RaycastHit hit;

        if (Physics.Raycast(bottom, new Vector3(0, -1, 0), out hit, 1.5f)
            && !(Input.GetButton(PlayerPrefs.GetString("Jump")))
            && !Input.GetButtonDown(PlayerPrefs.GetString("Jump")))
        {
            if (current_grounded && previous_grounded)
            {
                controller.Move(new Vector3(0, -hit.distance, 0));
                return true;
            }
        }

        return false;
    }

    private void MovementLean()
    {
        lean.z = Mathf.Lerp(lean.z, -velocity_endgoal.x / 5, 0.09f);
        rotation_thing.transform.localRotation = lean;
    }

    private void WalkRun(bool be_walking)
    {
        is_walking = be_walking;

        if (is_walking)
        {
            velocity_endgoal.x *= 0.5f;
            velocity_endgoal.z *= 0.5f;
        }
    }

    private void WalkRun()
    {
        
        if (is_walking)
        {
            is_walking = false;
        }
        else if (!is_walking)
        {
            is_walking = true;
        }
        
        if (is_walking)
        {
            velocity_endgoal.x *= 0.5f;
            velocity_endgoal.z *= 0.5f;
        }
    }
}
