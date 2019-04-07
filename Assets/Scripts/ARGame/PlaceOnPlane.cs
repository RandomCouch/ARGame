using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARSessionOrigin))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    public Vector3 positionOffset = Vector3.zero;

    public Action<Transform> OnShapeSpawned = null;
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    ARSessionOrigin m_SessionOrigin;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    public void ResetSpawnedObject()
    {
        Destroy(spawnedObject);
        spawnedObject = null;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (spawnedObject == null)
            {
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                    {

                        Pose hitPose = s_Hits[0].pose;
                        if (spawnedObject == null)
                        {
                            spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position + positionOffset, hitPose.rotation);
                        }
                        Debug.Log("Spawned shape");
                        if (OnShapeSpawned != null)
                        {
                            OnShapeSpawned(spawnedObject.transform);
                        }

                    }
                }
            }
            else
            {
                if(Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (m_SessionOrigin.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose hitPose = s_Hits[0].pose;
                        if (spawnedObject != null)
                        {
                            spawnedObject.transform.position = hitPose.position + positionOffset;
                        }
                    }
                }
            }
        }
    }
}
