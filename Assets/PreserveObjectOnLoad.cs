using UnityEngine;

public class PreserveObjectOnLoad : MonoBehaviour
{



    // �ڳ�������ʱ����
    void Awake()
    {
        // ���������ڼ����³���ʱ��������
        DontDestroyOnLoad(gameObject);
    }
}
