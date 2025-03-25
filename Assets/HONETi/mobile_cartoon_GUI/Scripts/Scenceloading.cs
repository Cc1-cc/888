using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenceloading : MonoBehaviour
{

    public GameObject setobj;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public GameObject star11;
    public GameObject star22;
    public GameObject star33;

    public GameObject star111;
    public GameObject star222;
    public GameObject star333;

    public GameObject star1111;
    public GameObject star2222;
    public GameObject star3333;

    public Button lv1;
    public Button lv2;
    public Button lv3;
    public Button lv4;
    // Start is called before the first frame update
    void Start()
    {
        int num1 = PlayerPrefs.GetInt("di1");
        int num2 = PlayerPrefs.GetInt("di2");
        int num3 = PlayerPrefs.GetInt("di3");
        int num4 = PlayerPrefs.GetInt("di4");

        if(num1<=5)
        {
            star1.SetActive(true);
            star2.SetActive(false);
            star3.SetActive(false);
        }
        if (num1 > 5&&num1<=10)
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(false);
        }
        if (num1 > 10 && num1 <= 20)
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        }

        if (num2 ==1)
        {
            lv2.interactable = true;
            star11.SetActive(true);
            star22.SetActive(true);
            star33.SetActive(true);
        }


        if (num3 <= 5)
        {
            lv3.interactable = true;
            star111.SetActive(true);
            star222.SetActive(false);
            star333.SetActive(false);
        }
        if (num3 > 5 && num3 <= 10)
        {
            star111.SetActive(true);
            star222.SetActive(true);
            star333.SetActive(false);
        }
        if (num3 > 10 && num3 <= 20)
        {
            star111.SetActive(true);
            star222.SetActive(true);
            star333.SetActive(true);
        }


        if (num4 <= 5)
        {
            lv4.interactable = true;
            star1111.SetActive(true);
            star2222.SetActive(false);
            star3333.SetActive(false);
        }
        if (num4 > 5 && num4 <= 10)
        {
            star1111.SetActive(true);
            star2222.SetActive(true);
            star3333.SetActive(false);
        }
        if (num4 > 10 && num4 <= 20)
        {
            star1111.SetActive(true);
            star2222.SetActive(true);
            star3333.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SETOPEN()
    {
        setobj.SetActive(true);
    }
    public void CloseOPEN()
    {
        setobj.SetActive(false);
    }

    public void Click1()
    {
        SceneManager.LoadScene("Numberinposition");
    }
    public void Click2()
    {
        SceneManager.LoadScene("Numberinposition1");
    }
    public void Click3()
    {
        SceneManager.LoadScene("Numberinposition2");
    }
    public void Click4()
    {
        SceneManager.LoadScene("SceneMaths");
    }
}
