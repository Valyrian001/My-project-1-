using UnityEngine;

public class CarControllerN : MonoBehaviour
{
    public float speed = 10f; // Speed of the car
    public float turnSpeed = 50f; // Turning speed

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get user input
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // Move the car forward or backward
        transform.Translate(Vector3.forward * move);

        // Rotate the car
        transform.Rotate(Vector3.up * turn);
    }
}
