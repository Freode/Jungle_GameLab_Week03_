using UnityEngine;

// 데솔로로 이동 가능 여부
[CreateAssetMenu(fileName = "NewSetCanGoDesolo", menuName = "Tech Tree/Effects/Can Go Desolo")]
public class SetCanGoDesoloTechEffect : BaseTechEffect
{
    public override void ApplyTechEffect()
    {
        GameManager.instance.SetCanGoDesolo();
    }
}
