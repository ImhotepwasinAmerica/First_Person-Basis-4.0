using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviorHuman : MonoBehaviour
{
    public float speed, jump_takeoff_speed, height_standing, height_squatting, lean_distance, distance_to_ground, speed_multiplier_squatting, sensitivity, smoothing, reach, acceleration, acceleration_grounded, acceleration_airborne;
    private bool lean_left, lean_right, jump, squat, move_forward, move_back, move_left, move_right, speed_toggle, general_action, item_rotate, attack_primary, attack_secondary, attack_tertiary;
    public GameObject rotation_thing, data_container, held_object_anchor, held_thing, self, camera_or_anchor;

    private float angular_speed, gravity_fake, time_fake, ex, why, zee, axis_x, axis_y;
    private bool is_squatting, is_walking, current_grounded, previous_grounded;
    private GameObject usage_target;
    private Vector3 velocity, velocity_endgoal;
    private Vector2 mouse_look, smooth_v, md;
    private Quaternion lean, held_thing_rotation;
    private RaycastHit hit;

    private CharacterController controller;
    private CapsuleCollider collider;
    private SavedObject guy;

    // Start is called before the first frame update
    void Start()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        controller = GetComponent<CharacterController>();
        collider = GetComponent<CapsuleCollider>();

        current_grounded = true;
        previous_grounded = true;

        angular_speed = Mathf.Sqrt((speed * speed / 2.0f));
        time_fake = 0.01666f;
        gravity_fake = Physics.gravity.y * time_fake;

        lean = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        AlterAcceleration();

        previous_grounded = current_grounded;
        current_grounded = IsGrounded();
    }

    void FixedUpdate()
    {
        velocity.x = Mathf.Lerp(velocity.x, velocity_endgoal.x, acceleration);
        velocity.z = Mathf.Lerp(velocity.z, velocity_endgoal.z, acceleration);
        velocity.y = velocity_endgoal.y;

        // The velocity value shall be changed by standing on moving platforms
        // which shall be done here.

        controller.Move(velocity);
    }

    public bool GetLeanLeft()
    {
        return lean_left;
    }

    public void SetLeanLeft(bool new_lean_left)
    {
        lean_left = new_lean_left;
    }

    public bool GetLeanRight()
    {
        return lean_right;
    }

    public void SetLeanRight(bool new_lean_right)
    {
        lean_right = new_lean_right;
    }

    public bool GetJump()
    {
        return jump;
    }

    public void SetJump(bool new_jump)
    {
        jump = new_jump;
    }

    public bool GetSquat()
    {
        return squat;
    }

    public void SetSquat(bool new_squat)
    {
        squat = new_squat;
    }

    public bool GetMoveForward()
    {
        return move_forward;
    }

    public void SetMoveForward(bool new_move_forward)
    {
        move_forward = new_move_forward;
    }

    public bool GetMoveBack()
    {
        return move_back;
    }

    public void SetMoveBackward(bool new_move_back)
    {
        move_back = new_move_back;
    }

    public bool GetMoveLeft()
    {
        return move_left;
    }

    public void SetMoveLeft(bool new_move_left)
    {
        move_left = new_move_left;
    }

    public bool GetMoveRight()
    {
        return move_right;
    }

    public void SetMoveRight(bool new_move_right)
    {
        move_right = new_move_right;
    }

    public bool GetSpeedToggle()
    {
        return speed_toggle;
    }

    public void SetSpeedToggle(bool new_speed_toggle)
    {
        speed_toggle = new_speed_toggle;
    }

    public bool GetGeneralAction()
    {
        return general_action;
    }

    public void SetGeneralAction(bool new_general_action)
    {
        general_action = new_general_action;
    }

    public bool GetItemRotate()
    {
        return item_rotate;
    }

    public void SetItemRotate(bool new_item_rotate)
    {
        item_rotate = new_item_rotate;
    }

    public bool GetAttackPrimary()
    {
        return attack_primary;
    }

    public void SetAttackPrimary(bool new_attack_primary)
    {
        attack_primary = new_attack_primary;
    }

    public bool GetAttackSecondary()
    {
        return attack_secondary;
    }

    public void SetAttackSecondary(bool new_attack_secondary)
    {
        attack_primary = new_attack_secondary;
    }

    public bool GetAttackTertiary()
    {
        return attack_tertiary;
    }

    public void SetAttackTertiary(bool new_attack_tertiary)
    {
        attack_primary = new_attack_tertiary;
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
        else if (velocity_endgoal.y > -0.00327654 && !jump)
        {
            velocity_endgoal.y += (0.5f * gravity_fake * time_fake);
        }
    }

    private void ControlLean()
    {
        if (lean_left && lean_right)
        {
            lean.z = Mathf.Lerp(lean.z, 0, 0.09f);
        }
        else if (lean_left && !lean_right)
        {
            lean.z = Mathf.Lerp(lean.z, 0.15f, 0.09f);
        }
        else if (lean_right && !lean_left)
        {
            lean.z = Mathf.Lerp(lean.z, -0.15f, 0.09f);
        }


        rotation_thing.transform.localRotation = lean;
    }

    public void CycaBlyat(float height_goal, float rate)
    {
        if (collider.height > height_goal)
        {
            collider.height = Mathf.Lerp(collider.height, height_goal, rate);//-= 0.1f;
            controller.height = Mathf.Lerp(controller.height, height_goal, rate);//-= 0.1f;
            distance_to_ground = Mathf.Lerp(distance_to_ground, height_goal / 2, rate/2);//-= 0.05f;
            transform.localScale.Set(0, Mathf.Lerp(transform.localScale.y, height_goal, rate/2), 0); //-= new Vector3(0, 0.05f, 0);
        }
        else if (collider.height < height_goal
            && !IsBeneathSomething())
        {
            collider.height = Mathf.Lerp(collider.height, height_goal, rate);//+= 0.1f;
            controller.height = Mathf.Lerp(controller.height, height_goal, rate);//+= 0.1f;
            distance_to_ground = Mathf.Lerp(distance_to_ground, height_goal / 2, rate/2);//+= 0.05f;
            transform.localScale.Set(0, Mathf.Lerp(transform.localScale.y, height_goal, rate/2), 0); //-= new Vector3(0, 0.05f, 0);
        }
    }

    private void GetInput()
    {
        if (item_rotate)
        {
            held_object_anchor.transform.Rotate(axis_x * Vector3.right);
            held_object_anchor.transform.Rotate(axis_y * Vector3.down);
        }
        else
        {
            md = new Vector2(axis_x, axis_y);

            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smooth_v.x = Mathf.Lerp(smooth_v.x, md.x, 1f / smoothing);
            smooth_v.y = Mathf.Lerp(smooth_v.y, md.y, 1f / smoothing);
            mouse_look += smooth_v;

            mouse_look.y = Mathf.Clamp(mouse_look.y, -90f, 90f);

            camera_or_anchor.transform.localRotation = Quaternion.AngleAxis(-mouse_look.y, Vector3.right); // up and down
            camera_or_anchor.transform.localRotation = Quaternion.Euler(0, mouse_look.x + why, 0); // left and right
        }
    }

    private void GeneralAction()
    {
        if (general_action)
        {
            try
            {
                usage_target = ReturnUsableObject();
                usage_target.GetComponent<ObjectBehaviorDefault>().UseDefault(held_object_anchor);

                if (PlayerPrefs.GetString("togglehold_carry") == "toggle")
                {
                    if (usage_target.tag == "Holdable" && held_thing == null)
                    {
                        held_thing = usage_target;
                    }
                    else if (held_thing == usage_target)
                    {
                        held_thing = null;
                    }
                }

                usage_target = null;
            }
            catch (System.NullReferenceException e)
            {
                usage_target = null;
            }
        }

        if (Input.GetButton(PlayerPrefs.GetString("General Action")))
        {
            try
            {
                usage_target = ReturnUsableObject();
                usage_target.GetComponent<ObjectBehaviorDefault>().UseDefaultHold(held_object_anchor);

                if (PlayerPrefs.GetString("togglehold_carry") == "toggle")
                {
                    if (usage_target.tag == "Holdable" && held_thing == null)
                    {
                        held_thing = usage_target;
                    }
                    else if (held_thing == usage_target)
                    {
                        held_thing = null;
                    }
                }
            }
            catch (System.NullReferenceException e)
            {
                usage_target = null;
            }
        }
        else
        {
            if (usage_target != null)
            {
                usage_target.GetComponent<ObjectBehaviorDefault>().UseDefaultHoldRelease();
                usage_target = null;
                held_thing = null;
            }
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
            && !Input.GetButton(PlayerPrefs.GetString("Jump"))
            && !Input.GetButtonDown(PlayerPrefs.GetString("Jump"))) // If the fact of the player not jumping is not assured as this is called, the character will not be able to jumpt at all.
        {
            if (current_grounded && previous_grounded)
            {
                controller.Move(new Vector3(0, -hit.distance, 0));
                return true;
            }
        }

        return false;
    }

    private void Jump(bool jump)
    {
        if (jump)
        {
            velocity_endgoal.y += (jump_takeoff_speed * time_fake);
        }
    }

    private void LoadRotation()
    {
        ex = data_container.GetComponent<DataContainer>().character.rotation_x;
        why = data_container.GetComponent<DataContainer>().character.rotation_y;
        zee = data_container.GetComponent<DataContainer>().character.rotation_z;
    }

    private void MovementLean()
    {
        lean.z = Mathf.Lerp(lean.z, -velocity_endgoal.x / 5, 0.09f);
        rotation_thing.transform.localRotation = lean;
    }

    private GameObject ReturnUsableObject()
    {
        GameObject thing = null;

        Physics.Raycast(transform.position, transform.forward, out hit, reach);

        if (hit.collider.gameObject != null)
        {
            thing = hit.collider.gameObject;
        }

        return thing;
    }

    private void SquatSpeedAugment()
    {
        velocity_endgoal.x *= speed_multiplier_squatting;
        velocity_endgoal.z *= speed_multiplier_squatting;
    }

    public void Walk(bool forward, bool backward, bool left, bool right)
    {
        if (forward && !backward)
        {
            velocity_endgoal.z = time_fake;
        }
        else if (!forward && backward)
        {
            velocity_endgoal.z = -time_fake;
        }
        else
        {
            velocity_endgoal.z = 0;
        }

        if (left && !right)
        {
            velocity_endgoal.x = time_fake;
        }
        else if (right && !left)
        {
            velocity_endgoal.x = -time_fake;
        }
        else
        {
            velocity_endgoal.x = 0;
        }

        if ((forward && left)
            || (forward && right)
            || (backward && left)
            || (backward && right))
        {
            velocity_endgoal.z *= angular_speed;
            velocity_endgoal.x *= angular_speed;
        }
        else
        {
            velocity_endgoal.z *= speed;
            velocity_endgoal.x *= speed;
        }
    }

    private void WalkRun(bool be_walking)
    {
        is_walking = be_walking;
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
    }

    private void WalkRunSpeedAugment()
    {
        if (is_walking)
        {
            velocity_endgoal.x *= 0.5f;
            velocity_endgoal.z *= 0.5f;
        }
    }
}
