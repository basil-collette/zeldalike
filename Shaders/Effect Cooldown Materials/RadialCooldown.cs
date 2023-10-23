using System.Collections;
using UnityEngine;

public class RadialCooldown : MonoBehaviour
{
    Material _mat;
    Coroutine _cooldownCoroutine;

    float _coolddownDuration;
    float _cooldown;

    void Start()
    {
        _mat = GetComponent<SpriteRenderer>().material;
    }

    private void OnDisable()
    {
        ResetComp();
    }

    void ResetComp()
    {
        _coolddownDuration = 0;
        _cooldown = 0;
        _cooldownCoroutine = null;
    }

    public void InitOrReset(float duration)
    {
        _coolddownDuration = duration;
        _cooldown = duration;

        if (_cooldownCoroutine == null)
        {
            _cooldownCoroutine = StartCoroutine(CooldownCo());
        }       
    }

    IEnumerator CooldownCo()
    {
        while (_cooldown > 0)
        {
            float cooldownPercentile = _cooldown * 100 / _coolddownDuration;
            _mat.SetFloat("_Arc1", cooldownPercentile);

            yield return null;
        }

        gameObject.SetActive(false);
    }

}
