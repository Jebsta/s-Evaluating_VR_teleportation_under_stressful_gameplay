using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);
    public Vector3 minScale = new Vector3(0.3f, 0.3f, 0.3f);

    public float scaleSpeed = 1.5f;    // wie schnell Größe wechselt
    public float movementRange = 0.5f;
    public float movementSpeed = 0.5f;

    public ConditionManager conditionManager;

    private Vector3 targetPosition;
    private Vector3 startingPosition;
    private Vector3 currentTargetScale;

    private float newScaleCooldown = 0f;

    void Start()
    {
        startingPosition = transform.position;
        targetPosition = startingPosition;

        // Start mit normaler Größe
        currentTargetScale = normalScale;

        if (conditionManager == null)
            conditionManager = FindFirstObjectByType<ConditionManager>();
    }

    void Update()
    {
        if (conditionManager == null) return;

        if (conditionManager.currentMode == ConditionManager.StressMode.Shrinking)
        {
            RandomScaling();
            Move();
        }
        else
        {
            ResetScale();
            ResetMovement();
        }
    }

    // -------------------------------------------------------------
    // RANDOM SHRINK + GROW (innerhalb minScale → normalScale)
    // -------------------------------------------------------------
    void RandomScaling()
    {
        newScaleCooldown -= Time.deltaTime;
        if (newScaleCooldown <= 0f)
        {
            float t = Random.Range(0f, 1f);

            currentTargetScale = Vector3.Lerp(minScale, normalScale, t);

            newScaleCooldown = Random.Range(1f, 3f);
        }

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            currentTargetScale,
            Time.deltaTime * scaleSpeed
        );
    }

    // -------------------------------------------------------------
    // MOVEMENT
    // -------------------------------------------------------------
    void Move()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = startingPosition + new Vector3(
                Random.Range(-movementRange, movementRange),
                0,
                Random.Range(-movementRange, movementRange)
            );
        }

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * movementSpeed
        );
    }

    // -------------------------------------------------------------
    // RESET BEHAVIOUR
    // -------------------------------------------------------------
    void ResetScale()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            normalScale,
            Time.deltaTime * 3f
        );
    }

    void ResetMovement()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            startingPosition,
            Time.deltaTime * 3f
        );
    }
}
