using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    public static CamerasManager instance;

    [SerializeField] private GameObject _firstPersonCamera;
    [SerializeField] private GameObject _endingCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ActivateEndingCamera()
    {
        _firstPersonCamera.SetActive(false);
        _endingCamera.SetActive(true);
    }
}
