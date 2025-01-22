using UnityEngine;

public class GraphicTest : MonoBehaviour
{
    [SerializeField] private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.AddTorque(Random.insideUnitSphere * 100);
    }
}
