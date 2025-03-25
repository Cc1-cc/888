using UnityEngine;

public class PreserveObjectOnLoad : MonoBehaviour
{



    // 在场景加载时调用
    void Awake()
    {
        // 保持物体在加载新场景时不被销毁
        DontDestroyOnLoad(gameObject);
    }
}
