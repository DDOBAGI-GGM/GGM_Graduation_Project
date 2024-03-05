using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class Cannon : MonoBehaviour, IObject
{
    [Header("Floor")]
    [SerializeField] private Transform[] floorPos = new Transform[4];
    [Header("Enemy")]
    [SerializeField] private Transform[] enemyPos = new Transform[4];
    [Header("Object")]
    [SerializeField] private Transform[] objectPos = new Transform[4];         // 4�� ������ ����

    private AttackCurve attackCurve;

    private void Awake()
    {
        attackCurve = GetComponent<AttackCurve>();
    }

    public GameObject Interaction(GameObject ingredient)
    {
        // ������� ���߰�, �߻縦 ������ ��������� ����.
        if (ingredient != null)
        {
            ingredient.transform.parent = transform;            // �ڽ����� ������ �ȳ�����
            ingredient.transform.localPosition = new Vector3(0, 0.5f, 0);
            ingredient.transform.parent = null;
            SoundManager.Instance.PlaySFX("get");
            //Debug.Log("����߻� �����ڰ�!");

            string type = ingredient.gameObject.name.Substring(ingredient.gameObject.name.IndexOf('-') + 1);
            Debug.Log(type);
            switch (type)
            {
                case "Floor":
                    //Debug.Log("�÷ξ� ���ݽ���!");
                    Attack(ingredient, floorPos);
                    break;
                case "Object":
                    Attack(ingredient, objectPos);
                    break;
                case "Enemy":
                    Attack(ingredient, enemyPos);
                    break;
                case "Recovery":
                    Debug.Log("ȸ���� ����.");
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

    private void Attack(GameObject weapon, Transform[] pos)
    {
        attackCurve.MakeCurve(weapon, pos);
    }
}
