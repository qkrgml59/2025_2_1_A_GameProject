using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemData item;
    public int amount;             //아이템 개수

    [Header("UI References")]
    public Image itemIcon;         //아이템 아이콘
    public Text amountText;        //개수 텍스트
    public GameObject emptySlotImage;        //빈 슬롯일 때 보여줄 이미지

    // Start is called before the first frame update
    void Start()
    {
        UpdateSloutUI();
    }

    //슬롯에 아이템 설정하는 함수
    public void SetItem(ItemData newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
        UpdateSloutUI();
    }


    //아이템 개수 추가하는 함수

    public void AddAmount(int value)
    {
        amount += value;
        UpdateSloutUI();
    }

    public void RemoveAmount(int value)
    {
        amount -= value;

        //개수가 0이하면 슬롯 비우기
        if(amount <= 0)
        {
            ClearSlot();
        }
        else
        {
            UpdateSloutUI();
        }
    }

    public void ClearSlot()                    //슬롯을 비우는 함수
    {
        item = null;
        amount = 0;
        UpdateSloutUI();
    }

    //UI를 업데이트 하는 함수
    void UpdateSloutUI()
    {
        if (item != null)
        {
            itemIcon.sprite = item.itemIcon;    //아이템 아이콘 표시
            itemIcon.enabled = true;

            amountText.text = amount > 1 ? amount.ToString() : "";          //개수가 1보다 많으면 숫자 표시
                if(emptySlotImage != null)                        //빈 슬롯 이미지 숨기기
                {
                   emptySlotImage.SetActive(false);
                }
        }
        else
        {
            itemIcon.enabled = false;                     
            amountText.text = "";
            if (emptySlotImage != null)
            {
                emptySlotImage.SetActive(true);
            }
        }
    }
}
