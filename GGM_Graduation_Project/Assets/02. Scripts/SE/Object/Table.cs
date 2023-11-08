using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Table : MonoBehaviour, IObject
{
    private bool interactive = false;
    public bool Interactive { get { return interactive; } set { interactive = value; } }
    private bool is_existObject = false;
    public bool Is_existObject
    {
        get { return is_existObject; }
        set
        {
            is_existObject = value;
        }
    }

    private PlayerFOV playerFOV;
    private PlayerInteraction playerInteraction;

    public GameObject Interaction(GameObject ingredient = null)
    {
        if (interactive)
        {
            if (!is_existObject)
            {
                ingredient.transform.parent = transform;
                ingredient.transform.localPosition = new Vector3(0, 0.5f, 0);
                playerInteraction.CurrentObjectInHand = null;
                is_existObject = true;   
                playerInteraction.Is_Object = false;
                Debug.Log("테이블에 넣어짐");
            }
            else
            {
                if (playerInteraction.CurrentObjectInHand == null)
                {
                    Debug.Log("테이블에서 다시 돌려줌.");
                    is_existObject = false;     // 이제 손에 없어
                    playerInteraction.Is_Object = true;
                    return gameObject.transform.GetChild(0).gameObject;
                }
                // 플레이어 손에 돌려주기
            }
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("F키로 상호작용이 가능해요!");
            // 상호작용 가능 표시해주기
            if (playerFOV == null)
            {
                playerFOV = other.gameObject.GetComponent<PlayerFOV>();
                playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
            }
            if (playerFOV.CheckForObjectsInView().name == gameObject.transform.name)
            {
                 interactive = true;
                if (!is_existObject)        // 손에 안가지고 있을 때만 작동
                {
                    playerInteraction.Is_Object = true;
                    //Debug.Log("플레이어 인터랙션 시 테이블에 재료를 올려놓아요.");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //상호작용 가능 표시해주기
            if (!is_existObject)
            {
                playerInteraction.Is_Object = false;
            }
            interactive = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //if (UnityEditor.Selection.activeObject == gameObject)
        //{
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, 1);
        Gizmos.color = Color.green;
        //}
    }
#endif
}
