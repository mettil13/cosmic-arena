using System.Runtime.CompilerServices;
using UnityEngine;

public class inputTest : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public float speedMultipier = 1;
    public Transform cursor;
    float lastTimeClicked = 0;
    public float coolDown = 0.5f;
    private Vector2 direction;

    private void Update() {
        float horizontal;
        float vertical;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0) {
            direction = new Vector2(horizontal, vertical).normalized;
        }

        cursor.eulerAngles = new Vector3(0, -Vector2.SignedAngle(Vector2.right, direction), 0);

        if (Input.GetKeyDown(KeyCode.Return) && Time.time - lastTimeClicked > coolDown) {
            myRigidbody.AddForce(new Vector3(direction.x, 0, direction.y) * speedMultipier, ForceMode.Impulse); //velocity o addforce?
            lastTimeClicked = Time.time;
        }
    }
}
