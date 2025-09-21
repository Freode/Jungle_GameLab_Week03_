using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 클리어 여부에 따라 화면 출력
public class ClearUI : MonoBehaviour
{
    public TextMeshProUGUI textTitle;
    public Button ButtonRestart;
    public Button ButtonQuit;

    void Start()
    {
        ButtonRestart.onClick.AddListener(GameManager.instance.GameRestart);
        ButtonQuit.onClick.AddListener(GameManager.instance.GameQuit);
        GameManager.instance.OnGameClear += SetClear;
        gameObject.SetActive(false);
    }

    public void SetClear(bool isClear)
    {
        gameObject.SetActive(true);
        textTitle.text = isClear ? "클리어!" : "실패!";
    }
}
