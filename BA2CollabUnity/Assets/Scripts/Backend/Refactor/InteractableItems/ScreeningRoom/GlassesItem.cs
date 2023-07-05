using DG.Tweening;
using UnityEngine;

public class GlassesItem : InteractableItem
{
    public GlassesManager glasses;
    [SerializeField] private GameObject glassesModel;

    public override void Collect()
    {
        interactParticle.SetActive(false);
        Player.instance.SetCanMove(false);
        Player.instance.animator.SetBool("isMoving", false);
        ActivateGlassesOnCamera();
        SetIsComplete(true);
    }

    public void ActivateGlassesOnCamera()
    {
        glassesModel.SetActive(true);
        Player.instance.hasGlasses = true;
        glassesModel.transform.DOLocalMove(new Vector3(0, -1, 3), 1).OnComplete(() =>
        {
            glassesModel.transform.DOLocalMove(new Vector3(0, -1, 0.16f), 1).OnComplete(() =>
            {
                glassesModel.transform.DOLocalMove(new Vector3(0, -1, -1), 1);
                glasses.gameObject.SetActive(true);
                glasses.InitializeGlasses();
            });
        });
    }

}


