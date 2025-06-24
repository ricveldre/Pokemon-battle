using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private List<Fighter> _fighters = new List<Fighter>();
    [SerializeField]
    private int _fightersNeededToStart = 2;
    [SerializeField]
    private UnityEvent _onBattleStarted;
    [SerializeField]
    private UnityEvent _onBattleEnded;
    private Coroutine _battleCoroutine;
    private DamageTarget _damageTarget = new DamageTarget();
    public void AddFighter(Fighter fighter)
    {
        _fighters.Add(fighter);
        if (_fighters.Count >= _fightersNeededToStart)
        {
            StartBattle();
        }
    }
    public void RemoveFighter(Fighter fighter)
    {
        _fighters.Remove(fighter);
        if (_battleCoroutine != null)
        {
            StopCoroutine(_battleCoroutine);
        }
    }
    public void InitialFighters()
    {
        foreach (var fighter in _fighters)
        {
            fighter.InitializedFighter();
        }
    }
    public void StartBattle()
    {
        InitialFighters();
        _battleCoroutine = StartCoroutine(BattleCoroutine());
    }
    private IEnumerator BattleCoroutine()
    {
        _onBattleStarted?.Invoke();
        while (_fighters.Count > 1)
        {
            Fighter attacker = _fighters[Random.Range(0, _fighters.Count)];
            Fighter defender = _fighters[Random.Range(0, _fighters.Count)];
            while (defender == attacker)
            {
                defender = _fighters[Random.Range(0, _fighters.Count)];
            }
            attacker.transform.LookAt(defender.transform);
            defender.transform.LookAt(attacker.transform);
            Attack attack = attacker.AttackData.attacks[Random.Range(0, attacker.AttackData.attacks.Length)];
            float damage = Random.Range(attack.minDamage, attack.maxDamage);
            attacker.CharacterAnimator.Play(attack.animationName);
            SoundManager.instance.Play(attack.soundName);
            GameObject attackParticles = Instantiate(attack.particlesPrefab, attacker.transform.position, Quaternion.identity);
            attackParticles.transform.SetParent(attacker.transform);
            yield return new WaitForSeconds(attack.attackDuration);
            GameObject hitParticles = Instantiate(attack.hitParticlesPrefab, defender.transform.position, Quaternion.identity);
            hitParticles.transform.SetParent(defender.transform);
            _damageTarget.SetDamageTarget(defender.transform, damage);
            defender.Health.TakeDamage(_damageTarget);
            if (defender.Health.CurrentHealth <= 0)
            {
                RemoveFighter(defender);
            }
            yield return new WaitForSeconds(2f);
        }
        _onBattleEnded?.Invoke();
    }
}
