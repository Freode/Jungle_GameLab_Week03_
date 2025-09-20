using UnityEngine;

// 코어 구매
[CreateAssetMenu(fileName = "NewPurchaseCore", menuName = "Tech Tree/Effects/Core Purchase")]
public class PurchaseCoreEffect : BaseTechEffect
{
    public Planet planet;

    public override void ApplyTechEffect()
    {
        GameManager.instance.PurchaseCore(planet);
    }
}
