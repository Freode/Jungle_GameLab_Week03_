using UnityEngine;
using UnityEngine.EventSystems;

public class DemonstartionInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    public string context;

    private RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("버튼에 마우스 올리기");
        GameManager.instance.ActiveDemonstration(this, rectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.InactiveDemonstration();
    }
}
