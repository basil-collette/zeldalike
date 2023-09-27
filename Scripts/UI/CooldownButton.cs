using System.Collections.Generic;
using UnityEngine;

public class CooldownButton : MonoBehaviour
{
    [HideInInspector] public static List<CooldownButton> buttons = new List<CooldownButton>();

    public string _name;
    public Animator _anim;

    private void Start()
    {
        buttons.Add(this);
    }

    private void OnDestroy()
    {
        buttons.Remove(this);
    }

    public void Cooldwon(float speed)
    {
        _anim.speed = 1 / speed;
        _anim.SetTrigger("cooldown");
    }

}
