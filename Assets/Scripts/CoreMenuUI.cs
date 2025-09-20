using UnityEngine;
using UnityEngine.UI;

public class CoreMenuUI : MonoBehaviour
{
    public Button ButtonExit;
    public Image imageBlock;

    private void Start()
    {
        ButtonExit.onClick.AddListener(OnInactiveUI);
    }

    // 활성화
    public void OnActiveUI()
    {
        Time.timeScale = 0f;
        imageBlock.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    // 비활성화
    public void OnInactiveUI()
    {
        Time.timeScale = 1f;
        imageBlock.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
