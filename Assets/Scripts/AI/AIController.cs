using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            RandomPatrol,
            RoutePatrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;
        [SerializeField] private AIPointPatrol m_RandomPatrolPoint;
        [SerializeField] private AIRoutePoint[] m_RoutePoints;

        [SerializeField] private bool m_UseLead;
        [Range(0.0f, 10.0f)]
        [SerializeField] private float LeadDistance;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        private SpaceShip m_Ship;

        private Vector3 m_MovePosition;
        private AIRoutePoint m_RouteTargetPoint;

        private Destructible m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private void Start()
        {
            m_Ship = GetComponent<SpaceShip>();

            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.RandomPatrol)
            {
                UpdateBehaviourPatrol();
            }

            if (m_AIBehaviour == AIBehaviour.RoutePatrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }

        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.RandomPatrol)
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = CheckLead(m_MovePosition, m_SelectedTarget);
                }

                else
                {
                    if (m_RandomPatrolPoint != null)
                    {
                        bool isInsidePatrolZone = (m_RandomPatrolPoint.transform.position - transform.position).sqrMagnitude < m_RandomPatrolPoint.Radius * m_RandomPatrolPoint.Radius;

                        if (isInsidePatrolZone == true)
                        {
                            if (m_RandomizeDirectionTimer.IsFinished == true)
                            {
                                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_RandomPatrolPoint.Radius + m_RandomPatrolPoint.transform.position;

                                m_MovePosition = newPoint;

                                m_RandomizeDirectionTimer.RestartTimer();
                            }
                        }

                        else
                        {
                            m_MovePosition = m_RandomPatrolPoint.transform.position;
                        }
                    }
                }
            }

            if (m_AIBehaviour == AIBehaviour.RoutePatrol)
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = CheckLead(m_MovePosition, m_SelectedTarget);
                    m_RouteTargetPoint = null;
                }

                else
                {
                    if (m_RoutePoints.Length > 0)
                    {
                        if (m_RouteTargetPoint == null)
                        {
                            m_RouteTargetPoint = NearestTarget();
                        }

                        else
                        {
                            bool isRouteTargetPointReached = (m_RouteTargetPoint.transform.position - transform.position).sqrMagnitude < m_RouteTargetPoint.Radius * m_RouteTargetPoint.Radius;

                            if (isRouteTargetPointReached == true)
                            {
                                for (int i = 0; i < m_RoutePoints.Length; i++)
                                {
                                    if (m_RouteTargetPoint == m_RoutePoints[i])
                                    {
                                        if (i + 1 == m_RoutePoints.Length)
                                        {
                                            m_RouteTargetPoint = m_RoutePoints[0];
                                        }

                                        else
                                        {
                                            m_RouteTargetPoint = m_RoutePoints[i + 1];
                                        }

                                        break;
                                    }
                                }
                            }
                        }

                        m_MovePosition = m_RouteTargetPoint.transform.position;
                    }

                    else
                    {
                        Debug.LogError("Нет маршрута");
                    }
                }
            }
        }

        private AIRoutePoint NearestTarget()
        {
            AIRoutePoint nearest = m_RoutePoints[0];

            float maxDist = float.MaxValue;

            for(int i = 0; i < m_RoutePoints.Length; i++)
            {
                float dist = Vector2.Distance(m_Ship.transform.position, m_RoutePoints[i].transform.position);

                if (dist < maxDist)
                {
                    maxDist = dist;
                    nearest = m_RoutePoints[i];
                }
            }

            return nearest;
        }

        private Vector3 CheckLead(Vector3 position, Destructible target)
        {
            position = target.transform.position;

            if (m_UseLead == true)
            {
                position += target.transform.up * LeadDistance * target.GetComponent<SpaceShip>().ThrustControl;
            }

            return position;
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + transform.right * 1000.0f;
            }
        }

        private void ActionControlShip()
        {
            m_Ship.ThrustControl = m_NavigationLinear;

            m_Ship.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_Ship.transform) * m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;

        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.RestartTimer();
            }
        }

        private void ActionFire()
        {
            bool canFire = IsCanFire();

            if (m_SelectedTarget != null && canFire == true)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_Ship.Fire(TurretMode.Primary);

                    m_FireTimer.RestartTimer();
                }
            }
        }

        private bool IsCanFire()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, float.MaxValue);

            if (hit && hit.collider.transform.root.GetComponent<Destructible>() != null)
            {
                if (hit.collider.transform.root.GetComponent<Destructible>().TeamId != m_Ship.TeamId &&
                    hit.collider.transform.root.GetComponent<Destructible>().TeamId != Destructible.TeamIDNeutral)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            else
            {
                return false;
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_Ship) continue;
                if (v.TeamId == Destructible.TeamIDNeutral) continue;
                if (v.TeamId == m_Ship.TeamId) continue;

                float dist = Vector2.Distance(m_Ship.transform.position, v.transform.position);
                if (dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }
        #endregion

        public void SetRandomPatrolBehaviour()
        {
            m_AIBehaviour = AIBehaviour.RandomPatrol;
            m_RandomPatrolPoint = FindObjectOfType<AIPointPatrol>();
        }

        public void SetRoutePatrolBehaviour()
        {
            m_AIBehaviour = AIBehaviour.RoutePatrol;

            List<AIRoutePoint> route = FindObjectsOfType<AIRoutePoint>().ToList();

            route.Sort((l, r) => l.ID.CompareTo(r.ID));

            for (int i = 0; i < route.Count; i++)
            {
                m_RoutePoints[i] = route[i];
            }
        }
    }
}
