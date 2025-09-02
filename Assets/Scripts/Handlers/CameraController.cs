using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(-5, 20, -20); 
    [SerializeField] private float followSpeed = 10f; 

    [Header("Occlusion")]
    [SerializeField] private bool avoidOcclusion = true; 
    [SerializeField] private float occlusionRadius = 0.25f; // spherecast radius
    [SerializeField] private float occlusionBackoff = 0.2f; // how far to back off from hit
    [SerializeField] private float targetHeight = 1.0f; // height offset for occlusion cast
    [SerializeField] private LayerMask occlusionLayers = ~0; // layers to check for occlusion

    [Header("Bounds")]
    [SerializeField] private BoxCollider bounds; 

    [Header("Dead Zone")]
    [SerializeField] private float deadZoneRadius; // radius of dead zone around target

    [SerializeField] private Transform target;

    private float baseZoomDistance; 

    private void Start()
    {
        baseZoomDistance = offset.magnitude;
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (target == null) return;
        Vector3 focus = target.position;

        // calc camera pos based on offset
        Vector3 desiredOffset = offset.normalized * Mathf.Max(0.01f, baseZoomDistance);
        Vector3 finalPos = focus + desiredOffset;

        // only move camera if target leaves the dead zone
        Vector3 cameraToTarget = focus - (transform.position - desiredOffset);
        float distance = cameraToTarget.magnitude;
        if (deadZoneRadius > 0f && distance < deadZoneRadius)
        {
            finalPos = transform.position;
        }
        else
        {
            finalPos = Vector3.Lerp(transform.position, finalPos, followSpeed * Time.deltaTime);
        }

        // back off camera if something blocks the view
        if (avoidOcclusion)
        {
            Vector3 castOrigin = target.position + Vector3.up * targetHeight;
            Vector3 toCam = finalPos - castOrigin;
            float dist = toCam.magnitude;
            if (dist > 0.001f)
            {
                Vector3 dir = toCam / dist;
                if (Physics.SphereCast(castOrigin, occlusionRadius, dir, out RaycastHit hit, dist, occlusionLayers, QueryTriggerInteraction.Ignore))
                {
                    finalPos = hit.point - dir * occlusionBackoff;
                }
            }
        }

        // make sure camera stays within bounds
        if (bounds != null)
        {
            Bounds b = bounds.bounds;
            finalPos.x = Mathf.Clamp(finalPos.x, b.min.x, b.max.x);
            finalPos.z = Mathf.Clamp(finalPos.z, b.min.z, b.max.z);
        }

        transform.position = finalPos;
    }
}
