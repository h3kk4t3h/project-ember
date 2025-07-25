using UnityEngine;

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
    private float timeOfLastShot = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityData = ability.GetComponent<AbilityBehaviour>().GetAbilitySO();
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate Targeted Position and Rotation for Projectile
        playerCurrentPosition = new Vector3(transform.position.x, 1, transform.position.z);
        hitPoint = GetHitPoint(playerCurrentPosition);


        //Shooting Projectile & Timers ---------------------------------------------------------------------------------------------------
        if (Time.time - timeOfLastShot >= 1/ abilityData.fireRate)
        {
            ability.GetComponent<AbilityBehaviour>().SpawnAbility(playerCurrentPosition, hitPoint);
            timeOfLastShot = Time.time;
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

   
}
