using UnityEngine;

public class FlyingEye : Enemy
{

    private void Update()
    {
        // Düşman hasar alabilirliği varsa AI çalışcak
        if (canTakeDamage)
        {
            AI();
        }
    }

    protected override void AI()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        // Mesafe takip mesafesinden küçükse oyuncu takip edilcek değilse rastgele yürüycek
        if (distance < followingDistance)
        {
            FollowPlayer(distance);
        }
        else
        {
            Patrol();
        }

        Hurt();
    }

    protected override void Patrol()
    {
        LookAtPlayer(moveSpot.x);
        transform.position = Vector2.MoveTowards(transform.position, moveSpot, stats.moveSpeed * Time.deltaTime);

        // Rastgele noktaya yakın bir konuma varınca bir süre bekleme
        if (Vector2.Distance(transform.position, moveSpot) < 0.2f)
        {
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0)
            {
                // Yeni bir rastgele nokta belirleme
                moveSpot = EnemySpawner.instance.RandomPosition();
                currentWaitTime = waitTime;
            }
        }
    }

    protected override void FollowPlayer(float distance)
    {
        LookAtPlayer(target.position.x);
        if (distance > stoppingDistance && canAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, stats.moveSpeed * Time.deltaTime);
        }
        else if (Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, playerLayers).Length == 0 && transform.position != newTarget.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, newTarget.position, stats.moveSpeed * Time.deltaTime);
        }
        else
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        currentAttackRate -= Time.deltaTime;

        if (currentAttackRate <= 0 && canAttack)
        {
            animator.SetTrigger(EnemyAnimationParametres.attack);
            canAttack = false;
        }
    }

    private void DealDamage()
    {
        // Belirli bir yarıçapta saldırı yapacğı birim sayısı 0 değilse Oyuncuya hasar veriyor
        if (Physics2D.OverlapCircleAll(attackPoint.position, stats.attackRange, playerLayers).Length != 0)
        {
            Player.instance.TakeDamage(stats.attack);
        }
        currentAttackRate = stats.attackRate;
        canAttack = true;
    }

    private void FixAttack()
    {
        currentAttackRate = stats.attackRate;
        canAttack = true;
    }

    private void FallDamage()
    {
        // Uçan düşman olduğundan platformdan düşemez
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, 0.2f);
    }
}