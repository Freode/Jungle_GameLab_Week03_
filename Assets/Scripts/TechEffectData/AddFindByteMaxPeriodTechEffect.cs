using UnityEngine;

// 바이트 최대 발견 주기
[CreateAssetMenu(fileName = "NewFindByteMaxPeriod", menuName = "Tech Tree/Effects/Find Byte Max Period")]
public class AddFindByteMaxPeriodTechEffect : BaseTechEffect
{
    public float increaseMaxPeriodValue = 0f;

    public override void ApplyTechEffect()
    {
        GameManager.instance.AddFindByteMaxPeriod(increaseMaxPeriodValue);
    }
}
