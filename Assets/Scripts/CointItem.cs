using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CointItem : InteractableObject
{
    [Header("동전 설정")]
    public int coinValue = 10;
    public string questTag = "Coin";                      //퀘스트에서 사용할 태그


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        objectName = "동전";
       interactionText = "[E] 동전 획득";
        interactionType = InteractionType.Item;
    }

    protected override void CollectItem()
    {

        //퀘스트 매니저에 수집을 알림
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.AddCollectProgress(questTag);
        }

        AchievementManager.instance?.UpdateProgress(AchievementType.CollectCoins, coinValue);
        transform.Rotate(Vector3.up * 360f);
        Destroy(gameObject, 0.5f);
    }
}

