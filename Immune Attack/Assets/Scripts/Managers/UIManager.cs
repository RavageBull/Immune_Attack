using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameObject player;

    public Text health;
    public Image gunImage;

    public Transform pivot;
    public Image fill;

    public Image ammo;
    public Transform ammoLoc;
    public Image ammoIcon;
    public List<Image> ammoList = new List<Image>();

    Animator animator;
    public Image fade;

    private void OnEnable()
    {
        Player.Healed += UpdateHealth;
        Player.Damaged += UpdateHealth;
        Player.Damaged += FlashRed;
        PlayerShoot.AmmoUpdate += UpdateAmmo;
        GameManager.FinishLoading += Load;
        PlayerShoot.ShootAni += ShootAnimation;
        Powerups.HealthUpdate += UpdateHealth;
        Powerups.AmmoUpdate += UpdateAmmo;
    }

    private void OnDisable()
    {
        Player.Healed -= UpdateHealth;
        Player.Damaged -= UpdateHealth;
        Player.Damaged -= FlashRed;
        PlayerShoot.AmmoUpdate -= UpdateAmmo;
        GameManager.FinishLoading -= Load;
        PlayerShoot.ShootAni -= ShootAnimation;
        Powerups.HealthUpdate -= UpdateHealth;
        Powerups.AmmoUpdate -= UpdateAmmo;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("ToBlack");
    }

    void Load()
    {
        if (GameManager.manager.player != null)
        {
            player = GameManager.manager.player;

            for (int i = 0; i < player.GetComponent<PlayerShoot>().bulletsMag; i++)
            {
                ammoList.Add(Instantiate(ammoIcon));

                ammoList[i].transform.SetParent(ammoLoc.transform);

                ammoList[i].transform.position = new Vector3(ammoLoc.transform.position.x + 30 * i, ammoLoc.transform.position.y, ammoLoc.transform.position.z);
            }

            UpdateHealth();
            UpdateAmmo();

            animator.SetTrigger("Normal");
        }
    }

    //ideally whenever the player's health is changed in any way, an event gets triggered which this script will listen to and
    //in turn trigger this function
    void UpdateHealth()
    {
        if (player!= null)
        {
            pivot.localScale = new Vector3(player.GetComponent<Stats>().health / 100, 1, 1);
        }
    }

    void FlashRed()
    {
        animator.SetTrigger("Damaged");
    }

    void ShootAnimation()
    {
        gunImage.GetComponent<Animator>().SetTrigger("Shoot");
    }

    void UpdateAmmo()
    {
        if (player != null)
        {
            for(int i = 0; i < player.GetComponent<PlayerShoot>().bulletsMag; i++)
            {
                if (i < player.GetComponent<PlayerShoot>().currentBullets)
                {
                    ammoList[i].enabled = true;
                }
                else
                {
                    ammoList[i].enabled = false;
                }
            }
        }
    }
}