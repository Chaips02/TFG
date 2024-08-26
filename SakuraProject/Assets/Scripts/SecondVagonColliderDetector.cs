using UnityEngine;

public class SecondVagonColliderDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // Player
        {
            TeleportLogic.instance.CloseSecondVagonDoor();
        }
    }
}
