using UnityEngine;

// 바이트 최소 발견 주기
[CreateAssetMenu(fileName = "NewFindByteMinPeriod", menuName = "Tech Tree/Effects/Find Byte Min Period")]
public class AddFindByteMinPeriodTechEffect : BaseTechEffect
{
    public float increaseMinPeriodValue = 0f;

    public override void ApplyTechEffect()
    {
        GameManager.instance.AddFindByteMinPeriod(increaseMinPeriodValue);
    }
}
