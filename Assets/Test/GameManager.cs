using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnWin()
    {
        Debug.Log("������!");
        var main = SceneManager.GetActiveScene();
        SceneManager.LoadScene(main.name);
    }
}
