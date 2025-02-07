using DG.Tweening;
using Highlighters;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public string associatedMemory;
    public bool isIntaractable;
    public List<Leaf> leafList = new();
    public static PlantManager instance = null;
    public Leaf currentLeaf;
    public bool isComplete;
    public bool canTurn;
    public float turnSpeed;
    public Highlighter highlighter;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        isIntaractable = false;
    }

    public void InitializePlantPuzzle()
    {
        isIntaractable = true;
        highlighter.enabled = true;
        canTurn = true;
        transform.DOScale(new Vector3(0.15f, 0.15f, 0.15f), 2);
    }

    private void Update()
    {
        if (canTurn)
            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * turnSpeed);

    }

    public void CallCheck()
    {
        if (PlantIsClean() && isIntaractable)
        {
            LightManager.instance.OpenDialogBoxLight(false);
            isIntaractable = false;
            UIManager.instance.dialogues.StartDialogue(associatedMemory);
            isComplete = true;
        }
    }

    private bool PlantIsClean()
    {
        foreach (Leaf v in leafList)
            if (!v.removed) return false;
        return true;

    }
    public bool IsComplete()
    {
        return isComplete;
    }

    internal void Activate()
    {
        isIntaractable = true;
    }
}
