using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*public class Recipe : ScriptableObject
{
    public GameObject completion;           // ����
    public List<GameObject> items;          // �ʿ��� ���µ�?
}*/

public class testcode : MonoBehaviour
{
   // HashSet<Recipe> recipes = new HashSet<Recipe>();

    private void Start()
    {
      //  recipes.Add(new Recipe());
    }


    public static testcode instance;

    public static testcode Instance { get { return instance; } }

    public Button button;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            dd();
            Debug.Log("tlqk");
        }

        if (Input.GetKey(KeyCode.F))
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    GameObject a;

    private void dd()
    {
     /*   var scripts = a.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script.GetType().IsSubclassOf(typeof(Ingredient)))
            {
                // ���ϴ� ó���� �����մϴ�.
            }
        }*/

    }
}