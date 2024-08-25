using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    private float _time = 0f;
    private const float _allowRotationTime = 2f;

    Vector2 velocity;
    Vector2 frameVelocity;

    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;

        // Rotate the character by 180 degrees around the Y axis at start
        character.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (_time >= _allowRotationTime)
        {
            // Get smooth velocity.
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            // Rotate camera up-down and controller left-right from velocity.
            float lerp = _time < _allowRotationTime * 3f ? Time.deltaTime : 1f;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(-velocity.y, Vector3.right), lerp);
            character.localRotation = Quaternion.Euler(0, velocity.x + 180, 0); // keep the initial 180 degrees rotation
        }
        
        _time += Time.deltaTime;
    }
}
