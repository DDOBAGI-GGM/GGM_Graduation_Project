using UnityEngine;

public class Table : MonoBehaviour, IObject
{
    private bool is_existObject = false;            // ���� ������Ʈ�� �������̴�?

    public GameObject Interaction(GameObject ingredient = null)
    {
        if (!is_existObject)
        {
            if (ingredient != null)
            {
                ingredient.transform.parent = transform;
                ingredient.transform.localPosition = new Vector3(0, 0.5f, 0);
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
