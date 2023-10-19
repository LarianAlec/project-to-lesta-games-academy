using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _winScreenTimer;
    [SerializeField] TextMeshProUGUI _defeatScreenTimer;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _winPanelUI;
    [SerializeField] GameObject _defeatPanelUI;
    [SerializeField] GameObject _thirdPersonCam;
    [SerializeField] AnimationControls _animator;

    private Scene _activeScene;
    private Timer _timer;

    void Start()
    {
        _activeScene = SceneManager.GetActiveScene();
        _timer = GetComponent<Timer>();
    }

    public void RestartLevel()
    {
        Debug.Log("RestartLevel()");
        SceneManager.LoadScene(_activeScene.name);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnWinScreen()
    {
        // Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // Update Timer
        _timer.StopTimer();
        _winScreenTimer.text = "Время: " + _timer.GetFinishTime();

        // Player
        _player.GetComponent<PlayerMovement>().enabled = false;
        _thirdPersonCam.gameObject.SetActive(false);
        _animator.PlayWinDance();

        // WinPanel
        _winPanelUI.gameObject.SetActive(true);
    }

    public void OnDefeatScreen()
    {
        // Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // Update Timer
        _timer.StopTimer();
        _defeatScreenTimer.text = "Время: " + _timer.GetFinishTime();

        // Player
        _player.GetComponent<PlayerMovement>().enabled = false;
        _thirdPersonCam.gameObject.SetActive(false);

        var ragdoll = _player.GetComponentInChildren<RagdollController>();
        ragdoll?.EnableRagdoll();

        // WinPanel
        _defeatPanelUI.gameObject.SetActive(true);
    }
}
