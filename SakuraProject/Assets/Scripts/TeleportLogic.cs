using UnityEngine;

public class TeleportLogic : MonoBehaviour
{
    public static TeleportLogic instance;

    [SerializeField] private GameObject _firstVagonDoorGO;
    [SerializeField] private GameObject _secondVagonDoorGO;
    [SerializeField] private float _doorOpenPosZ;
    [SerializeField] private float _doorClosedPosZ;
    [SerializeField] private Transform _secondVagonTeleportTransform;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CloseFirstVagonDoor(Transform player)
    {
        float time = 1f;
        LTDescr move = LeanTween.moveLocalZ(_firstVagonDoorGO, _doorClosedPosZ, time);
        move.setOnComplete(() =>
        {
            TeleportPlayer(player);
            OpenFirstVagonDoor();
            OpenSecondVagonDoor();
        });
    }

    private void OpenFirstVagonDoor()
    {
        float time = 1f;
        LeanTween.moveLocalZ(_firstVagonDoorGO, _doorOpenPosZ, time);
    }

    public void CloseSecondVagonDoor()
    {
        float time = 1f;
        LeanTween.moveLocalZ(_secondVagonDoorGO, _doorClosedPosZ, time);
    }

    private void OpenSecondVagonDoor()
    {
        float time = 1f;
        LeanTween.moveLocalZ(_secondVagonDoorGO, _doorOpenPosZ, time).setDelay(2f);
    }

    private void TeleportPlayer(Transform player)
    {
        player.position = new Vector3(_secondVagonTeleportTransform.position.x, player.position.y, _secondVagonTeleportTransform.position.z);
    }
}
