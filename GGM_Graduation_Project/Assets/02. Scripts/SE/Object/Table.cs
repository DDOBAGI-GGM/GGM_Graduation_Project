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
                Debug.Log("���̺� �־���");
            }
            else
            {
                if (playerInteraction.CurrentObjectInHand == null)
                {
                    Debug.Log("���̺��� �ٽ� ������.");
                    is_existObject = false;     // ���� �տ� ����
                    playerInteraction.Is_Object = true;
                    return gameObject.transform.GetChild(0).gameObject;
                }
                // �÷��̾� �տ� �����ֱ�
            }
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("FŰ�� ��ȣ�ۿ��� �����ؿ�!");
            // ��ȣ�ۿ� ���� ǥ�����ֱ�
            if (playerFOV == null)
            {
                playerFOV = other.gameObject.GetComponent<PlayerFOV>();
                playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
            }
            if (playerFOV.CheckForObjectsInView().name == gameObject.transform.name)
            {
                 interactive = true;
                if (!is_existObject)        // �տ� �Ȱ����� ���� ���� �۵�
                {
                    playerInteraction.Is_Object = true;
                    //Debug.Log("�÷��̾� ���ͷ��� �� ���̺� ��Ḧ �÷����ƿ�.");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //��ȣ�ۿ� ���� ǥ�����ֱ�
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
