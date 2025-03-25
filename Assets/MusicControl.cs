using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    public AudioSource audioSource;   // 用来控制的音频源
    public Slider volumeSlider;       // 控制音量的Slider

    public GameObject setobj;

    void Start()
    {
        // 初始化Slider值为当前音频源的音量
        volumeSlider.value = audioSource.volume;

        // 添加监听器，当Slider值变化时更新音量
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // 当Slider的值改变时调用
    void OnVolumeChanged(float value)
    {
        audioSource.volume = value;  // 设置音频源的音量
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
