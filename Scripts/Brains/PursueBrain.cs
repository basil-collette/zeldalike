using UnityEngine;

public class PursueBrain : Brain
{
    public string targetTag = "Player";
    public GameObject target;
    public bool avoidObstacle = true;
    public float minRange = 1.5f;
    public float avoidDistance = 4;
    public LayerMask obstacleLayer;

    protected Animator animator;
    protected AliveEntity entityComp;

    private void Start()
    {
        this.target = GameObject.FindGameObjectWithTag(targetTag);

        this.animator = GetComponent<Animator>();
        this.entityComp = GetComponent<AliveEntity>();
    }

    public override Vector3? Think(ThinkParam? param = null)
    {
        float distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);
        
        if (distanceFromTarget < minRange)
        {
            return Vector3.zero;
        }

        return target.transform.position;

        /*
        Vector3 direction = target.transform.position - transform.position;
        float distanceToPlayer = direction.magnitude;
        direction.Normalize();

        if (avoidObstacle && distanceToPlayer <= detectionRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distanceToPlayer, obstacleLayer);

            if (hit.collider != null
                && hit.distance <= avoidDistance
            ) {
                Vector3 avoidDirection = Vector3.Cross(Vector3.forward, direction);
                
                if (hit.normal.x > 0)
                {
                    direction = avoidDirection *= -1;
                }
            }
        }

        return direction;
        */
    }

    public override short? Behave(BehaveParam? param = null)
    {
        Vector3 direction = ((PursueBehaveParam)param).direction;

        //pursueComp.MoveTowardsTarget(direction);
        transform.position = Vector2.MoveTowards(transform.position, direction, entityComp.moveSpeed * Time.fixedDeltaTime);
        //rigidBody.velocity = Vector2.MoveTowards(transform.position, direction, entityComp.moveSpeed * Time.fixedDeltaTime);

        Vector3 directionAnim = (direction - transform.position).normalized;

        animator.SetBool("moving", true);
        animator.SetFloat("moveX", directionAnim.x);
        animator.SetFloat("moveY", directionAnim.y);

        return null;
    }

}
