using UnityEngine;

public class FirstVagonColliderDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // Player
        {
            TeleportLogic.instance.CloseFirstVagonDoor(other.transform);
        }
    }
}
