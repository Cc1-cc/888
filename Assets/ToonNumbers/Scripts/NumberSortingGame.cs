using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NumberSortingGame : MonoBehaviour
{
    public Transform obj; // 包含 0-9 数字模型的父对象
    public Transform[] targetPositions; // 目标位置（按 0-9 顺序排列）
    private List<Transform> numberModels = new List<Transform>(); // 存储数字模型
    private Transform selectedModel; // 当前选中的模型
    private Vector3 initialPosition; // 拖动前的初始位置
    private bool isDragging = false; // 是否正在拖动
    private bool isGameComplete = false; // 游戏是否完成

    public GameObject rightobj;



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
        // 初始化当前时间
        currentTime = totalTime;

        // 开始倒计时
        StartCountdown();

        // 获取所有数字模型
        foreach (Transform child in obj)
        {
            numberModels.Add(child);
        }

        // 随机打乱数字模型的位置（仅打乱位置，不改变模型顺序）
        ShufflePositions();
    }

    void Update()
    {
        if (isGameComplete) return; // 如果游戏已完成，不再执行后续逻辑

        // 拖动逻辑
        if (Input.GetMouseButtonDown(0)) // 鼠标左键按下
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 检查是否点击了数字模型
                if (numberModels.Contains(hit.transform))
                {
                    selectedModel = hit.transform;
                    initialPosition = selectedModel.position; // 记录拖动前的初始位置
                    isDragging = true;
                }
            }
        }

        if (isDragging && Input.GetMouseButton(0)) // 鼠标左键按住
        {
            // 拖动模型
            Vector3 newPosition = GetMouseWorldPosition();
            selectedModel.position = new Vector3(newPosition.x, newPosition.y, selectedModel.position.z);
        }

        if (isDragging && Input.GetMouseButtonUp(0)) // 鼠标左键松开
        {
            // 检测是否靠近目标位置
            CheckTargetPosition(selectedModel);
            selectedModel = null;
            isDragging = false;

            // 检查游戏是否完成
            CheckGameComplete();
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

    Vector3 GetMouseWorldPosition()
    {
        // 获取鼠标在世界空间中的位置
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(selectedModel.position).z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void ShufflePositions()
    {
        // 随机打乱数字模型的位置（仅打乱位置，不改变模型顺序）
        List<Vector3> positions = new List<Vector3>();
        foreach (Transform target in targetPositions)
        {
            positions.Add(target.position);
        }

        // 打乱位置列表
        for (int i = 0; i < positions.Count; i++)
        {
            int randomIndex = Random.Range(0, positions.Count);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // 将数字模型移动到打乱后的位置
        for (int i = 0; i < numberModels.Count; i++)
        {
            numberModels[i].position = positions[i];
        }
    }

    void CheckTargetPosition(Transform model)
    {
        // 检查模型是否靠近目标位置
        for (int i = 0; i < targetPositions.Length; i++)
        {
            if (Vector3.Distance(model.position, targetPositions[i].position) < 0.5f) // 距离阈值
            {
                // 如果模型的名字与目标位置的名字匹配，则吸附到目标位置
                if (model.name == i.ToString())
                {
                    model.position = targetPositions[i].position;
                    Debug.Log("模型 " + model.name + " 已放置到正确位置");
                }
                break;
            }
        }
    }

    void CheckGameComplete()
    {
        // 检查是否所有模型都已放置到正确位置
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
            isGameComplete = true; // 标记游戏完成
            rightobj.SetActive(true);
            OnCountdownFinished();
            Debug.Log("游戏完成！所有数字已按顺序排列");
        }
    }
    private void OnCountdownFinished()
    {

        // 在这里添加倒计时结束后的逻辑
        jieshuobj.SetActive(true);

        if (isGameComplete)
        {
            xing3.SetActive(true);
            guli.text = "太棒了，奖励你一朵小花花";
        }
        else
        {
            xing3.SetActive(false);
            xing1.SetActive(true);
            xing2.SetActive(false);
            guli.text = "别灰心，下次会更加棒";
        }
        
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

    public void adddddddddtime()
    {
        // 获取当前显示的倒计时文本时间（以秒为单位）
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int currentDisplayedTime = minutes * 60 + seconds;  // 将分钟和秒转为总秒数

        // 将当前倒计时的时间加到 timenum 上
        timenum += currentDisplayedTime;

        // 更新 PlayerPrefs 中存储的时间
        PlayerPrefs.SetInt("time", 0);
        PlayerPrefs.Save();

        // 更新倒计时的当前时间
        currentTime = timenum; // 直接更新倒计时的剩余时间

        // 输出当前的 timenum（调试）
        Debug.Log("Updated timenum: " + timenum);

        // 更新倒计时显示
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