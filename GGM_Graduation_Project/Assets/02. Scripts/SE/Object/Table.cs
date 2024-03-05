using UnityEngine;

public class Table : MonoBehaviour, IObject
{
    [SerializeField] private Vector3 tableObjectPosition = new Vector3(0, 0.5f, 0);

    private bool is_existObject = false;            // ���� ������Ʈ�� �������̴�?
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
                Debug.Log("���̺� �־���");
            }
        }
        else
        {
                Debug.Log("���̺��� �ٽ� ������.");
                is_existObject = false;     // ���� �տ� ����
                return gameObject.transform.GetChild(0).gameObject;
            // �÷��̾� �տ� �����ֱ�
        }
        return null;
    }
}
