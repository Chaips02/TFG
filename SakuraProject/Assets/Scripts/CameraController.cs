using UnityEngine;

public class DirectCameraController : MonoBehaviour
{
    public float speed = 5.0f; // Velocidad de movimiento hacia adelante/atrás
    public float rotationSpeed = 100.0f; // Velocidad de rotación

    private Rigidbody rb; // Referencia al Rigidbody

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtener el componente Rigidbody
    }

    private void FixedUpdate()
    {
        // Leer el input del teclado para movimiento adelante/atrás
        float moveVertical = Input.GetAxis("Vertical"); // W y S

        // Calcular el movimiento hacia adelante/atrás
        Vector3 movement = transform.forward * moveVertical;

        // Aplicar el movimiento
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        // Leer el input del teclado para rotación
        float rotateHorizontal = Input.GetAxis("Horizontal"); // A y D

        // Calcular la rotación
        Quaternion turn = Quaternion.Euler(0f, rotateHorizontal * rotationSpeed * Time.fixedDeltaTime, 0f);

        // Aplicar la rotación
        rb.MoveRotation(rb.rotation * turn);
    }
}
