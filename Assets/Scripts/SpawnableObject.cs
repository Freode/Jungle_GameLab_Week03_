using System.Collections;
using UnityEngine;

public enum SpawnType
{ 
    Resource = 0,
    Structure,
    Danger
}


public class SpawnableObject : MonoBehaviour
{
    public SpawnType spawnType;                 // ���� ������ ��ü�� Ÿ��
    public int interactByteValue = 0;           // ��ȣ�ۿ� ��, ȹ���� ����Ʈ�� ��

    public Vector3 baseScale;                   // ������ �Ǵ� ������ ũ��

    public float growthDuration = 0.5f;         // �����Ǵµ� �ɸ��� �ð�
    private bool canInteract = false;           // ���� �Ϸ� ��, ��ȣ�ۿ� �������� ����

    private void Awake()
    {
        
        transform.localScale = Vector3.zero;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnAnimate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ������ ��ü �����ϴ� �ִϸ��̼�
    IEnumerator SpawnAnimate()
    {
        float elapsedTime = 0f;
        Vector3 targetScale = Vector3.one;
        // �θ� �����Ϸ� ���ؼ� ������ ũ�Ⱑ ���ϴ� ���� ����
        if (transform.parent != null)
        {
            Vector3 parentScale = transform.parent.lossyScale;
            targetScale = new Vector3(
                baseScale.x / Mathf.Max(parentScale.x, float.Epsilon),
                baseScale.y / Mathf.Max(parentScale.y, float.Epsilon),
                baseScale.z / Mathf.Max(parentScale.z, float.Epsilon)
            );
        }

        // ������ Ű���
        while (elapsedTime < growthDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / growthDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        canInteract = true;

        // ���� ��Ұ� �ƴҶ��� ����
        if (spawnType == SpawnType.Danger)
            StartCoroutine(AfterSpawn());
    }

    // ���콺 Ŭ�� �Լ�
    private void OnMouseDown()
    {
        if (canInteract == false)
            return;

        // �ڿ��� ���, ����Ʈ ȹ��/�Ҿ������
        if(spawnType != SpawnType.Structure)
            GameManager.instance.AddByteValue(interactByteValue);
              
        Destroy(gameObject);
    }

    IEnumerator AfterSpawn()
    {
        float curWaitTime = 0f;
        float limitWaitTime = 10f;

        // ���� ��Ҵ� ���� �ð��� ���� �Ŀ� �ڵ����� ����
        while(curWaitTime < limitWaitTime)
        {
            curWaitTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
