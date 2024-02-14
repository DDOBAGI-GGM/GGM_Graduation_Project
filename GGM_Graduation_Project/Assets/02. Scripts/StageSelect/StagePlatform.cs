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
        Debug.Log("이동된다");
        //CloudManager.Instance?.Move(true);
        //LoadingSceneManager.Instance?.ChangeLoadScene(scene);

        if (CloudManager.Instance == null && LoadingSceneManager.Instance == null)      // 디버그용 코드. 씬 셀렉트 씬에서 바로 이동할 때 사용함.
        {
            GameManager.Instance.nowStageData = ScriptableObject.CreateInstance<StageDataSO>();         // 새 것으로 복사해서 넣어줌.
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
