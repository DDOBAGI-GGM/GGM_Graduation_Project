using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class AI : MonoBehaviour
{
    [Header("Attribute")]
    public AIManager manager;
    public BehaviourTreeManager<INode> bt;

    [Header("Component")]
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Recipe")]
    //public RecipeListSO oldRecipe;
    //public int oldRecipeIdx;
    public RECIPE recipe;
    public RECIPE oldRecipe;
    //public int recipeIdx;

    // 砧 鯵 戚雌税 傾獣杷研 佐楳拝 凶 (呪軒 雌殿 薦須) - hard 乞球拭辞 鯵降馬切...
    //public List<RECIPE> recipes = new List<RECIPE>();
    //public int recipeIdx;

    [Header("Destination")]
    public GameObject destination;

    [Header("State")]
    public AIStateType stateType;
    public bool isComplete = false;
    public bool isRecovery = false;

    [Header("Hand")]
    public GameObject hand;
    public Transform handPos;

    [Header("Test")]
    public string stateTxt;
    public bool twoRecipe;
    public int hp;
    // 亜管馬檎 manager recipe 煽舌馬澗 員拭 bool 幻級嬢辞 傾獣杷 唖唖拭辞 淫軒馬亀系 郊荷奄!
    // -> 左嫌. 識鋼 紫遂馬形蟹?..


    private void Awake()
    {
        manager = GameObject.Find("AIMgr").GetComponent<AIManager>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        recipe.recipe = null;
        oldRecipe.recipe = null;
    }

    void Start()
    {
        bt = new BehaviourTreeManager<INode>();
        bt.SetRoot(new SelectorNode
        (
            // 3託 - Dev
            // 呪軒
            new SequenceNode
            (
                // hp亜 n左陥 碍陥檎
                // manager recipes拭辞 亜舌 原走厳 葵税 available聖 true稽
                new ConditionNode(NullRecipe),
                new ConditionNode(fix),
                new ActionNode(heal),
                new ActionNode(getRecovery),
                new ChangeStateNode(this, AIStateType.Ingredient)
            ),

            // 傾獣杷 識澱
            new SequenceNode
            (
                // 傾獣杷亜 蒸陥檎
                new ConditionNode(NullRecipe),
                // 傾獣杷研 走舛廃陥
                //new ActionNode(GiveRecipe),
                new RecipeNode(this),
                // 傾獣杷亜 赤陥檎 (傾獣杷亜 走舛菊陥檎)
                new InverterNode(new ConditionNode(NullRecipe)),
                // 傾獣杷 稽益 窒径
                new LogNode("傾獣杷"),
                // 傾獣杷 段奄鉢
                //new ActionNode(ResetRecipe),
                // 仙戟 雌殿稽 痕井
                new ChangeStateNode(this, AIStateType.Ingredient)
            ),

            // 鯉旋走 竺舛 貢 戚疑
            new SequenceNode
            (
                // 鯉旋走亜 蒸陥檎 (雌殿 痕井戚 析嬢概陥檎)
                new ConditionNode(NullDestination),
                // 鯉旋走研 竺舛
                new DestinationNode(this),
                // 是拭辞 呪軒 淫恵 bool 葵聖 熱嬢層陥檎 食奄辞 戚 走偶 照 背亀 喫!
                //new SequenceNode
                //(
                //    new ConditionNode(Recovery),
                //    new DestinationNode(this)
                //),
                // 戚疑
                new MoveNode(this, 3f)
            ),

            // 仙戟 識澱
            new SequenceNode
            (
                // 仙戟 什吐戚 限陥檎
                new CheckStateNode(this, AIStateType.Ingredient),
                // 暗軒亜 吉陥檎
                new RangeNode(this),
                // 朔 謝戚虞檎
                new ConditionNode(HandNull),
                new WaitNode(1f),
                // 雌硲拙遂
                new InteractionNode(this),
                // 朔 謝戚 焼艦虞檎 (焼戚奴 塙究)
                new InverterNode(new ConditionNode(HandNull)),
                // 雌殿 稽益 窒径
                new LogNode("仙戟"),
                // 雌殿 段奄鉢
                new ActionNode(ClearState),
                // 亜因 雌殿稽 痕井
                new ChangeStateNode(this, AIStateType.Processing)
            ),

            // 亜因 (亜因 伊紫, 亜因)
            new SelectorNode
            (
                // 亜因 伊紫
                new SequenceNode
                (
                    // 亜因 什吐戚 限陥檎
                    new CheckStateNode(this, AIStateType.Processing),
                    // 焼戚奴聖 級壱 赤陥檎
                    new InverterNode(new ConditionNode(HandNull)),
                    // 亜因戚 琶推馬走 省陥檎
                    new InverterNode(new ConditionNode(NeedProcessing)),
                    // 雌殿 段奄鉢
                    new ActionNode(ClearState),
                    // 佐杯 雌殿稽 痕井
                    new ChangeStateNode(this, AIStateType.Merge)
                ),

                // 亜因
                new SequenceNode
                (
                    // 亜因 什吐戚 限陥檎
                    new CheckStateNode(this, AIStateType.Processing),
                    // 暗軒亜 吉陥檎
                    new RangeNode(this),
                    // 焼戚奴聖 級壱 赤陥檎
                    new InverterNode(new ConditionNode(HandNull)),
                    // 雌硲拙遂 稽益 窒径
                    new LogNode("亜因掻"),
                    // 雌硲拙遂
                    new InteractionNode(this),
                    // 雌殿 稽益 窒径
                    new LogNode("亜因"),
                    // 雌殿 段奄鉢
                    new ActionNode(ClearState),
                    // 佐杯 雌殿稽 痕井
                    new ChangeStateNode(this, AIStateType.Merge)
                )
            ),

            // 佐杯 (隔奄, 条奄)
            new SelectorNode
            (
                // 隔奄
                new SequenceNode
                (
                    // 佐杯 什吐戚 限陥檎
                    new CheckStateNode(this, AIStateType.Merge),
                    // 噺呪 舘域亜 焼艦虞檎
                    new InverterNode(new ConditionNode(MergeComplete)),
                    // 暗軒亜 吉陥檎
                    new RangeNode(this),
                    // 焼戚奴聖 級壱 赤陥檎
                    new InverterNode(new ConditionNode(HandNull)),
                    new WaitNode(1f),
                    // 雌硲拙遂
                    new InteractionNode(this),
                    // 雌殿 稽益 窒径
                    new LogNode("佐杯"),
                    // 朔 謝戚虞檎 (焼戚奴 採鐸)
                    new ConditionNode(HandNull),
                    new SelectorNode
                    (
                        new SequenceNode
                        (
                            new InverterNode(new ConditionNode(pick)),
                            // 傾獣杷 陥製 舘域
                            new ActionNode(NextStep),
                            new WaitNode(1f)
                        ),
                        new SequenceNode
                        (
                            new ConditionNode(pick),
                            new ActionNode(testtest),
                            new ActionNode(EndRecovery),
                            new LogNode("拭拭拭拭拭拭つつつつつつつつつつつつつつつ")
                        )
                    )
                ),

                // 噺呪
                new SequenceNode
                (
                    // 佐杯 什吐戚 限陥檎
                    new CheckStateNode(this, AIStateType.Merge),
                    // 噺呪 舘域亜 限陥檎
                    new ConditionNode(MergeComplete),
                    // 暗軒亜 吉陥檎
                    new RangeNode(this),
                    // 朔 謝戚虞檎
                    new ConditionNode(HandNull),
                    new WaitNode(1f),
                    // 雌硲拙遂
                    new InteractionNode(this),
                    // 朔 謝戚 焼艦虞檎 (焼戚奴 塙究)
                    new InverterNode(new ConditionNode(HandNull)),
                    // 雌殿 稽益 窒径
                    new LogNode("塙究"),
                    new ActionNode(testtest22),
                    // 雌殿 段奄鉢
                    //new ActionNode(ClearState),
                    // 因維-二奄 雌殿 伊紫
                    new SelectorNode
                    (
                        // 噺差 - test .ver (recovery昔 井酔...)
                        new SequenceNode
                        (
                            // 析舘 噺差 益杏 幻球澗 掻 析 凶幻...
                            new ConditionNode(Recovery),
                            // 悦汽 闇遭 惟 床傾奄檎 照 喫;;
                            new InverterNode(new ConditionNode(CheckTrash)),
                            new SelectorNode
                            (
                                // 識鋼
                                new SequenceNode
                                (
                                    new ConditionNode(canShelf),
                                    //new ActionNode(NextStep),
                                    new LogNode("析舘 識鋼拭 戦食丘"),
                                    new ChangeStateNode(this, AIStateType.Shelf),
                                    new ActionNode(ClearState)
                                ),
                                // 識鋼精 焼艦壱... 噺差戚虞辞 ... .ぞ.ぞ.ぞ
                                new SequenceNode
                                (
                                    new ConditionNode(save),
                                    new WaitNode(1f),
                                    new InverterNode(new ConditionNode(HandNull)),
                                    new InteractionNode(this),
                                    new LogNode("析舘 陥獣 隔嬢丘"),
                                    new ActionNode(NextRecipe),
                                    new ChangeStateNode(this, AIStateType.Shelf),
                                    new ActionNode(ClearState)
                                )
                            )
                        ),
                        new SequenceNode
                        (
                            new InverterNode(new ConditionNode(Recovery)),
                            // 因維
                            new SequenceNode
                            (
                                // 床傾奄亜 焼艦虞檎
                                new InverterNode(new ConditionNode(CheckTrash)),
                                // 舛左 稽益 窒径
                                new LogNode("焼戚奴"),
                                new ActionNode(ClearState),
                                // 因維 雌殿稽 痕井
                                new ChangeStateNode(this, AIStateType.Attack)
                            ),
                            // 二奄
                            new SequenceNode
                            (
                                // 床傾奄虞檎
                                new ConditionNode(CheckTrash),
                                // 舛左 稽益 窒径
                                new LogNode("二奄"),
                                new ActionNode(ClearState),
                                // 二奄 雌殿稽 痕井
                                new ChangeStateNode(this, AIStateType.Trash)
                            )
                        )
                    )
                )
            ),

            // 識鋼
            new SequenceNode
            (
                // 識鋼拭 砧君 神澗 依引 噺呪 馬君 神澗 依聖 姥歳背醤敗.)
                // 因維 什吐戚 限陥檎
                new CheckStateNode(this, AIStateType.Shelf),
                // 暗軒亜 吉陥檎,
                new RangeNode(this),
                new SelectorNode
                (
                    // 噺呪
                    new SequenceNode
                    (
                        new ConditionNode(save),
                        new ConditionNode(HandNull),
                        new InteractionNode(this),
                        new LogNode(" 識鋼 - 噺呪"),
                        new InverterNode(new ConditionNode(HandNull)),
                        new ChangeStateNode(this, AIStateType.Merge),
                        new ActionNode(ClearState),
                        //new ActionNode(NextStep),
                        //new ActionNode(NextRecipe),
                        new LogNode("噺呪 失因")
                    ),
                    // 左淫
                    new SequenceNode
                    (
                        new ConditionNode(canShelf),
                        new InverterNode(new ConditionNode(HandNull)),
                        new InteractionNode(this),
                        new LogNode(" 識鋼 - 左淫"),
                        new ConditionNode(HandNull),
                        new ActionNode(NextStep),
                        new ActionNode(NextRecipe)
                    )
                )
            // 焼戚奴聖 級壱 赤陥檎
            // 雌硲拙遂
            // 雌硲拙遂 失因
            // 雌殿 稽益 窒径
            // 傾獣杷 段奄鉢
            ),

            // 因維
            new SequenceNode
            (
                // 因維 什吐戚 限陥檎
                new CheckStateNode(this, AIStateType.Attack),
                // 暗軒亜 吉陥檎,
                new RangeNode(this),
                // 焼戚奴聖 級壱 赤陥檎
                new InverterNode(new ConditionNode(HandNull)),
                // 雌硲拙遂
                new InteractionNode(this),
                // 雌硲拙遂 失因
                new ConditionNode(HandNull),
                // 雌殿 稽益 窒径
                new LogNode(" 因維"),
                // 傾獣杷 段奄鉢
                new ActionNode(ClearRecipe)
            //new SequenceNode
            //(
            //    new ConditionNode(Recovery),
            //    new ActionNode(NextRecipe)
            //)
            ),

            // 二奄
            new SequenceNode
            (
                // 二奄 什吐戚 限陥檎
                new CheckStateNode(this, AIStateType.Trash),
                // 暗軒亜 吉陥檎,
                new RangeNode(this),
                // 焼戚奴聖 級壱 赤陥檎
                new InverterNode(new ConditionNode(HandNull)),
                // 雌硲拙遂
                new InteractionNode(this),
                // 雌硲拙遂 失因
                new ConditionNode(HandNull),
                // 雌殿 稽益 窒径
                new LogNode(" 二奄"),
                // 傾獣杷 段奄鉢
                new ActionNode(ClearRecipe)
            //new SequenceNode
            //(
            //    new ConditionNode(Recovery),
            //    new ActionNode(NextRecipe)
            //)
            )
        ));
    }

    void Update()
    {
        bt.Update();
        stateTxt = stateType.ToString();
    }

    bool NullRecipe()
    {
        if (recipe.recipe == null)
            return true;
        return false;
    }

    void ClearRecipe()
    {
        recipe.recipe = null;
        recipe.index = 0;
        isComplete = false;
        twoRecipe = false;
        destination = null; 
    }

    void ResetRecipe()
    {
        //recipeIdx = 0;
        recipe = null;
        destination = null;
    }

    //void NextRecipe()
    //{
    //    if (oldRecipeIdx < 2)
    //        oldRecipeIdx++;

    //    if (oldRecipeIdx == 2)
    //    {
    //        oldRecipe = null;
    //        oldRecipeIdx = 0;
    //        isRecovery = false;
    //    }

    //    //if (isRecovery == false)
    //    stateType = AIStateType.Ingredient;
    //    recipeIdx = 0;

    //    destination = null;
    //}
    //void NextRecipe()
    //{
    //    if (recipe.oldRecipe.index < 2)
    //        recipe.oldRecipe.index++;

    //    if (recipe.oldRecipe.index == 2)
    //    {
    //        recipe.oldRecipe = null;
    //        recipe.oldRecipe.index = 0;
    //        isRecovery = false;
    //    }

    //    stateType = AIStateType.Ingredient;
    //    destination = null;
    //}

    void NextRecipe()
    {
        oldRecipe.recipe = null;
        oldRecipe.index = 0;
        stateType = AIStateType.Ingredient;
        isComplete = false;
        destination = null;
    }

    bool fix()
    {
        if (hp < 100)
            return true;
        return false;
    }

    void two()
    {
        twoRecipe = true;
    }

    void heal()
    {
        isRecovery = true;
        twoRecipe = true;
    }

    void testtest()
    {
        isComplete = true;
    }

    void testtest22()
    {
        isComplete = false;
    }

    void getRecovery()
    {
        recipe = manager.recovery;
        recipe.index = 0;
    }

    //void NextStep()
    //{
    //    if (recipeIdx < 2)
    //        recipeIdx++;

    //    if (recipeIdx == 2)
    //        isComplete = true;

    //    if (isComplete == false)
    //        stateType = AIStateType.Ingredient;
    //    destination = null;
    //}

    void NextStep()
    {
        if (twoRecipe && oldRecipe.recipe != null && oldRecipe.index >= 0)
        {
            if (oldRecipe.index < 2)
                oldRecipe.index++;

            if (oldRecipe.index == 2)
            {
                isComplete = true;
                oldRecipe.index = -1;
            }

            if (isComplete == false)
                stateType = AIStateType.Ingredient;
        }
        else
        {
            if (recipe.index < 2)
                recipe.index++;

            if (recipe.index == 2)
            {
                isComplete = true;
                recipe.index = -1;
            }

            if (isComplete == false)
                stateType = AIStateType.Ingredient;
        }

        destination = null;
    }

    bool NeedProcessing()
    {
        Ingredient ingredient = hand.GetComponent<Ingredient>();
        if (ingredient != null)
            return true;
        return false;
    }

    bool MergeComplete()
    {
        return isComplete;
    }

    bool Recovery()
    {
        return isRecovery;
    }

    void EndRecovery()
    {
        isRecovery = false; 
    }

    void CanFixed()
    {

    }

    bool canShelf()
    {
        return recipe.index == 0;
    }

    bool save()
    {
        return recipe.index == 1;
    }

    bool pick()
    {
        return recipe.index == -1;
    }

    //bool Recovery2()
    //{
    //    return oldRecipeIdx == 1;
    //}

    bool NullDestination()
    {
        if (destination != null)
            return false;
        return true;
    }

   void ClearState()
   {
        destination = null;
   }

    bool CheckTrash()
    {
        if (hand.name == "Trash")
            return true;
        return false;
    }

    //bool Test()
    //{
    //    if (oldRecipeIdx == 1)
    //}

    bool HandNull()
    {
        return hand == null ? true : false;
    }
}
