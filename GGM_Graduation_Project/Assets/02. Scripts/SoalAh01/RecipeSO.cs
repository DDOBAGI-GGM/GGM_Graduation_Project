using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//public struct OBJ
//{
//    List<ObjectType> processed;
//    List<DvcType> device;
//    List<GameObject> image;
//}

[System.Serializable]
[CreateAssetMenu(menuName = "SO/Recipe", fileName = "Recipe_Data")]
public class RecipeSO : ScriptableObject
{
    public GameObject obj;
    public ObjectType objType;
    public List<RecipeSO> source;
    public DvcType device;
    // �̹���
    // ������Ʈ (���� ���, �߰�,,... �� �´� ���� �ⱸ, �̹���)
}