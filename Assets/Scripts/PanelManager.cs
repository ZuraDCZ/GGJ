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
    [SerializeField] public List<GameObject> pannels;
    [SerializeField] public List<GameObject> optionsList;

    public void OpenPausePannel()
    {
        mainPannel.SetActive(false);
        playerViewPannel.SetActive(true);

    }

    public void OpenNotesPannel()
    {
        playerViewPannel.SetActive(false);
        mainPannel.SetActive(false);
    }

    public void OpenVotingPannel()
    {
        mainPannel.SetActive(false);
    }

    public void BackToMainPannel()
    {
        playerViewPannel.SetActive(false);
        mainPannel.SetActive(true);
    }


    // Update is called once per frame
    public void SetPannelOn(int index)
    {
        pannels[index].SetActive(true);
        playerViewPannel.SetActive(false);
    }

    public void BackToPannel(int index)
    {
        pannels[index].SetActive(false);
        playerViewPannel.SetActive(true);
    }

    public void ClosePannel(int identifyer)
    {
        optionsList[identifyer].SetActive(false);
        optionsPannel.SetActive(false);
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
