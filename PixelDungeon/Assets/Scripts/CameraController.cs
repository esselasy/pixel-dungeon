using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject hero;
    new Camera camera;

    float size = 9;         // Orthographic camera size
    float zoomSpeed = 10;   // 줌 스피드.

    // Start is called before the first frame update
    void Awake()
    {
        hero = GameObject.FindWithTag("Player");
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 heroPos = hero.transform.position;
            transform.position = new Vector3(heroPos.x, heroPos.y, transform.position.z);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //print("Zoom " + Input.GetAxis("Mouse ScrollWheel"));
            Zoom();
        }
    }

    void Zoom()
    {
        size -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        size = Mathf.Clamp(size, 5, 25);
        camera.orthographicSize = size;
    }
}
