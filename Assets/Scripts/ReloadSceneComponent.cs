using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneComponent : MonoBehaviour
{
    public void ReloadScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
