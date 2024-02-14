using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlatform : MonoBehaviour
{
    [SerializeField] private string moveSceneName;
    public StageDataSO thisStageData;
    [SerializeField] private GameObject stageCanvas;

    private PlayerInteract playerInteract;

    public void Interact()
    {
        Debug.Log("�̵��ȴ�");
        //CloudManager.Instance?.Move(true);
        //LoadingSceneManager.Instance?.ChangeLoadScene(scene);

        if (CloudManager.Instance == null && LoadingSceneManager.Instance == null)      // ����׿� �ڵ�. �� ����Ʈ ������ �ٷ� �̵��� �� �����.
        {
            GameManager.Instance.nowStageData = ScriptableObject.CreateInstance<StageDataSO>();         // �� ������ �����ؼ� �־���.
            SceneManager.LoadScene(moveSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stageCanvas.SetActive(true);
            if (playerInteract == null)
            {
                playerInteract = other.GetComponent<PlayerInteract>();
            }
            playerInteract.platform = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stageCanvas.SetActive(false);
            playerInteract.platform = null;
        }
    }
}
