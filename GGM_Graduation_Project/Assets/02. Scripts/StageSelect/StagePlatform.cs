using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlatform : MonoBehaviour
{
    [SerializeField] private string moveSceneName;
    public StageDataSO thisStageData;
    [SerializeField] private GameObject stageCanvas;

    private Vector3[] starPos;   // 별이 들어가는 위치
    [SerializeField] private GameObject starPrefab;

    private PlayerInteract playerInteract;

    private void Awake()
    {
        starPos = new Vector3[3];
        for (int i = 0; i < starPos.Length; i++)
        {
            starPos[i] = transform.GetChild(i).position;
            starPos[i] = new Vector3(starPos[i].x, starPos[i].y + 0.5f, starPos[i].z);      // y 값 올려주기
        }
    }

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

    public void StarInit(int starCnt = 0)
    {
        for (int i = 0;i < starCnt;i++)
        {
            GameObject star = Instantiate(starPrefab, starPos[i], Quaternion.identity);       // 월드냐 로컬이냐 테스트 한번 해보기
            star.transform.parent = transform;
            if (i == 2)     // 3번째, 가운데 것 이라면 크기를 조금 크게 해주기
            {
                star.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);      // 키워줌.
            }
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
