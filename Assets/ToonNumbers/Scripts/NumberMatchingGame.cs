using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NumberMatchingGame : MonoBehaviour
{
    public GameObject obj;
    public List<GameObject> clickableModels;
    public GameObject checkmark;
    public GameObject crossmark;

    private List<GameObject> numberModels;
    private GameObject currentDisplayedModel;
    private bool isWaiting = false;

    public GameObject jieshuobj;
    public TextMeshProUGUI zhengquetext;
    public TextMeshProUGUI cuowutext;
    public GameObject xing1;
    public GameObject xing2;
    public GameObject xing3;
    public TextMeshProUGUI guli;

    public float totalTime = 60f;
    public TextMeshProUGUI countdownText;
    private float currentTime;
    private bool isCounting = false;
    private int correctCount = 0;
    private int wrongCount = 0;

    public GameObject jjjjjjj;

    public GameObject obj111;
    public GameObject obj222;
    public GameObject obj333;
    public GameObject obj444;
    public GameObject obj555;

    // ✅【新增】连续答对5题检测
    private int consecutiveCorrect = 0;
    public TextMeshProUGUI fiveCorrectText;  // 连续答对5题的提示

    void Start()
    {
        currentTime = totalTime;
        StartCountdown();
        checkmark.SetActive(false);
        crossmark.SetActive(false);

        // 获取父物体下所有子物体
        numberModels = new List<GameObject>();
        foreach (Transform child in obj.transform)
        {
            numberModels.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        StartCoroutine(StartGame());

    }

    public void jjjjjjjj()
    {
        jjjjjjj.SetActive(true);
    }


    IEnumerator StartGame()
    {
        while (true)
        {
            ShowRandomModel();
            yield return new WaitUntil(() => isWaiting);
            currentDisplayedModel.SetActive(false);
            isWaiting = false;
            yield return new WaitForSeconds(1f);
        }
    }

    void ShowRandomModel()
    {
        GameObject newModel;
        do
        {
            newModel = numberModels[Random.Range(0, numberModels.Count)];
        } while (newModel == currentDisplayedModel);

        newModel.SetActive(true);
        currentDisplayedModel = newModel;
        checkmark.SetActive(false);
        crossmark.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedModel = hit.collider.gameObject;
                if (clickableModels.Contains(clickedModel))
                {
                    OnModelClicked(clickedModel);
                }
            }
        }

        if (isCounting)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCounting = false;
                OnCountdownFinished();
            }
            UpdateCountdownDisplay();
        }
    }

    public void OnModelClicked(GameObject clickedModel)
    {
        if (isWaiting) return;
        string clickedName = clickedModel.name;
        string displayedName = currentDisplayedModel.name;

        if (clickedName == displayedName)
        {
            checkmark.SetActive(true);
            correctCount++;
            consecutiveCorrect++;  // ✅ 连续答对+1

            // ✅ 检测是否连续答对5题
            if (consecutiveCorrect >= 5)
            {
                //TriggerFiveCorrectReward();
                PlayerPrefs.SetInt("lianxu", correctCount);
                PlayerPrefs.Save();
            }
        }
        else
        {
            crossmark.SetActive(true);
            wrongCount++;
            consecutiveCorrect = 0;  // ❌ 答错则清零连对
            //fiveCorrectText.gameObject.SetActive(false);
        }

        isWaiting = true;
    }

    public void lllllll()
    {
        jjjjjjj.SetActive(true);
        int lianxu = PlayerPrefs.GetInt("lianxu");


        if (consecutiveCorrect == 5)
        {
            obj111.SetActive(true);
        }
        if(consecutiveCorrect == 10)
        {
            obj111.SetActive(true);
            obj222.SetActive(true);
        }
        if (consecutiveCorrect == 20)
        {
            obj111.SetActive(true);
            obj222.SetActive(true);
            obj333.SetActive(true);
        }
        if (consecutiveCorrect == 30)
        {
            obj111.SetActive(true);
            obj222.SetActive(true);
            obj333.SetActive(true);
            obj444.SetActive(true);
        }
        if (consecutiveCorrect == 50)
        {
            obj111.SetActive(true);
            obj222.SetActive(true);
            obj333.SetActive(true);
            obj444.SetActive(true);
            obj555.SetActive(true);
        }
    }
    public void ggggggg()
    {
        jjjjjjj.SetActive(false);
    }
    private void HideFiveCorrectText()
    {
        fiveCorrectText.gameObject.SetActive(false);
    }

    public void StartCountdown()
    {
        isCounting = true;
    }

    public void StopCountdown()
    {
        isCounting = false;
    }

    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnCountdownFinished()
    {
        jieshuobj.SetActive(true);
        zhengquetext.text = "正确次数: " + correctCount;
        cuowutext.text = "错误次数: " + wrongCount;
        PlayerPrefs.SetInt("di1", correctCount);
        PlayerPrefs.Save();
        if (correctCount < 20)
        {
            xing1.SetActive(true);
            guli.text = "别灰心，下次会更棒";
            PlayerPrefs.SetInt("time", 5);
        }
        else if (correctCount > 20 && correctCount < 40)
        {
            xing2.SetActive(true);
            xing1.SetActive(true);
            xing3.SetActive(false);
            guli.text = "你真棒，争取做到100分哦";
            PlayerPrefs.SetInt("time", 10);
        }
        else if (correctCount > 40)
        {
            xing3.SetActive(true);
            xing1.SetActive(true);
            xing2.SetActive(true);
            guli.text = "太棒了，奖励你一朵小花花";
            PlayerPrefs.SetInt("time", 15);
        }
        PlayerPrefs.Save();
    }

    public void ClickCHONGWAN()
    {
        SceneManager.LoadScene("Numberinposition1");
    }

    public void CCCCCC()
    {
        SceneManager.LoadScene("Numberinposition");
    }
}
