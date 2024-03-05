using UnityEngine;

public class Table : MonoBehaviour, IObject
{
    [SerializeField] private Vector3 tableObjectPosition = new Vector3(0, 0.5f, 0);

    private bool is_existObject = false;            // 지금 오브젝트가 보관중이니?
    public bool Is_existObject { get { return is_existObject; } }

    public GameObject Interaction(GameObject ingredient = null)
    {
        SoundManager.Instance.PlaySFX("get");
        if (!is_existObject)
        {
            if (ingredient != null)
            {
                ingredient.transform.parent = transform;
                ingredient.transform.localPosition = tableObjectPosition;
                is_existObject = true;
                Debug.Log("테이블에 넣어짐");
            }
        }
        else
        {
                Debug.Log("테이블에서 다시 돌려줌.");
                is_existObject = false;     // 이제 손에 없어
                return gameObject.transform.GetChild(0).gameObject;
            // 플레이어 손에 돌려주기
        }
        return null;
    }
}
