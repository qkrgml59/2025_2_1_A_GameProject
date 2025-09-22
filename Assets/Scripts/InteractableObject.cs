using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("��ȣ�ۿ� ����")]
    public string objectName = "������";
    public string interactionText = "[E] ��ȣ�ۿ�";
    public InteractionType interactionType = InteractionType.Item;                 //Ÿ���� �켱 Item

    [Header("���̶���Ʈ ����")]
    public Color highlightColor = Color.yellow;
    public float highlightintensity = 1.5f;

    public Renderer objectRenderer;
    private Color orignalColor;
    private bool isHighlighted = false;


    public enum InteractionType
    {
        Item,                      //������ ( ����, ���� �� )
        Machine,                   //��� ( ����, ��ư �� )
        Building,                  //�ǹ� ( ��, ���� �� )
        NPC,                       //NPC
        Collectible                //����ǰ
      
    }

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer !=null)
        {
            orignalColor = objectRenderer.material.color;
        }

        gameObject.layer = 8;
    }

    public virtual void OnPlayerEnter()
    {
        Debug.Log($"({objectName}) ������");
        HighlightObject();
    }

    public virtual void OnPlayerExit()
    {
        Debug.Log($"({objectName}) ������");
        HighlightObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interact()
    {
        switch (interactionType)
        {
            case InteractionType.Item:
                CollectItem();
                break;

            case InteractionType.Machine:
                OperateMachine();
                break;
            case InteractionType.Building:
                AccessBuilding();
                break;
            case InteractionType.Collectible:
                CollectItem();
                break;

        }
    }

    public string GetInteractionText()
    {
        return interactionText;
    }


    protected virtual void HighlightObject()
    {
        if (objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Enission", highlightintensity);
            isHighlighted = true;
        }
    }

    protected virtual void RemoveHighlight()
    {
        if (objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Emission", 0f);
            isHighlighted = false;
        }
    }

    protected virtual void CollectItem()
    {
        Debug.Log($"{objectName}��(��) ȹ���߽��ϴ�!");
        Destroy(gameObject);
    }

    protected virtual void OperateMachine()
    {
        Debug.Log($"{objectName}��(��) �۵����׽��ϴ�!");
        if(objectRenderer !=null)
        {
            objectRenderer.material.color = Color.green;

        }

    }

    protected virtual void AccessBuilding()
    {
        Debug.Log($"{objectName}��(��) �� �����߽��ϴ�.");
        transform.Rotate(Vector3.up * 90f);
     
    }

    protected virtual void TalkToNPC()
    {
        Debug.Log($"{objectName}�� ��ȭ�� �����մϴ�.");

    }

}
