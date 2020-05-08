using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    /// <summary>
    /// Total hitpoints
    /// </summary>
    public int hp = 1;

    /// <summary>
    /// Enemy or player?
    /// </summary>
    public bool isEnemy = true;

    /// <summary>
    /// Object used to make death explosion
    /// </summary>
    public Transform deathExplosion;

    private Material matWhite;
    private Material matDefault;
    private SpriteRenderer sr;
    private float flashInterval = 0.1f;
    private AsteroidScript astScr = null;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("flashWhite", typeof(Material)) as Material;
        matDefault = sr.material;
    }

    /// <summary>
    /// Inflicts damage, flash sprite, and check if the object should be destroyed
    /// </summary>
    /// <param name="damageCount"></param>
    /// 
    public void Damage(int damageCount)
    {
        hp -= damageCount;
        sr.material = matWhite; //Flash white

        if (hp <= 0)
        {
            // Dead!
            if (deathExplosion != null)
            {
                var deathExplosionTransform = Instantiate(deathExplosion) as Transform;
                deathExplosionTransform.position = transform.position;
            }

            //Asteroids may break apart
            astScr = GetComponentInChildren<AsteroidScript>();
            if (astScr != null)
            {
                astScr.BreakApart();
            }
            Destroy(gameObject);
        } else
        { //If not dead, return from white flash after interval
            Invoke("ResetMaterial", flashInterval);

        }
    }

    void ResetMaterial()
    {
        sr.material = matDefault;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a shot?
        ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            // Avoid friendly fire
            if (shot.isEnemyShot != isEnemy)
            {
                Damage(shot.damage);
                shot.onImpact(gameObject.transform);
            }
        }
    }
}
