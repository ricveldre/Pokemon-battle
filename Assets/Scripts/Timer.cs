using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private List<Sprite> timerTextures;
    [SerializeField]
    private List<string> _soundName;
    [SerializeField]
    private Image _timerImage;
    [SerializeField]
    private string showTimerAnimationName = "ShowSecond";
    [SerializeField]
    private UnityEvent _onSecondPassed;
    [SerializeField]
    private UnityEvent _onTimerFinished;
    private Coroutine _timerCoroutine;
    public void StartTimer(float duration)
    {
        _timerCoroutine = StartCoroutine(TimerCoroutine(duration));
    }
    private IEnumerator TimerCoroutine(float duration)
    {
    while (duration >= 0)
        {
            int index = Mathf.FloorToInt(duration);
            _onSecondPassed?.Invoke();
            _timerImage.sprite = timerTextures[index];
            _animator.Play(showTimerAnimationName);
            duration--;
            SoundManager.instance.Play(_soundName[index]);
            yield return new WaitForSeconds(1f);
        }
        _onTimerFinished.Invoke();
    }
    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }
}
