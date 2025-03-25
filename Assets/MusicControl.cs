using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    public AudioSource audioSource;   // �������Ƶ���ƵԴ
    public Slider volumeSlider;       // ����������Slider

    public GameObject setobj;

    void Start()
    {
        // ��ʼ��SliderֵΪ��ǰ��ƵԴ������
        volumeSlider.value = audioSource.volume;

        // ��Ӽ���������Sliderֵ�仯ʱ��������
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ��Slider��ֵ�ı�ʱ����
    void OnVolumeChanged(float value)
    {
        audioSource.volume = value;  // ������ƵԴ������
    }

    public void openset()
    {
        setobj.SetActive(true);
    }

    public void closeset()
    {
        setobj.SetActive(false);
    }
}
