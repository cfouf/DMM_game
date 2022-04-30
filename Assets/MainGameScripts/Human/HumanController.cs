using UnityEngine;

namespace MainGameScripts
{
    public class HumanController : MonoBehaviour
    {
        public int EyeDirection;
        public float speed;
        public Vector2 moveDirection;
        public Rigidbody2D currentRb;
        public Vector2 homePosition;
        public GameObject[] pathPoints;

        private int currentPathPointIndex;

        private GameObject Player;

        private float pauseStart;
        private const float pauseTime = 1f;

        private Stage currentStage;


        private enum Stage
        {
            Moving,
            Pause,
            Following
        }

        private void Start()
        {
            speed = 8f;
            EyeDirection = 1;
            pauseStart = Time.time;
            currentPathPointIndex = 0;
            currentStage = Stage.Pause;
            UpdateMoveDirection(pathPoints[currentPathPointIndex].transform.position);
        }

        private void FixedUpdate()
        {
            switch (currentStage)
            {
                case Stage.Pause:
                    var difference = Time.time - pauseStart;
                    if (difference >= pauseTime)
                    {
                        currentStage = Stage.Moving;
                        EyeDirection = (int)Mathf.Sign(moveDirection.x);
                    }
                    break;
                case Stage.Moving:
                    Move();
                    break;
                case Stage.Following:
                    Follow();
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            col.gameObject.GetComponent<Rigidbody2D>().WakeUp();
        }


        private void OnTriggerStay2D(Collider2D col)
        {
            if (!col.gameObject.GetComponent<Rigidbody2D>())
                return;

            var rb = col.gameObject.GetComponent<Rigidbody2D>();

            if (Mathf.Sign(rb.position.x - currentRb.position.x) == Mathf.Sign(EyeDirection))
            {
                if (col.gameObject.layer == 6)
                    if (rb.velocity.magnitude > 0.1f)
                    {
                        Player = col.gameObject;
                        currentStage = Stage.Following;
                    }
            }
        }

        private void Move()
        {
            currentRb.velocity = moveDirection * speed;

            if (Mathf.Sign(pathPoints[currentPathPointIndex].transform.position.x - transform.position.x) == Mathf.Sign(moveDirection.x))
                return;

            currentRb.velocity = Vector3.zero;
            currentPathPointIndex = GetLoopSum(currentPathPointIndex, 1, pathPoints.Length);
            UpdateMoveDirection(pathPoints[currentPathPointIndex].transform.position);
            currentStage = Stage.Pause;
            pauseStart = Time.time;
        }

        private void Follow()
        {
            UpdateMoveDirection(Player.transform.position);
            currentRb.velocity = moveDirection * speed;
            return;

            if (Mathf.Abs(Player.transform.position.x - transform.position.x) > 2f)
                return;           
            
            

            currentRb.velocity = Vector3.zero;
            currentStage = Stage.Pause;
            pauseStart = Time.time;
        }

        private int GetLoopSum(int a, int b, int maxValue)
            => (maxValue + a + b) % maxValue;

        private float GetDistance2D(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(new Vector3(a.x, a.y, 0), new Vector3(b.x, b.y));
        }

        private void UpdateMoveDirection(Vector3 target)
        {
            moveDirection = new Vector2(
                Mathf.Sign(target.x - currentRb.position.x),
                0f
                );
        }
    }
}
