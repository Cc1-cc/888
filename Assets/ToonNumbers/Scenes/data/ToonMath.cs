using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToonNumbers
{
    public class ToonMath : MonoBehaviour
    {
        public GameObject Numbers;
        public GameObject Signs;
        GameObject[] operation;
        int gamePhase;
        int[] values;
        int[] operators;
        float result;
        int guess;
        public GameObject Texts;

        const int MAX_RESULT = 99;
        const int MIN_RESULT = 0;

        int consecutiveCorrectCount = 0; // 连续答对次数


        // 倒计时的总时间（秒）
        public float totalTime = 60f;

        // 显示倒计时的UI文本
        public TextMeshProUGUI countdownText;

        // 内部计时器
        private float currentTime;

        // 是否开始倒计时
        private bool isCounting = false;


        public GameObject jieshuobj;

        public GameObject chengjiuobj;

        public GameObject obj1;
        public GameObject obj2;
        public GameObject obj3;
        public GameObject obj4;
        public GameObject obj5;

        public GameObject addtime;

        void Start()
        {

            // 初始化当前时间
            currentTime = totalTime;

            // 开始倒计时
            StartCountdown();
            result = 1000f;
            gamePhase = 0;
            Texts = Instantiate(Texts, transform.position, transform.rotation, transform);
            operation = new GameObject[8];
            for (int i = 0; i < 8; i += 2)
                operation[i] = Instantiate(Numbers, transform.position - Vector3.right * ((i - 4f)), transform.rotation, transform);
            operation[7] = Instantiate(Numbers, transform.position + Vector3.right * (-3f), transform.rotation, transform);
            for (int i = 1; i < 7; i += 2)
                operation[i] = Instantiate(Signs, transform.position - Vector3.right * ((i - 4f)) + transform.forward * -0.25f, transform.rotation, transform);
        }


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

        void Update()
        {
            if (isCounting)
            {
                // 更新当前时间
                currentTime -= Time.deltaTime;

                // 如果时间小于0，停止倒计时
                if (currentTime <= 0)
                {
                    currentTime = 0;
                    isCounting = false;
                    showjieshu();
                }

                // 更新UI显示
                UpdateCountdownDisplay();
            }
            if (Input.anyKey && gamePhase == 0)
            {
                GenerateNewProblem();
                gamePhase++;
                StartCoroutine("Delayed", 2f);
                Texts.GetComponent<FloatingTexts>().Guess();
            }

            if (gamePhase == 2)
            {
                HandleInputForStage2();
            }

            if (gamePhase == 4)
            {
                HandleInputForStage4();
            }

            if (gamePhase == 6)
            {
                CheckAnswer();
            }

            if (gamePhase == 8)
            {
                ResetGame();
            }

            if (Input.GetKeyDown("x"))
            {
                StopAllCoroutines();
                ResetGame();
            }
        }

        void GenerateNewProblem()
        {
            while (result < MIN_RESULT || result > MAX_RESULT || result != Mathf.Floor(result))
            {
                result = 0;
                values = new int[3] { Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10) };
                operators = new int[3] { Random.Range(0, 4), Random.Range(0, 2), 4 }; // 第二位运算符只能是加或减

                // 避免除以零的情况
                if (operators[0] == 3 && values[1] == 0) values[1]++;
                if (operators[0] == 3 && values[0] == 0) values[0]++;
                if (operators[1] == 3 && values[2] == 0) values[2]++;
                if (operators[1] == 3 && values[1] == 0) values[1]++;

                CalculateResult();
            }

            operation[0].GetComponent<numbers>().letsgo(values[0]);
            operation[1].GetComponent<signs>().letsgo(operators[0]);
            operation[2].GetComponent<numbers>().letsgo(values[1]);
            operation[3].GetComponent<signs>().letsgo(operators[1]);
            operation[4].GetComponent<numbers>().letsgo(values[2]);
            operation[5].GetComponent<signs>().letsgo(operators[2]);
        }

        void CalculateResult()
        {
            // 处理第一个运算符（优先级高的乘除）
            if (operators[0] == 0) result = values[0] + values[1];   // 加法
            if (operators[0] == 1) result = values[0] - values[1];   // 减法
            if (operators[0] == 2) result = values[0] * values[1];   // 乘法
            if (operators[0] == 3) result = (float)values[0] / values[1];  // 除法

            // 处理第二个运算符
            if (operators[1] == 0) result = result + values[2];   // 加法
            if (operators[1] == 1) result = result - values[2];   // 减法
            if (operators[1] == 2) result = result * values[2];   // 乘法
            if (operators[1] == 3) result = result / (float)values[2];   // 除法
        }

        void HandleInputForStage2()
        {
            if (result < 10)
            {
                for (int i = 0; i <= 9; i++)
                {
                    if (Input.GetKeyDown(i.ToString())) HandleInput(i);
                }
            }
            else
            {
                for (int i = 0; i <= 9; i++)
                {
                    if (Input.GetKeyDown(i.ToString())) HandleInputForStage2B(i);
                }
            }
        }

        void HandleInput(int inputValue)
        {
            guess = inputValue;
            operation[6].GetComponent<numbers>().letsgo(inputValue);
            for (int i = 0; i < 6; i += 2)
                operation[i].GetComponent<numbers>().Tension();
            gamePhase = 5;
            StartCoroutine("Delayed", 2f);
        }

        void HandleInputForStage2B(int inputValue)
        {
            Texts.GetComponent<FloatingTexts>().GuessMore();
            guess = inputValue * 10;
            operation[6].GetComponent<numbers>().letsgo(inputValue);
            for (int i = 0; i < 6; i += 2)
                operation[i].GetComponent<numbers>().Tension();
            gamePhase = 3;
            StartCoroutine("Delayed", 0.125f);
        }

        void HandleInputForStage4()
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    operation[7].GetComponent<numbers>().letsgo(i);
                    gamePhase++;
                    StartCoroutine("Delayed", 2f);
                    guess += i;
                }
            }
        }

        void CheckAnswer()
        {
            if (guess == result)
            {
                consecutiveCorrectCount++; // 增加连续答对次数
                for (int i = 0; i < 7; i += 2) operation[i].GetComponent<numbers>().Victory();
                Texts.GetComponent<FloatingTexts>().Correct();
                if (result > 10) operation[7].GetComponent<numbers>().Victory();
                Debug.Log("连续答对次数: " + consecutiveCorrectCount); // 打印连续答对次数
                if(consecutiveCorrectCount==5)
                {
                    addtime.SetActive(true);
                }
            }
            else
            {
                consecutiveCorrectCount = 0; // 答错时清零
                for (int i = 0; i < 7; i += 2) operation[i].GetComponent<numbers>().Fail();
                Texts.GetComponent<FloatingTexts>().Wrong();
                if (result > 10) operation[7].GetComponent<numbers>().Fail();
                Debug.Log("连续答对次数已清零");
            }
            gamePhase++;
            StartCoroutine("Delayed", 1f);
        }

        void ResetGame()
        {
            result = 1000f;
            gamePhase = 0;

            for (int i = 0; i < 7; i += 2) operation[i].GetComponent<numbers>().resetN();
            operation[6].GetComponent<numbers>().resetN();
            operation[7].GetComponent<numbers>().resetN();
            Texts.GetComponent<FloatingTexts>().Guess();
        }

        IEnumerator Delayed(float time)
        {
            yield return new WaitForSeconds(time);
            gamePhase++;
        }

        public void ShowChnegjiu()
        {
            chengjiuobj.SetActive(true);
            if (consecutiveCorrectCount == 5)
            {
                obj1.SetActive(true);
            }
            if (consecutiveCorrectCount >5&& consecutiveCorrectCount <= 10)
            {
                obj1.SetActive(true);
                obj2.SetActive(true);
            }
            if (consecutiveCorrectCount > 10 && consecutiveCorrectCount <= 20)
            {
                obj1.SetActive(true);
                obj2.SetActive(true);
                obj3.SetActive(true);
                obj4.SetActive(false);
                obj5.SetActive(false);
            }
            if (consecutiveCorrectCount > 20 && consecutiveCorrectCount <= 30)
            {
                obj1.SetActive(true);
                obj2.SetActive(true);
                obj3.SetActive(true);
                obj4.SetActive(true);
                obj5.SetActive(false);
            }
            if (consecutiveCorrectCount > 30 && consecutiveCorrectCount <= 40)
            {
                obj1.SetActive(true);
                obj2.SetActive(true);
                obj3.SetActive(true);
                obj4.SetActive(true);
                obj5.SetActive(false);
            }
        }
        public void Clickaddtime()
        {
            currentTime += 5;
            addtime.SetActive(false);
        }
        public void chongwanwannwa()
        {

            SceneManager.LoadScene("SceneMaths");
        }

        public void showjieshu()
        {
            jieshuobj.SetActive(true);
        }
        public void jieshuhoubtnclick()
        {
            PlayerPrefs.SetInt("di4", consecutiveCorrectCount);
            PlayerPrefs.Save();
            SceneManager.LoadScene("load");
        }

        public void Closechengjiu()
        {
            chengjiuobj.SetActive(false);
        }
    }
}