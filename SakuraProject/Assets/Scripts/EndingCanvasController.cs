using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class EndingCanvasController : MonoBehaviour
{
    public static EndingCanvasController instance;

    [SerializeField] private Animator _animator;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _panel;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _videoRawImage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _videoPlayer.loopPointReached += QuitApplication;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Application Quit");
            Application.Quit();
        }
    }

    private void OnDestroy()
    {
        _videoPlayer.loopPointReached -= QuitApplication;
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
            StartCoroutine(PlayCreditsVideo());
        });
    }

    private void ActivateAnimation()
    {
        _animator.SetTrigger("activate");
    }

    private IEnumerator PlayCreditsVideo()
    {
        yield return new WaitForSeconds(17f);
        _videoRawImage.SetActive(true);
        _videoPlayer.Play();
        _panel.SetActive(true);
    }

    private void QuitApplication(VideoPlayer vp)
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}
