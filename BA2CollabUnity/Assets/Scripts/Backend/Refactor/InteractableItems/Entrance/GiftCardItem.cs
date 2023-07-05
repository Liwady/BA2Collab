using DG.Tweening;
using UnityEngine;

public class GiftCardItem : InteractableItem
{
    [Header("positions")]
    [SerializeField] protected Transform initPos;
    [SerializeField] protected Transform activePos;
    public GameObject shelfDoor;
    public string giftCardMemory;

    [Header("object")]
    [SerializeField] private GameObject giftCardUI;

    [Header("Duration")]
    [SerializeField] private float noteMovementDuration;


    public override void Collect()
    {
        //closes HUD when activating the puzzle 
        interactParticle.SetActive(false);
        InstantiateAndMove();
        Player.instance.SetCanMove(false);
        Player.instance.animator.SetBool("isMoving", false);
        SetIsComplete(true);
    }

    public void InstantiateAndMove()
    {
        giftCardUI.SetActive(true);
        giftCardUI.transform.DOMove(activePos.position, noteMovementDuration).OnComplete(() =>
        {
            UIManager.instance.dialogues.StartDialogue(giftCardMemory);
        });
    }

    public void OpenShelfDoor()
    {
        shelfDoor.transform.DOLocalRotate(new Vector3(0, 0, 0), noteMovementDuration).OnComplete(() => Destroy(this));
    }

    public void MoveItemAway()
    {
        giftCardUI.transform.DOMove(initPos.position, noteMovementDuration).OnComplete(() =>
        {
            giftCardUI.SetActive(false);
            OpenShelfDoor();
        });
    }
}
