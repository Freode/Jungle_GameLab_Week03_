using TMPro;
using UnityEngine;

public class DemonstrationUI : MonoBehaviour
{
    public RectTransform uiRectTransform;
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textContent;

    private void Start()
    {
        GameManager.instance.OnDemonstrationActive += SetActiveInfo;
        GameManager.instance.OnDemonstrationInactive += SetInactiveInfo;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.instance.OnDemonstrationActive -= SetActiveInfo;
        GameManager.instance.OnDemonstrationInactive -= SetInactiveInfo;
    }

    // 활성화
    public void SetActiveInfo(DemonstartionInfo info, RectTransform baseTransform)
    {
        textTitle.text = info.title;
        textContent.text = info.context;
        SetUIPosition(baseTransform);

        gameObject.SetActive(true);
    }

    // 비활성화
    public void SetInactiveInfo()
    {
        gameObject.SetActive(false);
    }

    // 설명 UI 위치 설정
    private void SetUIPosition(RectTransform baseTransform)
    {
        Vector3[] buttonCorners = new Vector3[4];

        RectTransform rectTransform = GetComponent<RectTransform>();

        // 현재 이미지의 높이와 너비
        float height = rectTransform.rect.height;
        float weight = rectTransform.rect.width;

        // 기준이 되는 버튼의 모서리 위치
        baseTransform.GetWorldCorners(buttonCorners);

        // 화면 크기
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float rightX = buttonCorners[2].x + weight;
        float downY = buttonCorners[2].y + height;

        bool rightOver = rightX > screenWidth;
        bool downOver = downY > screenHeight;

        Vector3 resultPos = Vector3.zero;
        // 1. 오른쪽 상단을 맞춰서 배치 - 오른쪽 경계 x, 아래쪽 경계 x
        if(rightOver == false &&  downOver == false)
        {
            resultPos.x = buttonCorners[2].x + weight / 2f;
            resultPos.y = buttonCorners[2].y - height / 2f;
        }

        // 2. 오른쪽 하단을 맞춰서 배치 - 오른쪽 경계 x, 아래쪽 경계 o
        else if(rightOver == false && downOver == true)
        {
            resultPos.x = buttonCorners[2].x + weight / 2f;
            resultPos.y = buttonCorners[3].y + height / 2f;
        }

        // 3. 왼쪽 상단을 맞춰서 배치 - 오른쪽 경계 o, 아래쪽 경계 x
        else if (rightOver == true && downOver == false)
        {
            resultPos.x = buttonCorners[1].x - weight / 2f;
            resultPos.y = buttonCorners[2].y - height / 2f;
        }

        // 4. 왼쪽 하단을 맞춰서 배치 - 오른쪽 경계 o, 아래쪽 경계 o
        else
        {
            resultPos.x = buttonCorners[1].x - weight / 2f;
            resultPos.y = buttonCorners[0].y + height / 2f;
        }

        uiRectTransform.position = resultPos;
    }
}
