using UnityEngine;

// 바이트 최대 수용량을 증가하는 효과
[CreateAssetMenu(fileName = "NewAddMaxByteValue", menuName = "Tech Tree/Effects/Max Byte")]
public class AddMaxByteValueTechEffect : BaseTechEffect
{
    public int addMaxByteValue = 0;

    public override void ApplyTechEffect()
    {
        GameManager.instance.AddMaxByteValue(addMaxByteValue);
    }
}
