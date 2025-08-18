<<<<<<< HEAD
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHandler : MonoBehaviour {
    //Ability Handler Variables
    public Camera MainCamera;
    private Plane hitPlane = new Plane(Vector3.up, Vector3.up);
    private Vector3 playerCurrentPosition;
    private Vector3 hitPoint;

    //PLayer Associated Variables
    private PlayerController playerControllerScript;
    private Rigidbody playerRb;
    private Collider playerCollider;

    //Abilities Associated Variables
    public GameObject[] abilities = new GameObject[8];
    public AbilitySO[] abilityData = new AbilitySO[8];
    private bool[] abilityRefreshedFlag = new bool[8];
    private float[] cooldownTimer = { 0, 0, 0, 0, 0, 0, 0, 0 };

    //Dash Associated Variables
    [SerializeField] private DashSO dashData;
    private bool isDashing = false;

    //Input Actions
    private InputSystemActions inputActions;
    private InputAction[] abilityUse = new InputAction[8];

    private void Awake() {
        inputActions = new InputSystemActions();

        playerControllerScript = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        foreach (GameObject ability in abilities) {
            ability.GetComponent<AbilityBehaviour>().userStats = GetComponentInParent<PlayerStats>();
        }
    }

    private void OnEnable() {
        abilityUse[0] = inputActions.Player.Ability1;
        abilityUse[1] = inputActions.Player.Ability2;
        abilityUse[2] = inputActions.Player.Ability3;
        abilityUse[3] = inputActions.Player.Ability4;
        abilityUse[4] = inputActions.Player.Ability5;
        abilityUse[5] = inputActions.Player.Dash;
        abilityUse[6] = inputActions.Player.Item1;
        abilityUse[7] = inputActions.Player.Item2;

        foreach (InputAction ability in abilityUse) {
            ability.Enable();
        }

    }

    private void OnDisable() {
        foreach (InputAction ability in abilityUse) {
            ability.Disable();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        MainCamera = Camera.main;
        for (int i = 0; i < abilities.Length; i++) {
            abilityData[i] = abilities[i].GetComponent<AbilityBehaviour>().GetAbilitySO();
            abilityRefreshedFlag[i] = true;
        }
    }

    // Update is called once per frame
    void Update() {
        //Calculate Targeted Position and Rotation for Projectile
        playerCurrentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        hitPoint = GetHitPoint(playerCurrentPosition);


        //Use Ability
        for (int i = 0; i < 5; i++) {
            if (abilityUse[i].ReadValue<float>() != 0f) {
                UseAbility(i);
            }

            //Refresh cooldown timer
            AbilityRefreshTimer(i);
        }
        if (abilityUse[5].WasPressedThisFrame() && abilityRefreshedFlag[5]) {
            StartCoroutine(DashRoutine(dashData.cooldown));
        }
    }

    Vector3 GetHitPoint(Vector3 playerCurrentPosition) {
        float enter = 0.0f;
        Vector3 hitPoint = new Vector3(0, 0, 0);
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        if (hitPlane.Raycast(ray, out enter)) {
            //Get targeted Position
            hitPoint = ray.GetPoint(enter);
            playerCurrentPosition = new Vector3(transform.position.x, 1, transform.position.z);

            //DEBUG LINE HIT POSITION
            //Debug.Log(hitPoint);
            Debug.DrawLine(playerCurrentPosition, hitPoint, Color.red);
        }
        return hitPoint;
    }

    private void UseAbility(int index) {

        //Cooldown Ability
        if (abilityRefreshedFlag[index] && abilityData[index].cooldownFlag) {
            abilityRefreshedFlag[index] = false;
            cooldownTimer[index] = abilityData[index].cooldown;
            StartCoroutine(abilities[index].GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint));
        }

        //Fire Rate Ability
        if (abilityRefreshedFlag[index] && !(abilityData[index].cooldownFlag)) {
            abilityRefreshedFlag[index] = false;
            cooldownTimer[index] = 1 / abilityData[index].fireRate;
            StartCoroutine(abilities[index].GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint));
        }
    }

    private void AbilityRefreshTimer(int index) {
        if (abilityRefreshedFlag[index] == false) {
            cooldownTimer[index] -= Time.deltaTime;

            if (cooldownTimer[index] < 0) {
                cooldownTimer[index] = 0;
            }

            if (cooldownTimer[index] <= 0) {
                abilityRefreshedFlag[index] = true;
            }
        }
    }

    private void UseDash(float dashForce, Vector3 direction) {
        if (abilityRefreshedFlag[5]) {
            playerRb.AddForce(direction * dashForce, ForceMode.Impulse);
        }
    }

    public bool GetDashingState() {
        return isDashing;
    }

    private IEnumerator DashRoutine(float cooldown) {
    isDashing = true;
    AudioManager.Instance.PlaySfx(SfxType.Dash);
        playerRb.isKinematic = false;
        playerCollider.isTrigger = true;

        UseDash(dashData.dashForce, playerControllerScript.rotatedDirection);
        abilityRefreshedFlag[5] = false;

        yield return new WaitForSeconds(0.25f);
        playerRb.linearVelocity = Vector3.zero; // Prevents physics interactions during dash

        isDashing = false;
        playerRb.isKinematic = true;
        playerCollider.isTrigger = false;

        yield return new WaitForSeconds(cooldown);
        abilityRefreshedFlag[5] = true;
    }
}
||||||| 49382df
=======
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHandler : MonoBehaviour {
    //Ability Handler Variables
    public Camera MainCamera;
    private Plane hitPlane = new Plane(Vector3.up, Vector3.up);
    private Vector3 playerCurrentPosition;
    private Vector3 hitPoint;

    //PLayer Associated Variables
    private PlayerController playerControllerScript;
    private Rigidbody playerRb;
    private Collider playerCollider;

    //Abilities Associated Variables
    public GameObject[] abilities = new GameObject[8];
    public AbilitySO[] abilityData = new AbilitySO[8];
    private bool[] abilityRefreshedFlag = new bool[8];
    private float[] cooldownTimer = { 0, 0, 0, 0, 0, 0, 0, 0 };

    //Dash Associated Variables
    [SerializeField] private DashSO dashData;
    private bool isDashing = false;

    //Input Actions
    private InputSystemActions inputActions;
    private InputAction[] abilityUse = new InputAction[8];

    private void Awake() {
        inputActions = new InputSystemActions();

        playerControllerScript = GetComponent<PlayerController>();
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        foreach (GameObject ability in abilities) {
            ability.GetComponent<AbilityBehaviour>().userStats = GetComponentInParent<PlayerStats>();
        }
    }

    private void OnEnable() {
        abilityUse[0] = inputActions.Player.Ability1;
        abilityUse[1] = inputActions.Player.Ability2;
        abilityUse[2] = inputActions.Player.Ability3;
        abilityUse[3] = inputActions.Player.Ability4;
        abilityUse[4] = inputActions.Player.Ability5;
        abilityUse[5] = inputActions.Player.Dash;
        abilityUse[6] = inputActions.Player.Item1;
        abilityUse[7] = inputActions.Player.Item2;

        foreach (InputAction ability in abilityUse) {
            ability.Enable();
        }

    }

    private void OnDisable() {
        foreach (InputAction ability in abilityUse) {
            ability.Disable();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        MainCamera = Camera.main;
        for (int i = 0; i < abilities.Length; i++) {
            abilityData[i] = abilities[i].GetComponent<AbilityBehaviour>().GetAbilitySO();
            abilityRefreshedFlag[i] = true;
        }
    }

    // Update is called once per frame
    void Update() {
        //Calculate Targeted Position and Rotation for Projectile
        playerCurrentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        hitPoint = GetHitPoint(playerCurrentPosition);


        //Use Ability
        for (int i = 0; i < 5; i++) {
            if (abilityUse[i].ReadValue<float>() != 0f) {
                UseAbility(i);
            }

            //Refresh cooldown timer
            AbilityRefreshTimer(i);
        }
        if (abilityUse[5].WasPressedThisFrame() && abilityRefreshedFlag[5]) {
            StartCoroutine(DashRoutine(dashData.cooldown));
        }
    }

    Vector3 GetHitPoint(Vector3 playerCurrentPosition) {
        float enter = 0.0f;
        Vector3 hitPoint = new Vector3(0, 0, 0);
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        if (hitPlane.Raycast(ray, out enter)) {
            //Get targeted Position
            hitPoint = ray.GetPoint(enter);
            playerCurrentPosition = new Vector3(transform.position.x, 1, transform.position.z);

            //DEBUG LINE HIT POSITION
            //Debug.Log(hitPoint);
            Debug.DrawLine(playerCurrentPosition, hitPoint, Color.red);
        }
        return hitPoint;
    }

    private void UseAbility(int index) {

        //Cooldown Ability
        if (abilityRefreshedFlag[index] && abilityData[index].cooldownFlag) {
            abilityRefreshedFlag[index] = false;
            cooldownTimer[index] = abilityData[index].cooldown;
            StartCoroutine(abilities[index].GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint));
        }

        //Fire Rate Ability
        if (abilityRefreshedFlag[index] && !(abilityData[index].cooldownFlag)) {
            abilityRefreshedFlag[index] = false;
            cooldownTimer[index] = 1 / abilityData[index].fireRate;
            StartCoroutine(abilities[index].GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint));
        }
    }

    private void AbilityRefreshTimer(int index) {
        if (abilityRefreshedFlag[index] == false) {
            cooldownTimer[index] -= Time.deltaTime;

            if (cooldownTimer[index] < 0) {
                cooldownTimer[index] = 0;
            }

            if (cooldownTimer[index] <= 0) {
                abilityRefreshedFlag[index] = true;
            }
        }
    }

    private void UseDash(float dashForce, Vector3 direction) {
        if (abilityRefreshedFlag[5]) {
            playerRb.AddForce(direction * dashForce, ForceMode.Impulse);
        }
    }

    public bool GetDashingState() {
        return isDashing;
    }

    private IEnumerator DashRoutine(float cooldown) {
        isDashing = true;
        playerRb.isKinematic = false;
        playerCollider.isTrigger = true;

        UseDash(dashData.dashForce, playerControllerScript.rotatedDirection);
        abilityRefreshedFlag[5] = false;

        yield return new WaitForSeconds(0.25f);
        playerRb.linearVelocity = Vector3.zero; // Prevents physics interactions during dash

        isDashing = false;
        playerRb.isKinematic = true;
        playerCollider.isTrigger = false;

        yield return new WaitForSeconds(cooldown);
        abilityRefreshedFlag[5] = true;
    }
}
>>>>>>> origin/tech-demo
