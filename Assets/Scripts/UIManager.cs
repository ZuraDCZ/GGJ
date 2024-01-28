using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    //Event to update score change
    public delegate void OnScore();
    public OnScore onScore;

    //Event to detect lifes change
    public delegate void OnLifeLost();
    public OnLifeLost onLifeLost;

    [Header("Score tracker")]
    [SerializeField] TextMeshProUGUI scoreText;
    
    [Header("Lifes Tracker")]
    [SerializeField] RectTransform lifesHolder;
    [SerializeField] GameObject lifeHolderPrefab;
    [SerializeField] Sprite LostLifeSprite;
    private List<GameObject> currentLifesSprites = new List<GameObject>();


    private void Awake()
    {
        //Set singleton
        if (GameObject.Find("UIManager") == gameObject)
            instance = this;
        else
            Destroy(gameObject);

        scoreText.text = "Score: " + LvlManager.instance.GetScore();
    }

    private void Start()
    {
        InitializeLifes();
    }

    private void Update()
    {
        UpdateScore();
    }

    private void InitializeLifes()
    {
        for (int i = 0; i < LvlManager.instance.GetMaxLifes(); i++)
        {
            GameObject newLife = Instantiate(lifeHolderPrefab, lifesHolder);
            currentLifesSprites.Add(newLife);
        }
    }

    private void UpdateScore()
    {
        float newScore = Mathf.FloorToInt(LvlManager.instance.GetScore());
        scoreText.text = "Score: " + newScore;
    }

    public void UpdateLifes()
    {
        if (LvlManager.instance.GetCurrentLifes() > 0)
        {
            currentLifesSprites[LvlManager.instance.GetCurrentLifes()].GetComponent<Image>().sprite = LostLifeSprite;
        }
    }

    public List<GameObject> GetLifeSprites()
    {
        return currentLifesSprites;
    }
}
