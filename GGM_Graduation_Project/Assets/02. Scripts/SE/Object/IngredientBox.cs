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
            // 인터랙션 코드 작성
            // 오브젝트 풀링 사용하기?
            Instantiate(giveIngredientPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            // 상호작용 가능 표시해주기
            if (player == null)
            {
                player = other.gameObject.GetComponent<Player>();
            }
            interactive = true;
        }
            Debug.Log("F키로 상호작용이 가능해요!");
    }

    private void Update()
    {
        if (interactive)
        {
            // 인터랙터 키기
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            //상호작용 가능 표시해주기
            interactive = false;
        }
    }
}

       // gameObject.GetComponent<IObject>().Interaction();