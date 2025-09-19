using UnityEngine;

// 바이트 감소량을 커버하는 숫자를 추가하는 효과
[CreateAssetMenu(fileName = "NewAddReduceBytesCover", menuName = "Tech Tree/Effects/Recover Bytes Cover")]
public class AddReduceBytesCoverValueTechEffect : BaseTechEffect
{
    public int addReduceBytesCoverValue = 0;

    public override void ApplyTechEffect()
    {
        GameManager.instance.AddReduceBytesCoverValue(addReduceBytesCoverValue);
    }
}
