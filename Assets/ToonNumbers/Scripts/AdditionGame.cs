using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditionGame : MonoBehaviour
{
    public Transform numobj; // 第一个数字的父对象
    public Transform numobj1; // 第二个数字的父对象
    public Transform dianji; // 可点击的数字的父对象
    public Transform answer1; // answer1 的父对象
    public Transform targetPosition; // 目标位置

    private int num1; // 第一个数字
    private int num2; // 第二个数字
    private int result; // 计算结果
    private bool isGameComplete = false; // 游戏是否完成
    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>(); // 记录初始位置

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

    // 倒计时的总时间（秒）
    public float totalTime = 60f;

    // 显示倒计时的UI文本
    public TextMeshProUGUI countdownText;

    // 内部计时器
    private float currentTime;

    // 是否开始倒计时
    private bool isCounting = false;

    // 答对和答错的次数
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

        // 初始化当前时间
        currentTime = totalTime;

        // 开始倒计时
        StartCountdown();
        // 记录所有数字模型的初始位置
        RecordInitialPositions(dianji);
        StartNewGame();
    }

    void RecordInitialPositions(Transform parent)
    {
        // 记录父对象下所有数字模型的初始位置
        foreach (Transform child in parent)
        {
            initialPositions[child] = child.position;
        }
    }

    void StartNewGame()
    {
        // 重置游戏完成状态
        isGameComplete = false;

        // 隐藏所有数字模型
        HideAllNumbers(numobj);
        HideAllNumbers(numobj1);
        HideAllNumbers(dianji);
        answer1.GetChild(1).gameObject.SetActive(false); // 隐藏 answer1 下的第一个物体

        // 将所有数字模型恢复到初始位置
        ResetNumbersToInitialPositions();

        // 随机生成两个数字
        num1 = Random.Range(0, 10);
        num2 = Random.Range(0, 10);
        result = num1 + num2;

        Debug.Log("题目: " + num1 + " + " + num2 + " = ?");

        // 显示随机生成的数字
        ShowNumber(numobj, num1);
        ShowNumber(numobj1, num2);

        // 如果结果大于等于 10，显示 answer1 下的第一个物体
        if (result >= 10)
        {
            answer1.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            answer1.GetChild(1).gameObject.SetActive(false);
        }

        // 显示 dianji 下的数字模型
        ShowAllNumbers(dianji);
    }

    void HideAllNumbers(Transform parent)
    {
        // 隐藏父对象下的所有数字模型
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
    }

    void ShowAllNumbers(Transform parent)
    {
        // 显示父对象下的所有数字模型
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(true);
        }
    }

    void ShowNumber(Transform parent, int number)
    {
        // 显示指定数字的模型
        parent.GetChild(number).gameObject.SetActive(true);
    }

    void ResetNumbersToInitialPositions()
    {
        // 将所有数字模型恢复到初始位置
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
        if (isGameComplete) return; // 如果游戏已完成，不再执行后续逻辑

        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 检查是否点击了 dianji 下的数字模型
                if (hit.transform.parent == dianji)
                {
                    MoveNumberToTarget(hit.transform);
                    wenhaoobj.SetActive(false);
                }
            }
        }
        if (isCounting)
        {
            // 更新当前时间
            currentTime -= Time.deltaTime;

            // 如果时间小于0，停止倒计时
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCounting = false;
                OnCountdownFinished();
            }

            // 更新UI显示
            UpdateCountdownDisplay();
        }
    }

    void MoveNumberToTarget(Transform number)
    {
        // 将数字移动到目标位置
        StartCoroutine(MoveToPosition(number, targetPosition.position, 1f));

        // 检查答案
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

        number.position = targetPosition; // 确保最终位置准确
    }

    void CheckAnswer(Transform number)
    {
        // 检查点击的数字是否正确
        int selectedNumber = int.Parse(number.name); // 获取玩家选择的数字

        if (result >= 10)
        {
            // 如果结果大于等于 10，拼接十位数和个位数
            int combinedNumber = 10 + selectedNumber; // 十位数固定为 1
            if (combinedNumber == result)
            {
                Debug.Log("回答正确！");
                isGameComplete = true;
                rightobj.SetActive(true);
                correctCount++;
                consecutiveCorrectCount++; // 答对时，增加连续答对计数
            }
            else
            {
                Debug.Log("回答错误！");
                cuoobj.SetActive(true);
                wrongCount++;
                consecutiveCorrectCount=0; // 答对时，增加连续答对计数
            }
        }
        else
        {
            // 如果结果小于 10，直接比较
            if (selectedNumber == result)
            {
                Debug.Log("回答正确！");
                isGameComplete = true;
                rightobj.SetActive(true);
                correctCount++;
                consecutiveCorrectCount++; // 答对时，增加连续答对计数
            }
            else
            {
                Debug.Log("回答错误！");
                cuoobj.SetActive(true);
                wrongCount++;
                consecutiveCorrectCount=0; // 答对时，增加连续答对计数
            }
        }
        if(consecutiveCorrectCount==5)
        {
            addtime.SetActive(true);
        }

        // 开始新游戏
        StartCoroutine(StartNewGameAfterDelay(2f));
    }

    IEnumerator StartNewGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNewGame();
    }

    // 开始倒计时
    public void StartCountdown()
    {
        isCounting = true;
    }

    // 停止倒计时
    public void StopCountdown()
    {
        isCounting = false;
    }

    public void addddddtime()
    {
        addtime.SetActive(false);

        int timenum = 5;
        // 获取当前显示的倒计时文本时间（以秒为单位）
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int currentDisplayedTime = minutes * 60 + seconds;  // 将分钟和秒转为总秒数

        // 将当前倒计时的时间加到 timenum 上
        timenum += currentDisplayedTime;

        

        // 更新倒计时的当前时间
        currentTime = timenum; // 直接更新倒计时的剩余时间

        // 输出当前的 timenum（调试）
        Debug.Log("Updated timenum: " + timenum);

        // 更新倒计时显示
        UpdateCountdownDisplay();
    }


    // 更新倒计时显示
    private void UpdateCountdownDisplay()
    {
        if (countdownText != null)
        {
            // 将时间转换为分钟和秒
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            // 格式化显示
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // 倒计时结束时的回调
    private void OnCountdownFinished()
    {
        Debug.Log("倒计时结束！");
        Debug.Log("答对次数: " + correctCount);
        Debug.Log("答错次数: " + wrongCount);
        // 在这里添加倒计时结束后的逻辑
        jieshuobj.SetActive(true);
        zhengquetext.text = "正确次数:" + correctCount;
        cuowutext.text = "错误次数:" + wrongCount;
        if (correctCount < 10)
        {
            xing1.SetActive(true);
            guli.text = "别灰心，下次会更棒";
        }
        if (correctCount >= 10 && correctCount <= 15)
        {
            xing2.SetActive(true);
            xing1.SetActive(false);
            xing3.SetActive(false);
            guli.text = "你真棒，争取做到100分哦";
        }
        if (correctCount > 15)
        {
            xing3.SetActive(true);
            xing1.SetActive(false);
            xing2.SetActive(false);
            guli.text = "太棒了，奖励你一朵小花花";
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