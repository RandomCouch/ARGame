using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperCube : MonoBehaviour
{
    // Start is called before the first frame update
    private Color _oldSphereColor;

    private bool _canMove = false;

    private Vector3 _sphereOffset;

    private Transform _targetSphere;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && _canMove && _targetSphere != null)
        {
            //Move the sphere to the helperCube position
            if(Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                _targetSphere.position = transform.position;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched other trigger");
        if(other.tag == "PlayerSphere")
        {
            //can now move the player sphere by touching and moving device
            _targetSphere = other.transform;
            MeshRenderer renderer = other.GetComponent<MeshRenderer>();
            if(renderer != null)
            {
                _oldSphereColor = renderer.material.color;
                renderer.material.color = Color.green;
                _canMove = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "PlayerSphere")
        {
            //can now move the player sphere by touching and moving device
            MeshRenderer renderer = other.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material.color = _oldSphereColor;
                _canMove = false;
            }
        }
    }
    
}
