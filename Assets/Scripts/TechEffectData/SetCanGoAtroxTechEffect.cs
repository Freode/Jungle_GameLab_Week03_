using UnityEngine;

// 아트록스로 이동 가능 여부
[CreateAssetMenu(fileName = "NewCanGoAtrox", menuName = "Tech Tree/Effects/Can Go Atrox")]
public class SetCanGoAtroxTechEffect : BaseTechEffect
{
    public override void ApplyTechEffect()
    {
        GameManager.instance.SetCanGoAtrox();
    }
}
