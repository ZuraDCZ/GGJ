using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [SerializeField] public GameObject mainPannel;
    [SerializeField] public GameObject playerViewPannel;
    [SerializeField] public GameObject optionsPannel;

    public void OpenPlayerView()
    {
        mainPannel.SetActive(false);
        playerViewPannel.SetActive(true);

    }

    public void OptionsPannel()
    {
        playerViewPannel.SetActive(false);
        mainPannel.SetActive(false);
        optionsPannel.SetActive(true);
    }

    public void DisactivetePlayerView()
    {
        mainPannel.SetActive(false);
    }

    public void OpenPausePannel()
    {
        playerViewPannel.SetActive(false);
        mainPannel.SetActive(true);
    }

    public void closingApp()
    {
        Application.Quit();
        Debug.Log("Se cerro la app");
    }
    void Update()
    {

    }
}
