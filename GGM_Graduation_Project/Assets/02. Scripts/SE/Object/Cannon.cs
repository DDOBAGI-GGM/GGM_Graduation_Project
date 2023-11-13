using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class Cannon : MonoBehaviour, IObject
{
    [SerializeField] private Transform floorPos;
    [SerializeField] private Transform enemyPos;
    [SerializeField] private Transform objectPos;

    public GameObject Interaction(GameObject ingredient)
    {
        // ������� ���߰�, �߻縦 ������ ��������� ����.
        if (ingredient != null)
        {
            ingredient.transform.parent = transform;            // �ڽ����� ������ �ȳ�����
            ingredient.transform.localPosition = new Vector3(0, 0.5f, 0);
            ingredient.transform.parent = null;
            Debug.Log("����߻� �����ڰ�!");

            string type = ingredient.gameObject.name.Substring(ingredient.gameObject.name.IndexOf('-') + 1);
            Debug.Log(type);
            switch (type)
            {
                case "Floor":
                    Attack(ingredient, floorPos.position);
                    break;
                case "Object":
                    Attack(ingredient, objectPos.position);
                    break;
                case "Enemy":
                    Attack(ingredient, enemyPos.position);
                    break;
                default:
                    Debug.Log("�ùٸ� ���� ������ �ƴϿ��� �������!");
                    // Ǯ������ϱ�?
                    Destroy(ingredient);
                    break;
            }
        }
        return null;
    }

    private void Attack(GameObject weapon, Vector3 pos)
    {
        weapon.transform.DOMove(pos, 2f);
    }
}
