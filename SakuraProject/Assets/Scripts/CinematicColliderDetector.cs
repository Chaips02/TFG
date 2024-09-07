using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicColliderDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // Player
        {
            AudioManager.instance.PlayEnding();
        }
    }
}
