using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;

    [SerializeField] Animator animator;

    [SerializeField] DialogueTrigger dialogueTrigger;

    private List<string> listDialogues;

    private float maxTime = 10f;
    public float currentTime;
    private int randomIndex;

    void Start()
    {
        listDialogues = new List<string>();

        currentTime = maxTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            dialogueTrigger.TriggerDialogue();
            StartCoroutine(CloseCloud());
        }
    }

    public void StartTalking(ObjectDialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        foreach (string dial in dialogue.sentences)
        {
            listDialogues.Add(dial);
        }

        dialogueText.text = listDialogues[randomIndex];
    }

    IEnumerator CloseCloud()
    {
        yield return new WaitForSeconds(5f);
        currentTime = maxTime;
        randomIndex = Random.Range(0, listDialogues.Count - 1);
        EndDialogue();
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
