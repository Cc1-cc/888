using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditionGame : MonoBehaviour
{
    public Transform numobj; // ��һ�����ֵĸ�����
    public Transform numobj1; // �ڶ������ֵĸ�����
    public Transform dianji; // �ɵ�������ֵĸ�����
    public Transform answer1; // answer1 �ĸ�����
    public Transform targetPosition; // Ŀ��λ��

    private int num1; // ��һ������
    private int num2; // �ڶ�������
    private int result; // ������
    private bool isGameComplete = false; // ��Ϸ�Ƿ����
    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>(); // ��¼��ʼλ��

    public GameObject rightobj;
    public GameObject cuoobj;
    public GameObject wenhaoobj;


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

    // ��Ժʹ��Ĵ���
    private int correctCount = 0;
    private int wrongCount = 0;


    public GameObject openjl;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject obj5;

    public GameObject addtime;
    private int consecutiveCorrectCount = 0;
    void Start()
    {

        // ��ʼ����ǰʱ��
        currentTime = totalTime;

        // ��ʼ����ʱ
        StartCountdown();
        // ��¼��������ģ�͵ĳ�ʼλ��
        RecordInitialPositions(dianji);
        StartNewGame();
    }

    void RecordInitialPositions(Transform parent)
    {
        // ��¼����������������ģ�͵ĳ�ʼλ��
        foreach (Transform child in parent)
        {
            initialPositions[child] = child.position;
        }
    }

    void StartNewGame()
    {
        // ������Ϸ���״̬
        isGameComplete = false;

        // ������������ģ��
        HideAllNumbers(numobj);
        HideAllNumbers(numobj1);
        HideAllNumbers(dianji);
        answer1.GetChild(1).gameObject.SetActive(false); // ���� answer1 �µĵ�һ������

        // ����������ģ�ͻָ�����ʼλ��
        ResetNumbersToInitialPositions();

        // ���������������
        num1 = Random.Range(0, 10);
        num2 = Random.Range(0, 10);
        result = num1 + num2;

        Debug.Log("��Ŀ: " + num1 + " + " + num2 + " = ?");

        // ��ʾ������ɵ�����
        ShowNumber(numobj, num1);
        ShowNumber(numobj1, num2);

        // ���������ڵ��� 10����ʾ answer1 �µĵ�һ������
        if (result >= 10)
        {
            answer1.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            answer1.GetChild(1).gameObject.SetActive(false);
        }

        // ��ʾ dianji �µ�����ģ��
        ShowAllNumbers(dianji);
    }

    void HideAllNumbers(Transform parent)
    {
        // ���ظ������µ���������ģ��
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
    }

    void ShowAllNumbers(Transform parent)
    {
        // ��ʾ�������µ���������ģ��
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(true);
        }
    }

    void ShowNumber(Transform parent, int number)
    {
        // ��ʾָ�����ֵ�ģ��
        parent.GetChild(number).gameObject.SetActive(true);
    }

    void ResetNumbersToInitialPositions()
    {
        // ����������ģ�ͻָ�����ʼλ��
        foreach (var kvp in initialPositions)
        {
            kvp.Key.position = kvp.Value;
        }
        wenhaoobj.SetActive(true);
        rightobj.SetActive(false);
        cuoobj.SetActive(false);
    }

    void Update()
    {
        if (isGameComplete) return; // �����Ϸ����ɣ�����ִ�к����߼�

        // ��������
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // ����Ƿ����� dianji �µ�����ģ��
                if (hit.transform.parent == dianji)
                {
                    MoveNumberToTarget(hit.transform);
                    wenhaoobj.SetActive(false);
                }
            }
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

    void MoveNumberToTarget(Transform number)
    {
        // �������ƶ���Ŀ��λ��
        StartCoroutine(MoveToPosition(number, targetPosition.position, 1f));

        // ����
        CheckAnswer(number);
    }

    IEnumerator MoveToPosition(Transform number, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = number.position;

        while (time < duration)
        {
            number.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        number.position = targetPosition; // ȷ������λ��׼ȷ
    }

    void CheckAnswer(Transform number)
    {
        // ������������Ƿ���ȷ
        int selectedNumber = int.Parse(number.name); // ��ȡ���ѡ�������

        if (result >= 10)
        {
            // ���������ڵ��� 10��ƴ��ʮλ���͸�λ��
            int combinedNumber = 10 + selectedNumber; // ʮλ���̶�Ϊ 1
            if (combinedNumber == result)
            {
                Debug.Log("�ش���ȷ��");
                isGameComplete = true;
                rightobj.SetActive(true);
                correctCount++;
                consecutiveCorrectCount++; // ���ʱ������������Լ���
            }
            else
            {
                Debug.Log("�ش����");
                cuoobj.SetActive(true);
                wrongCount++;
                consecutiveCorrectCount=0; // ���ʱ������������Լ���
            }
        }
        else
        {
            // ������С�� 10��ֱ�ӱȽ�
            if (selectedNumber == result)
            {
                Debug.Log("�ش���ȷ��");
                isGameComplete = true;
                rightobj.SetActive(true);
                correctCount++;
                consecutiveCorrectCount++; // ���ʱ������������Լ���
            }
            else
            {
                Debug.Log("�ش����");
                cuoobj.SetActive(true);
                wrongCount++;
                consecutiveCorrectCount=0; // ���ʱ������������Լ���
            }
        }
        if(consecutiveCorrectCount==5)
        {
            addtime.SetActive(true);
        }

        // ��ʼ����Ϸ
        StartCoroutine(StartNewGameAfterDelay(2f));
    }

    IEnumerator StartNewGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNewGame();
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

    public void addddddtime()
    {
        addtime.SetActive(false);

        int timenum = 5;
        // ��ȡ��ǰ��ʾ�ĵ���ʱ�ı�ʱ�䣨����Ϊ��λ��
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int currentDisplayedTime = minutes * 60 + seconds;  // �����Ӻ���תΪ������

        // ����ǰ����ʱ��ʱ��ӵ� timenum ��
        timenum += currentDisplayedTime;

        

        // ���µ���ʱ�ĵ�ǰʱ��
        currentTime = timenum; // ֱ�Ӹ��µ���ʱ��ʣ��ʱ��

        // �����ǰ�� timenum�����ԣ�
        Debug.Log("Updated timenum: " + timenum);

        // ���µ���ʱ��ʾ
        UpdateCountdownDisplay();
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

    // ����ʱ����ʱ�Ļص�
    private void OnCountdownFinished()
    {
        Debug.Log("����ʱ������");
        Debug.Log("��Դ���: " + correctCount);
        Debug.Log("������: " + wrongCount);
        // ��������ӵ���ʱ��������߼�
        jieshuobj.SetActive(true);
        zhengquetext.text = "��ȷ����:" + correctCount;
        cuowutext.text = "�������:" + wrongCount;
        if (correctCount < 10)
        {
            xing1.SetActive(true);
            guli.text = "����ģ��´λ����";
        }
        if (correctCount >= 10 && correctCount <= 15)
        {
            xing2.SetActive(true);
            xing1.SetActive(false);
            xing3.SetActive(false);
            guli.text = "���������ȡ����100��Ŷ";
        }
        if (correctCount > 15)
        {
            xing3.SetActive(true);
            xing1.SetActive(false);
            xing2.SetActive(false);
            guli.text = "̫���ˣ�������һ��С����";
        }
    }
    public void openchengjiu()
    {
        openjl.SetActive(true);
        PlayerPrefs.SetInt("di3", consecutiveCorrectCount);
        PlayerPrefs.Save();
        if (consecutiveCorrectCount==5)
        {
            obj1.SetActive(true);
        }
        if (consecutiveCorrectCount == 10)
        {
            obj1.SetActive(true);
            obj2.SetActive(true);
        }
        if (consecutiveCorrectCount == 20)
        {
            obj1.SetActive(true);
            obj2.SetActive(true);
            obj3.SetActive(true);
        }
        if (consecutiveCorrectCount == 30)
        {
            obj1.SetActive(true);
            obj2.SetActive(true);
            obj3.SetActive(true);
            obj4.SetActive(true);
        }
        if (consecutiveCorrectCount == 40)
        {
            obj1.SetActive(true);
            obj2.SetActive(true);
            obj3.SetActive(true);
            obj4.SetActive(true);
            obj5.SetActive(true);
        }
    }

    public void closechengjiu()
    {
        openjl.SetActive(false);
    }
    public void ClickCHONGWAN()
    {
        if (correctCount < 10)
        {
            SceneManager.LoadScene("Numberinposition2");
        }
        else
        {
            SceneManager.LoadScene("SceneMaths");
        }

    }

    public void chongwan()
    {
        SceneManager.LoadScene("Numberinposition2");
    }
}