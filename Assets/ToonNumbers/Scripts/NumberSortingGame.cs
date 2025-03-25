using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NumberSortingGame : MonoBehaviour
{
    public Transform obj; // ���� 0-9 ����ģ�͵ĸ�����
    public Transform[] targetPositions; // Ŀ��λ�ã��� 0-9 ˳�����У�
    private List<Transform> numberModels = new List<Transform>(); // �洢����ģ��
    private Transform selectedModel; // ��ǰѡ�е�ģ��
    private Vector3 initialPosition; // �϶�ǰ�ĳ�ʼλ��
    private bool isDragging = false; // �Ƿ������϶�
    private bool isGameComplete = false; // ��Ϸ�Ƿ����

    public GameObject rightobj;



    public GameObject jieshuobj;
    public TextMeshProUGUI zhengquetext;
    public TextMeshProUGUI cuowutext;
    public GameObject xing1;
    public GameObject xing2;
    public GameObject xing3;

    public TextMeshProUGUI guli;

    // ����ʱ����ʱ�䣨�룩
    public float totalTime = 60f;

    // ��ʾ����ʱ��UI�ı�
    public TextMeshProUGUI countdownText;

    // �ڲ���ʱ��
    private float currentTime;

    // �Ƿ�ʼ����ʱ
    private bool isCounting = false;


    public GameObject lllllll;
    public GameObject obj111111;
    public GameObject obj222222;
    public GameObject obj333333;
    public GameObject obj444444;
    public GameObject obj555555;

    public int timenum;
    public GameObject timeobj;
    void Start()
    {
        timenum = PlayerPrefs.GetInt("time");
        if(timenum>0)
        {
            timeobj.SetActive(true);
        }
        Debug.Log(timenum);
        // ��ʼ����ǰʱ��
        currentTime = totalTime;

        // ��ʼ����ʱ
        StartCountdown();

        // ��ȡ��������ģ��
        foreach (Transform child in obj)
        {
            numberModels.Add(child);
        }

        // �����������ģ�͵�λ�ã�������λ�ã����ı�ģ��˳��
        ShufflePositions();
    }

    void Update()
    {
        if (isGameComplete) return; // �����Ϸ����ɣ�����ִ�к����߼�

        // �϶��߼�
        if (Input.GetMouseButtonDown(0)) // ����������
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ����Ƿ���������ģ��
                if (numberModels.Contains(hit.transform))
                {
                    selectedModel = hit.transform;
                    initialPosition = selectedModel.position; // ��¼�϶�ǰ�ĳ�ʼλ��
                    isDragging = true;
                }
            }
        }

        if (isDragging && Input.GetMouseButton(0)) // ��������ס
        {
            // �϶�ģ��
            Vector3 newPosition = GetMouseWorldPosition();
            selectedModel.position = new Vector3(newPosition.x, newPosition.y, selectedModel.position.z);
        }

        if (isDragging && Input.GetMouseButtonUp(0)) // �������ɿ�
        {
            // ����Ƿ񿿽�Ŀ��λ��
            CheckTargetPosition(selectedModel);
            selectedModel = null;
            isDragging = false;

            // �����Ϸ�Ƿ����
            CheckGameComplete();
        }
        if (isCounting)
        {
            // ���µ�ǰʱ��
            currentTime -= Time.deltaTime;

            // ���ʱ��С��0��ֹͣ����ʱ
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCounting = false;
                OnCountdownFinished();
            }

            // ����UI��ʾ
            UpdateCountdownDisplay();
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // ��ȡ���������ռ��е�λ��
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(selectedModel.position).z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void ShufflePositions()
    {
        // �����������ģ�͵�λ�ã�������λ�ã����ı�ģ��˳��
        List<Vector3> positions = new List<Vector3>();
        foreach (Transform target in targetPositions)
        {
            positions.Add(target.position);
        }

        // ����λ���б�
        for (int i = 0; i < positions.Count; i++)
        {
            int randomIndex = Random.Range(0, positions.Count);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // ������ģ���ƶ������Һ��λ��
        for (int i = 0; i < numberModels.Count; i++)
        {
            numberModels[i].position = positions[i];
        }
    }

    void CheckTargetPosition(Transform model)
    {
        // ���ģ���Ƿ񿿽�Ŀ��λ��
        for (int i = 0; i < targetPositions.Length; i++)
        {
            if (Vector3.Distance(model.position, targetPositions[i].position) < 0.5f) // ������ֵ
            {
                // ���ģ�͵�������Ŀ��λ�õ�����ƥ�䣬��������Ŀ��λ��
                if (model.name == i.ToString())
                {
                    model.position = targetPositions[i].position;
                    Debug.Log("ģ�� " + model.name + " �ѷ��õ���ȷλ��");
                }
                break;
            }
        }
    }

    void CheckGameComplete()
    {
        // ����Ƿ�����ģ�Ͷ��ѷ��õ���ȷλ��
        bool isComplete = true;
        for (int i = 0; i < numberModels.Count; i++)
        {
            if (Vector3.Distance(numberModels[i].position, targetPositions[i].position) > 0.5f || numberModels[i].name != i.ToString())
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
        {
            isGameComplete = true; // �����Ϸ���
            rightobj.SetActive(true);
            OnCountdownFinished();
            Debug.Log("��Ϸ��ɣ����������Ѱ�˳������");
        }
    }
    private void OnCountdownFinished()
    {

        // ��������ӵ���ʱ��������߼�
        jieshuobj.SetActive(true);

        if (isGameComplete)
        {
            xing3.SetActive(true);
            guli.text = "̫���ˣ�������һ��С����";
        }
        else
        {
            xing3.SetActive(false);
            xing1.SetActive(true);
            xing2.SetActive(false);
            guli.text = "����ģ��´λ���Ӱ�";
        }
        
    }


    // ��ʼ����ʱ
    public void StartCountdown()
    {
        isCounting = true;
    }

    // ֹͣ����ʱ
    public void StopCountdown()
    {
        isCounting = false;
    }

    // ���µ���ʱ��ʾ
    private void UpdateCountdownDisplay()
    {
        if (countdownText != null)
        {
            // ��ʱ��ת��Ϊ���Ӻ���
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            // ��ʽ����ʾ
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void adddddddddtime()
    {
        // ��ȡ��ǰ��ʾ�ĵ���ʱ�ı�ʱ�䣨����Ϊ��λ��
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int currentDisplayedTime = minutes * 60 + seconds;  // �����Ӻ���תΪ������

        // ����ǰ����ʱ��ʱ��ӵ� timenum ��
        timenum += currentDisplayedTime;

        // ���� PlayerPrefs �д洢��ʱ��
        PlayerPrefs.SetInt("time", 0);
        PlayerPrefs.Save();

        // ���µ���ʱ�ĵ�ǰʱ��
        currentTime = timenum; // ֱ�Ӹ��µ���ʱ��ʣ��ʱ��

        // �����ǰ�� timenum�����ԣ�
        Debug.Log("Updated timenum: " + timenum);

        // ���µ���ʱ��ʾ
        UpdateCountdownDisplay();
        timeobj.SetActive(false);
    }

    public void ClickCHONGWAN()
    {
        if (isGameComplete)
        {
            PlayerPrefs.SetInt("di2", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Numberinposition2");
        }
        else
        {

            SceneManager.LoadScene("Numberinposition1");
        }

    }

    public void chongwanaaaaaaa()
    {
        SceneManager.LoadScene("Numberinposition1");
    }

}