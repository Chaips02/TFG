using UnityEngine;

public class CinematicColliderDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // Player
        {
            GetComponent<Collider>().enabled = false;
            AudioManager.instance.PlayEnding();
            EndingCanvasController.instance.ShowPanelTween();
        }
    }
}
