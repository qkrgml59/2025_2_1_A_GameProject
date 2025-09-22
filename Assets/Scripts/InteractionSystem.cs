using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("상호 작용 설정")]
    public float interactionRange = 2.0f;                         //상호 작용 범위
    public LayerMask interactionLayerMask = 1;                   //상호 작용 할 레이어
    public KeyCode interactionKey = KeyCode.E;                   //상호 작용 키 (E 키)

    [Header("UI 설정")]
    public Text interactionText;                               //상호작용 UI 텍스트
    public GameObject interactionUI;                           //상호작용 UI 패널

    private Transform playerTransform;
    private InteractableObject currentInteractable;            //감지된 오브젝트 담는 클래스

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        HideInteractionUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
        HandleInteractionInput();
        
    }

    void CheckForInteractables()
    {
        Vector3 checkPosition = playerTransform.position + playerTransform.forward * (interactionRange * 0.5f);             //플레이어 앞쪽 방향으로 체크

        Collider[] hitColliders = Physics.OverlapSphere(checkPosition, interactionRange, interactionLayerMask);

        InteractableObject closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in hitColliders)
        {
            InteractableObject interactabel = collider.GetComponent<InteractableObject>();
            if(interactabel != null)
            {
                float distance = Vector3.Distance(playerTransform.position, collider.transform.position);

                Vector3 directionToObject = (collider.transform.position - playerTransform.position) .normalized;
                float angle = Vector3.Angle(playerTransform.forward, directionToObject);

                if (angle < 90f && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactabel;
                }
            }
        }

        if (closestInteractable != currentInteractable)
        {
            if(currentInteractable != null)
            {
                currentInteractable.OnPlayerExit();
            }

            currentInteractable = closestInteractable;

            if(currentInteractable != null)
            {
                currentInteractable.OnPlayerEnter();
                ShowInteractionUI(currentInteractable.GetInteractionText());
            }
            else
            {
                HideInteractionUI();
            }
        }

    }

    void HandleInteractionInput()                                                       //인터랙션 입력 함수
    {
        if (currentInteractable != null && Input.GetKeyDown(interactionKey))           //인터렉션 키 값 눌렀을 때
        {
            currentInteractable.Interact();                                            //행동 진행
        }
    }

    void ShowInteractionUI(string text)                                                   //인터렉션 창을 연다
    {
        if (interactionUI !=null)
        {
            interactionUI.SetActive(true);
        }

        if(interactionText !=null)
        {
            interactionText.text = text;
        }
    }

    void HideInteractionUI()
    {
        if(interactionUI !=null)
        {
            interactionUI.SetActive(false);
        }
    }
}
