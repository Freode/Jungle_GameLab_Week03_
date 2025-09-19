using UnityEngine;

// 바이트 발견 시, 발견량을 최대 n% 증가
[CreateAssetMenu(fileName = "NewFindBytesRate", menuName = "Tech Tree/Effects/Find Bytes Rate")]
public class AddFindBytesRateTechEffect : BaseTechEffect
{
    public int increaseRate = 0;

    public override void ApplyTechEffect()
    {
        GameManager.instance.AddFindBytesRate(increaseRate);
    }
}
