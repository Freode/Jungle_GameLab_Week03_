using UnityEngine;

// 글라시오로 이동 가능 여부
[CreateAssetMenu(fileName = "NewCanGoGlaco", menuName = "Tech Tree/Effects/Can Go Glacio")]
public class SetCanGoGlacloTechEffect : BaseTechEffect
{
    public override void ApplyTechEffect()
    {
        GameManager.instance.SetCanGoGlaclo();
    }
}
