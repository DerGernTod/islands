using scripts.utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scripts.camera
{
    public class CameraController : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            Vector3 offset = new Vector3(Input.GetAxis(Constants.INPUT_AXIS_HORIZONTAL), Input.GetAxis(Constants.INPUT_AXIS_VERTICAL));
            Vector3 curPos = transform.position;
            transform.position = new Vector3(
                Mathf.Clamp(curPos.x + offset.x, -Constants.CAMERA_MAX_HORIZONTAL, Constants.CAMERA_MAX_HORIZONTAL), 
                Mathf.Clamp(curPos.y + offset.y, -Constants.CAMERA_MAX_VERTICAL, Constants.CAMERA_MAX_VERTICAL), 
                transform.position.z);
        }
    }
}