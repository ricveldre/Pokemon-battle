using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private Animator _textAnimator;
    [SerializeField]
    private string _animationName;
    private Text _text;
    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    public void ShowDamage(float damage)
    {
        _text.text = damage.ToString("f0");
        _textAnimator.Play(_animationName);
    }
}
