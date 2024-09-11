using UnityEngine;

public class EndingCanvasController : MonoBehaviour
{
    public static EndingCanvasController instance;

    [SerializeField] private Animator _animator;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _panel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ShowPanelTween()
    {
        float time = 1f;
        LTDescr show = LeanTween.alphaCanvas(_canvasGroup, 1f, time);
        show.setOnComplete(() =>
        {
            CamerasManager.instance.ActivateEndingCamera();
            _panel.SetActive(false);
            ActivateAnimation();
        });
    }

    private void ActivateAnimation()
    {
        _animator.SetTrigger("activate");
    }
}
