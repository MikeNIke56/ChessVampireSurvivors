using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;

    //as long as speed is lower than player velocity- how much the camera "lags" behind
    public float speed;

    //the difference in z-axis
    Vector3 offset;

    private void Awake()
    {
        offset = target.position - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position - offset, speed * Time.deltaTime);
    }
}
