using UnityEngine;
using System.Collections;

public class DashAbility : MonoBehaviour
{

    public DashState dashState;
    public float dashTimer;
    public float maxDash = 20f;
    public float dashCost = 25f;

    private Animator anim;					// Reference to the player's animator component.

    public Vector2 savedVelocity;
    private GearMeter gearMeter;

    private void Awake()
    {
        gearMeter = GetComponent<GearMeter>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        switch (dashState)
        {
            case DashState.Ready:
                anim.SetBool("isDash", true);
                var isDashKeyDown = Input.GetKeyDown(KeyCode.LeftAlt);
                if (isDashKeyDown && gearMeter.gear == 3)
                {
                    GetComponent<PlayerControl>().maxSpeed *= 2f;
                    savedVelocity = GetComponent<Rigidbody2D>().velocity;
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 3f, GetComponent<Rigidbody2D>().velocity.y);
                    gearMeter.LoseEnergy(dashCost);
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:

                anim.SetBool("isDash", true);
                dashTimer += Time.deltaTime * 3;
                if (dashTimer >= maxDash)
                {
                    dashTimer = maxDash;
                    GetComponent<Rigidbody2D>().velocity = savedVelocity;
                    dashState = DashState.Cooldown;
                }
                break;
            case DashState.Cooldown:
                anim.SetBool("isDash", false);
                GetComponent<PlayerControl>().maxSpeed = 5f;
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }
    }
}

public enum DashState
{
    Ready,
    Dashing,
    Cooldown
}