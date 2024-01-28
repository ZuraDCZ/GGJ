using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] Animator m_transition;
    [SerializeField] Animator m_optionsPanel;
    [SerializeField] float m_transitionTime = 1f;

    public void LoadMainGameScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    } 

    public void LoadOptions()
    {
        m_optionsPanel.SetBool("Enter", true);
    }

    public void ExitOptions()
    {
        m_optionsPanel.SetBool("Enter", false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int p_levelIndex)
    {
        m_transition.SetTrigger("Start");
        yield return new WaitForSeconds(m_transitionTime);
        SceneManager.LoadScene(p_levelIndex);
    }
}