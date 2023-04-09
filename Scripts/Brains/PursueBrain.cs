using UnityEngine;

public class PursueBrain : Brain
{
    public string targetTag = "Player";
    public GameObject target;
    public bool avoidObstacle = true;
    public float minRange = 1.5f;
    public float avoidDistance = 4;

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

        if (avoidObstacle)
        {
            //
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

    /*
    private void Update()
    {
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }

    // This method is called when the NavMeshAgent encounters an obstacle
    private void OnObstacleAvoidanceEvent(List<Vector3> corners)
    {
        // Recalculate path to avoid obstacle
        navMeshAgent.CalculatePath(target.position, new NavMeshPath());

        // Loop through new corners and set the path
        for (int i = 0; i < corners.Count - 1; i++)
        {
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.CalculatePath(corners[i + 1], path);
            navMeshAgent.SetPath(path);
        }
    }
    */

}
