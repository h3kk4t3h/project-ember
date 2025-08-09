using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHandler : MonoBehaviour
{
    //Ability Handler Variables
    public Camera MainCamera;
    private Plane hitPlane = new Plane(Vector3.up, Vector3.up);
    private Vector3 playerCurrentPosition;
    private Vector3 hitPoint;
    

    //Abilities Associated Variables
    public GameObject[] ability = new GameObject[8];
    public AbilitySO[] abilityData = new AbilitySO[8];
    private float[] timeLastSpawn = {0, 0, 0, 0, 0 ,0 ,0, 0};

    //Input Actions
    private InputSystemActions inputActions;
    private InputAction[] abilityUse = new InputAction[8];

    private void Awake()
    {
        inputActions = new InputSystemActions();
    }

    private void OnEnable()
    {
        abilityUse[0] = inputActions.Player.Ability1;
        abilityUse[1] = inputActions.Player.Ability2;
        abilityUse[2] = inputActions.Player.Ability3;
        abilityUse[3] = inputActions.Player.Ability4;
        abilityUse[4] = inputActions.Player.Ability5;
        abilityUse[5] = inputActions.Player.Dash;
        abilityUse[6] = inputActions.Player.Item1;
        abilityUse[7] = inputActions.Player.Item2;

        foreach(InputAction ability in abilityUse)
        {
            ability.Enable();
        }
        
    }

    private void OnDisable()
    {
        foreach (InputAction ability in abilityUse)
        {
            ability.Disable();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainCamera = Camera.main;
        for (int i = 0; i < ability.Length; i++)
        {
            abilityData[i] = ability[i].GetComponent<AbilityBehaviour>().GetAbilitySO();
            timeLastSpawn[i] = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate Targeted Position and Rotation for Projectile
        playerCurrentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        hitPoint = GetHitPoint(playerCurrentPosition);


        //Use Ability
        //abilityUse[0].ReadValue<float>() != 0f)
        for(int i = 0; i < 6; i++)
        {
            if(abilityUse[i].ReadValue<float>() != 0f)
            {
                UseAbility(i);
            }
        }
    }

    Vector3 GetHitPoint(Vector3 playerCurrentPosition)
    {
        float enter = 0.0f;
        Vector3 hitPoint = new Vector3(0,0,0);
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (hitPlane.Raycast(ray, out enter))
        {
            //Get targeted Position
            hitPoint = ray.GetPoint(enter);
            playerCurrentPosition = new Vector3(transform.position.x, 1, transform.position.z);

            //DEBUG LINE HIT POSITION
            //Debug.Log(hitPoint);
            Debug.DrawLine(playerCurrentPosition, hitPoint, Color.red);
        }
        return hitPoint;
    }

    private void UseAbility(int index)
    {

        //Cooldown Ability
        if (Time.time - timeLastSpawn[index] >= abilityData[index].cooldown && abilityData[index].cooldownFlag)
        {
            timeLastSpawn[index] = Time.time;
            StartCoroutine(ability[index].GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint));
        }

        //Fire Rate Ability
        if (Time.time - timeLastSpawn[index] >= 1 / abilityData[index].fireRate && !(abilityData[index].cooldownFlag))
        {
            timeLastSpawn[index] = Time.time;
            StartCoroutine(ability[index].GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint));
        }

    }
}
