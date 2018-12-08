using AstroWorld.Extras;
using UnityEngine;

namespace AstroWorld.Enemies.Creature
{
    public static class CreatureHelpers
    {
        public static bool IsAngleWithinToleranceLevel(float normalizedAngle, float angleTolerance)
        {
            if (normalizedAngle < 0)
                return false;

            if (normalizedAngle <= angleTolerance)
                return true;

            return false;
        }

        public static float CheckTargetInsideFOV(Transform target,
                  float minimumDetectionDistance, float maxLookAngle,
                  Transform lookingPoint)
        {
            if (target == null)
                return -1;

            float distanceToPlayer = Vector3.Distance(target.position, lookingPoint.position);
            if (distanceToPlayer > minimumDetectionDistance)
                return -1;

            Vector3 modifiedPlayerPosition = new Vector3(target.position.x, 0, target.position.z);
            Vector3 modifiedLookingPosition =
                new Vector3(lookingPoint.position.x, 0, lookingPoint.position.z);

            Vector3 lookDirection = modifiedPlayerPosition - modifiedLookingPosition;
            float angleToPlayer = Vector3.Angle(lookDirection, lookingPoint.forward);
            float normalizedAngle = ExtensionFunctions.To360Angle(angleToPlayer);

            if (normalizedAngle <= maxLookAngle)
                return normalizedAngle;
            else
                return -1;
        }
    }
}