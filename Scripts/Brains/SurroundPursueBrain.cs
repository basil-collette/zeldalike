using UnityEngine;

public class SurroundPursueBrain : PursueBrain
{

    public override Vector3? Think(ThinkParam? param = null)
    {
        Vector3 direction = base.Think(param) ?? Vector3.zero;

        if (direction != Vector3.zero)
        {
            return direction;
        }

        Transform target = ((TargetThinkParam)param).target;
        direction = DirectionHelper.GetDirection(transform.position, target.position);

        return base.GetAvoidingAngledDirection(direction, 90);
    }

}
