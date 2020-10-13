using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerAnims : MonoBehaviour
{
    Animator anim;
    public AudioSource swing1Sound;
    public AudioSource dashSound;
    public AudioSource guardSound;
    public float hitsounddelay;
    public float guardsounddelay;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void PlayerDash()
    {
        //Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetBool("idling", false);
            anim.SetBool("attacking", false);
            anim.SetBool("dashing", true);
            anim.Play("Dash");
            dashSound.Play();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("idling", true);
            anim.SetBool("dashing", false);
        }
    }

    public void PlayerGuardAnims()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            anim.SetBool("idling", false);
            anim.SetBool("attacking", false);
            anim.SetBool("guarding", true);
            anim.Play("ShieldUP");
            guardSound.PlayDelayed(guardsounddelay);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            anim.SetBool("guarding", false);
            anim.SetBool("idling", true);
        }
    }

    public void PlayerAttackAnims()
    {

        //Attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("idling", false);
            anim.SetBool("attacking", true);
            anim.Play("Swing1");
            swing1Sound.PlayDelayed(hitsounddelay);
        }
        //if not attacking or dashing go idle
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.SetBool("attacking", false);
            anim.SetBool("idling", true);
        }


    }

    // Update is called once per frame
    void Update()
    {
        PlayerAttackAnims();
        PlayerDash();
        PlayerGuardAnims();
    }
}
