using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooking2 : MonoBehaviour
{
    public float sensitivity, smoothing, reach;
    private float _camRotation;

    public GameObject held_object_anchor, character, data_container;

    private RaycastHit hit;
    private GameObject usage_target, held_thing;
    private Quaternion held_thing_rotation;
    private Vector2 mouse_look, smooth_v, md;
    private string general_action, attack_primary, attack_secondary, attack_tertiary;
    public float ex, why, zee;

    // Start is called before the first frame update
    void Start()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        GameEvents.current.LoadCharacterRotation += LoadRotation;

        Cursor.lockState = CursorLockMode.Locked;

        usage_target = null;
        held_thing = null;

        LoadRotation();
    }

    // Update is called once per frame
    void Update()
    {
        data_container = GameObject.FindGameObjectWithTag("DataContainer");

        GeneralAction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetButton(PlayerPrefs.GetString("Item Rotate")))
        {
            held_object_anchor.transform.Rotate(Input.GetAxisRaw("Mouse X") * Vector3.right);
            held_object_anchor.transform.Rotate(Input.GetAxisRaw("Mouse Y") * Vector3.down);
        }
        else
        {
            float y = Input.GetAxis("Mouse Y");
            float x = why+Input.GetAxis("Mouse X");

            _camRotation -= y * sensitivity * Time.deltaTime * 10f;
            _camRotation = Mathf.Clamp(_camRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_camRotation, 0f, 0f);

            character.transform.Rotate(x * sensitivity * Time.deltaTime * 20f * Vector3.up);
        }
    }

    private void GeneralAction()
    {
        if (Input.GetButtonDown(PlayerPrefs.GetString("General Action")))
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
                Debug.Log("No object found");
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

    private void LoadRotation()
    {
        ex = data_container.GetComponent<DataContainer>().character.rotation_x;
        why = data_container.GetComponent<DataContainer>().character.rotation_y;
        zee = data_container.GetComponent<DataContainer>().character.rotation_z;
    }
}
