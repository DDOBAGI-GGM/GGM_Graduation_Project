using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlatform : MonoBehaviour
{
    [Header("�̵��� ��")][SerializeField] private string scene;

    public StageDataSO thisStage;

    public void Interact()
    {
        Debug.Log("�̵��ȴ�");
        //CloudManager.Instance?.Move(true);
        //LoadingSceneManager.Instance?.ChangeLoadScene(scene);

        if (CloudManager.Instance == null && LoadingSceneManager.Instance == null)      // ����׿� �ڵ�. �� ����Ʈ ������ �ٷ� �̵��� �� �����.
        {
            GameManager.Instance.nowStageData = ScriptableObject.CreateInstance<StageDataSO>();         // �� ������ �����ؼ� �־���.
            SceneManager.LoadScene(scene);
        }
    }
}
