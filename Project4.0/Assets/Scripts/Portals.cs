using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour
{
    public GameObject portal_this, portal_other, character, camera_main;
    public Camera camera_this, camera_other;
    public RenderTexture texture_this;
    
    // Start is called before the first frame update
    void Start()
    {
        camera_main = GameObject.FindGameObjectWithTag("MainCamera");
        //camera_this = GetComponentInChildren<Camera>();

        // Assumes that the portal is assigned a render texture
        //texture_this = this.GetComponent<RenderTexture>();
        //texture_this.height = Screen.height;
        //texture_this.width = Screen.width;
        
    }

    // Update is called once per frame
    void Update()
    {
        camera_other.transform.localPosition = new Vector3(
            (camera_main.transform.position.y - this.transform.position.y),
            (camera_main.transform.position.x - this.transform.position.x),
            -(camera_main.transform.position.z - this.transform.position.z));
        //camera_other.transform.localEulerAngles = new Vector3(
        //    (Mathf.Atan(camera_other.transform.localPosition.z / camera_other.transform.localPosition.y) * (180 / Mathf.PI)) - 90,
        //    (Mathf.Atan(camera_other.transform.localPosition.x / camera_other.transform.localPosition.z) * (180 / Mathf.PI)),
        //    180);
    }
}
