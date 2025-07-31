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
    public GameObject ability;
    public AbilitySO abilityData;
    private float timeLastSpawn = 0;

    //Input Actions
    private InputSystemActions inputActions;
    private InputAction[] abilityUse = new InputAction[7];

    private void Awake()
    {
        inputActions = new InputSystemActions();
    }

    private void OnEnable()
    {
        abilityUse[0] = inputActions.Player.Attack;
        abilityUse[0].Enable();
        
    }

    private void OnDisable()
    {
        abilityUse[0].Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainCamera = Camera.main;
        abilityData = ability.GetComponent<AbilityBehaviour>().GetAbilitySO();
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate Targeted Position and Rotation for Projectile
        playerCurrentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        hitPoint = GetHitPoint(playerCurrentPosition);

        //Use Ability
        if (abilityUse[0].ReadValue<float>() != 0f)
        {
            UseAbility();
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

    private void UseAbility()
    {

        //Cooldown Ability
        //if (Time.time - timeLastSpawn >= 1 / abilityData.cooldown && abilityData.cooldownFlag)
        //{
        //    ability.GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint);
        //    timeLastSpawn = Time.time;
        //}

        //Fire Rate Ability
        if (Time.time - timeLastSpawn >= 1 / abilityData.fireRate)
        {
            ability.GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint);
            timeLastSpawn = Time.time;
        }

    }
}
