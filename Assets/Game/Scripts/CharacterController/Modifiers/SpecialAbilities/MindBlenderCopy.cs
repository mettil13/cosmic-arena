using System.Collections.Generic;
using UnityEngine;
using CommonLogic;

public class MindBlenderCopy : MonoBehaviour
{
    private CharacterManager enemyHitted;
    private CharacterManager spawner;
    private float damage;
    private float radius;
    private CommonLogic.Timer timer;

    public void SetupCopy(CharacterManager spawner, float damage, float radius, float duration) {
        timer = new CommonLogic.Timer(duration);
        timer.OnEnd += () => Destroy(gameObject);
        this.damage = damage;
        this.radius = radius;
        this.spawner = spawner;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer.Update(Time.fixedDeltaTime);
        List<CharacterManager> hitted = new List<CharacterManager>();
        foreach (Collider hit in Physics.OverlapSphere(transform.position, radius, LayerMask.NameToLayer("player"))) {
            if (hit.TryGetComponent(out CharacterManager hittedCharacterManager)) {
                if (gameObject != hittedCharacterManager.gameObject && spawner != hittedCharacterManager) {
                    hitted.Add(hittedCharacterManager);
                }
            }
        }
        float minDistance = 0;
        foreach (CharacterManager enemy in hitted) {
            float distance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (distance < minDistance) {
                minDistance = distance;
                enemyHitted = enemy;
            }
        }
        if (enemyHitted == null) return;

        timer.Stop();
        transform.position = Vector3.Lerp(transform.position, enemyHitted.transform.position, Time.deltaTime * 10);
        transform.LookAt(enemyHitted.transform);

        if ((transform.position - enemyHitted.transform.position).magnitude < 0.1) {

            enemyHitted.stateMachine.AddModifier(new DamageModifier(1, enemyHitted, damage));
            timer.EndTimer();

        }
    }
}
