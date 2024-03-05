using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlatform : MonoBehaviour
{
    [SerializeField] private string moveSceneName;
    public StageDataSO thisStageData;
    [SerializeField] private GameObject stageCanvas;

    private Vector3[] starPos;   // ���� ���� ��ġ
    [SerializeField] private GameObject starPrefab;

    private PlayerInteract playerInteract;

    private void Awake()
    {
        starPos = new Vector3[3];
        for (int i = 0; i < starPos.Length; i++)
        {
            starPos[i] = transform.GetChild(i).position;
            starPos[i] = new Vector3(starPos[i].x, starPos[i].y + 0.5f, starPos[i].z);      // y �� �÷��ֱ�
        }

        StarInit();
    }

    public void Interact()
    {
        Debug.Log("F Ű ����.");

        CloudManager.Instance?.Move(true);
        LoadingSceneManager.Instance?.ChangeLoadScene(moveSceneName);
        GameManager.Instance.nowStageData = thisStageData;
    }

    public void StarInit()
    {
        thisStageData.PersentSetting();
        for (int i = 0;i < 3; i++)
        {
            if (thisStageData.star[i] == true)
            {
                GameObject star = Instantiate(starPrefab, starPos[i], Quaternion.identity);       // ����� �����̳� �׽�Ʈ �ѹ� �غ���
                star.transform.parent = transform;
                if (i == 2)     // 3��°, �� ��� �� �̶�� ũ�⸦ ���� ũ�� ���ֱ�
                {
                    star.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);      // Ű����.
                }
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
