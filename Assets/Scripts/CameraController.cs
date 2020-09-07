using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed;
    public float camMoveSpeed = 5.0f;
    public float bounceOffset = 7.0f;
    public float xMin, xMax, zMin, zMax;

    Vector3 newPosition = Vector3.zero;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    newPosition = transform.position;
                    break;

                case TouchPhase.Moved:
                    var delta = touch.deltaPosition * scrollSpeed;
                    newPosition.x -= delta.x;
                    newPosition.z -= delta.y;
                    newPosition.x = Mathf.Clamp(newPosition.x, xMin - bounceOffset, xMax + bounceOffset);
                    newPosition.z = Mathf.Clamp(newPosition.z, zMin - bounceOffset, zMax + bounceOffset);
                    break;
            }
        }

        var p = transform.position;

        transform.position = Vector3.Lerp(p, newPosition, camMoveSpeed * Time.deltaTime);

        if (p.x > xMax)
            p.x = xMax - bounceOffset;

        if (p.x < xMin)
            p.x = xMin + bounceOffset;

        if (p.z > zMax)
            p.z = zMax - bounceOffset;

        if (p.z < zMin)
            p.z = zMin + bounceOffset;

        newPosition = p;
    }
}
