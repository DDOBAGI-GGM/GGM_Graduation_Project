using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cannon : MonoBehaviour, IObject
{
    [Header("Floor")]
    [SerializeField] private Transform[] floorPos = new Transform[7];
    [Header("Enemy")]
    [SerializeField] private Transform[] enemyPos = new Transform[1];
    [Header("Object")]
    [SerializeField] private Transform[] objectPos = new Transform[7];   

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
            ingredient.transform.parent = transform;   
            ingredient.transform.localPosition = new Vector3(0, 0.5f, 0);
            ingredient.transform.parent = null;
            SoundManager.Instance.PlaySFX("get");

            string type = ingredient.gameObject.name.Substring(ingredient.gameObject.name.LastIndexOf('_') + 1);
            switch (type)
            {
                case "Floor":
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
                    Destroy(ingredient);
                    break;
            }
        }
        return null;
    }

    private void Attack(GameObject weapon, Transform[] pos)
    {
        weapon.AddComponent<Rigidbody>();
        attackCurve.MakeCurve(weapon, pos);
    }
}
