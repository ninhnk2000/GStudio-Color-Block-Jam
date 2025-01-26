using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtil
{
    public static bool IsReachedDestination(NavMeshAgent navMeshAgent)
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
