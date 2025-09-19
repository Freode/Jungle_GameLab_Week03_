using UnityEngine;

// 바이트 최소 발견량 증가
[CreateAssetMenu(fileName = "NewIncreaseFindByteMinValue", menuName = "Tech Tree/Effects/Increase Find Byte Min Value")]
public class AddIncreaseFindByteMinValueTechEffect : BaseTechEffect
{
    public int increaseFindByteMinValue = 0;

    public override void ApplyTechEffect()
    {
        GameManager.instance.AddIncreaseFindByteMinValue(increaseFindByteMinValue);
    }
}
