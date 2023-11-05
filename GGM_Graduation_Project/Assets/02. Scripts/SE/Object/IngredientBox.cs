using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBox : MonoBehaviour, IObject
{
    [SerializeField] private GameObject giveIngredientPrefab;
    private bool interactive = false;
    Player player;

    public void Interaction(GameObject ingredient = null)
    {
        if (interactive)
        {
            // ���ͷ��� �ڵ� �ۼ�
            // ������Ʈ Ǯ�� ����ϱ�?
            Instantiate(giveIngredientPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            // ��ȣ�ۿ� ���� ǥ�����ֱ�
            if (player == null)
            {
                player = other.gameObject.GetComponent<Player>();
            }
            interactive = true;
        }
            Debug.Log("FŰ�� ��ȣ�ۿ��� �����ؿ�!");
    }

    private void Update()
    {
        if (interactive)
        {
            // ���ͷ��� Ű��
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            //��ȣ�ۿ� ���� ǥ�����ֱ�
            interactive = false;
        }
    }
}

       // gameObject.GetComponent<IObject>().Interaction();