using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    //public KeyCode key;

    AbilityState _state = AbilityState.ready;
    float _cooldownTime;
    float _activeTime;

    static bool _needActivation = false;
    public static void Activate() => _needActivation = true;

    void Update()
    {
        switch (_state)
        {
            case AbilityState.ready:
                if (_needActivation /*Input.GetKeyDown(key)*/)
                {
                    _needActivation = false;
                    ability.Activate(gameObject);
                    _state = AbilityState.active;
                    _activeTime = ability._activeTime;
                }
                break;

            case AbilityState.active:
                if (_activeTime > 0)
                {
                    _activeTime -= Time.deltaTime;
                }
                else
                {
                    ability.AfterActivate(gameObject);
                    _state = AbilityState.cooldown;
                    _cooldownTime = ability._cooldownTime;
                }
                break;

            case AbilityState.cooldown:
                if (_cooldownTime > 0)
                {
                    _cooldownTime -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.ready;
                }
                break;

            default:
                break;
        }
    }

}