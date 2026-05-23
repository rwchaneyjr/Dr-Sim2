using UnityEngine;

public class CubeGridSpawner : MonoBehaviour
{
    public static CubeGridSpawner Instance { get; private set; }

    [Header("Room Prefab")]
    public GameObject cubePrefab;

    [Header("Target Prefab")]
    public GameObject targetPrefab;

    [Header("Roof Pad")]
    public bool spawnRoofPad = true;
    public float roofPadYOffset = 8f;

    [Header("Room Scale")]
    public float roomScale = 425f;

    [Header("Grid Size")]
    public int rows = 3;
    public int columns = 3;

    [Header("Spacing")]
    public float spacingMultiplier = 2.5f;

    [Header("Start Position")]
    public Vector3 startPosition = new Vector3(20f, 0f, 0f);

    [Header("Room Rotation")]
    public Vector3 roomRotation = new Vector3(-90f, 90f, 0f);

    [Header("Patient Move")]
    public Vector3 patientTargetOffset = new Vector3(0f, 0f, 0f);
    public GameObject patientPrefab;
    public Transform patientPlacementPoint;
    public string patientPlacementChildName = "CameraOffsetcube";

    [Header("Camera Move")]
    public Camera cameraToMove;
    public Vector3 cameraTargetOffset = new Vector3(0f, 8f, -8f);
    public bool moveCameraToTarget = true;

    [Header("Pad Light")]
    public bool spawnPadLight = true;
    public float padLightHeight = .3f;
    public float padLightRange = 12f;
    public float padLightIntensity = 1.5f;
    public Color padLightColor = new Color(1f, 0.95f, 0.8f);

    public RoomController[,] grid;

    public int activeRow = 0;
    public int activeCol = 1;

    [Header("Forward Direction")]
    public int northRowStep = 1;

    [Header("Wrong Answer Direction")]
    public int wrongAnswerLeftColStep = -1;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (cubePrefab == null || targetPrefab == null)
        {
            Debug.LogError("Missing prefab assignments!");
            return;
        }

        if (cameraToMove == null)
            cameraToMove = Camera.main;

        grid = new RoomController[rows, columns];

        SpawnGrid();
    }

    void SpawnGrid()
    {
        Renderer rend = cubePrefab.GetComponentInChildren<Renderer>();
        Vector3 size = rend != null ? rend.bounds.size : new Vector3(13f, 13f, 13f);

        float spacingX = size.x * spacingMultiplier;
        float spacingZ = size.z * spacingMultiplier;

        Quaternion rotation = Quaternion.Euler(roomRotation);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 spawnPos = startPosition + new Vector3(
                    col * spacingX,
                    0f,
                    row * spacingZ
                );

                RoomController room = SpawnRoom(spawnPos, rotation);

                room.row = row;
                room.col = col;

                grid[row, col] = room;
            }
        }
    }

    RoomController SpawnRoom(Vector3 position, Quaternion rotation)
    {
        GameObject newCube = Instantiate(cubePrefab, position, rotation);
        newCube.transform.localScale = new Vector3(roomScale, roomScale, roomScale);

        RoomController room = newCube.AddComponent<RoomController>();

        Vector3 roomCenter = newCube.transform.position;
        Vector3 targetOffset = new Vector3(0f, 0.872f, 0f);

        GameObject target = Instantiate(
            targetPrefab,
            roomCenter - targetOffset,
            Quaternion.identity
        );

        Patient patient = FindObjectOfType<Patient>();

        if (patient != null)
        {
            int conditionCount =
                System.Enum.GetValues(typeof(Patient.Condition)).Length;

            int rand = Random.Range(1, conditionCount);

            patient.SetCondition((Patient.Condition)rand);

            Debug.Log("Assigned condition: " + patient.currentCondition);
        }

        target.transform.localScale = new Vector3(2f, 0.02f, 2f);

        SpawnRoofPad(target);
        SpawnPadLight(newCube, target);

        target.SetActive(false);

        room.target = target;
        target.transform.SetParent(null);

        return room;
    }

    void SpawnRoofPad(GameObject floorPad)
    {
        if (!spawnRoofPad || floorPad == null)
            return;

        GameObject roofPad = Instantiate(
            targetPrefab,
            floorPad.transform.position + Vector3.up * roofPadYOffset,
            floorPad.transform.rotation
        );

        roofPad.name = "Roof Pad";
        roofPad.tag = "Untagged";
        roofPad.transform.localScale = floorPad.transform.localScale;
        roofPad.transform.SetParent(floorPad.transform, true);
        roofPad.transform.position= new Vector3(roofPad.transform.position.x, roofPad.transform.position.y+1.48f, roofPad.transform.position.z);
        foreach (Collider collider in roofPad.GetComponentsInChildren<Collider>())
            collider.enabled = false;

        foreach (CureTargetTrigger trigger in roofPad.GetComponentsInChildren<CureTargetTrigger>())
            trigger.enabled = false;
    }

    void SpawnPadLight(GameObject roomObject, GameObject target)
    {
        if (!spawnPadLight || target == null)
            return;

        GameObject lightObject = new GameObject("Pad Light");
        lightObject.transform.position = target.transform.position + Vector3.up * padLightHeight;
        lightObject.transform.SetParent(roomObject.transform, true);

        Light padLight = lightObject.AddComponent<Light>();
        padLight.type = LightType.Point;
        padLight.range = padLightRange;
        padLight.intensity = padLightIntensity;
        padLight.color = padLightColor;
        padLight.shadows = LightShadows.None;
    }

    public void HideAllTargets()
    {
        foreach (RoomController room in grid)
        {
            if (room != null)
                room.HideTarget();
        }
    }

    public void ShowNextForwardTarget()
    {
        RoomController currentRoom = FindActiveTargetRoom();

        if (currentRoom == null)
        {
            Debug.LogWarning("No active target found. Showing first room target.");
            HideAllTargets();

            if (grid[0, 0] != null)
                grid[0, 0].ShowTarget();

            return;
        }

        int nextRow = currentRoom.row + northRowStep;
        int nextCol = currentRoom.col;

        if (nextRow < 0 || nextRow >= rows)
        {
            Debug.Log("All rooms completed.");
            return;
        }

        RoomController nextRoom = grid[nextRow, nextCol];

        if (nextRoom == null)
        {
            Debug.LogError("No room found at row " + nextRow + ", col " + nextCol);
            return;
        }

        HideAllTargets();
        nextRoom.ShowTarget();

        PutPatientOnTarget(nextRoom.target);

        Debug.Log("Moved target north from " + currentRoom.row + "," + currentRoom.col +
                  " to " + nextRow + "," + nextCol);
    }

    public void MovePlayerLeft()
    {
        ShowLeftTargetForWrongAnswer();
    }

    public void ShowLeftTargetForWrongAnswer()
    {
        RoomController currentRoom = FindActiveTargetRoom();

        if (currentRoom == null)
        {
            Debug.LogWarning("No active target found. Cannot move left.");
            return;
        }

        int nextRow = currentRoom.row;
        int nextCol = currentRoom.col + wrongAnswerLeftColStep;

        if (nextCol < 0 || nextCol >= columns)
        {
            Debug.Log("No room to the left.");
            return;
        }

        RoomController nextRoom = grid[nextRow, nextCol];

        if (nextRoom == null)
        {
            Debug.LogError("No room found at row " + nextRow + ", col " + nextCol);
            return;
        }

        HideAllTargets();
        nextRoom.ShowTarget();

        PutPatientOnTarget(nextRoom.target);

        Debug.Log("Wrong answer moved patient left from " + currentRoom.row + "," + currentRoom.col +
                  " to " + nextRow + "," + nextCol);
    }

    RoomController FindActiveTargetRoom()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                RoomController room = grid[row, col];

                if (room != null && room.target != null && room.target.activeSelf)
                    return room;
            }
        }

        return null;
    }

    void PutPatientOnTarget(GameObject target)
    {
        if (target == null)
            return;

        Patient patient = FindObjectOfType<Patient>();

        if (patient == null)
        {
            Debug.LogWarning("No Patient found in scene.");
            return;
        }

        MovePatientToTarget(patient, target);

        DoctorTool tool = FindObjectOfType<DoctorTool>();
        if (tool != null)
        {
            tool.SelectPatient(patient);
            Debug.Log("GAME LOOP RESTARTED IN NEXT ROOM");
        }
    }

    void MovePatientToTarget(GameObject target)
    {
        if (target == null)
            return;

        Patient patient = FindObjectOfType<Patient>();

        if (patient == null)
            return;

        MovePatientToTarget(patient, target);
    }

    void MovePatientToTarget(Patient patient, GameObject target)
    {
        Transform placementPoint = GetPatientPlacementPoint(patient);
        Vector3 targetPosition = target.transform.position + patientTargetOffset;
        Vector3 movementDelta = targetPosition - placementPoint.position;

        patient.transform.position += movementDelta;
    }

    Transform GetPatientPlacementPoint(Patient patient)
    {
        if (patientPlacementPoint != null)
            return patientPlacementPoint;

        if (!string.IsNullOrEmpty(patientPlacementChildName))
        {
            Transform childPlacementPoint = patient.transform.Find(patientPlacementChildName);

            if (childPlacementPoint != null)
                return childPlacementPoint;
        }

        return patient.transform;
    }

    void MoveCameraToTarget(GameObject target)
    {
        if (!moveCameraToTarget || target == null)
            return;

        if (cameraToMove == null)
            cameraToMove = Camera.main;

        if (cameraToMove == null)
            return;

        cameraToMove.transform.position = target.transform.position + cameraTargetOffset;
        cameraToMove.transform.LookAt(target.transform);
    }
}