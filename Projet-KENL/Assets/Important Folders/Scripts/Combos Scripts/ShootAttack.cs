﻿using UnityEngine;
using UnityEngine.Networking;

public class ShootAttack : AttackTemplate
{
    public GameObject bulletPrefab;
    public float offsetX = 1f, offsetY = 2f;

    private PlayerScript player;

    private void Start()
    {
        inputKey = "f";
        attackCooldown = 0.5f;

        player = GetComponent<PlayerScript>();
    }
    
    public override void Attack()
    {
        CmdSpawnBullet();
    }

    [Command] private void CmdSpawnBullet()
    {
        Vector3 pos = transform.position;
        pos += new Vector3(
            (player.LookToRight() ? 1f : -1f) * offsetX, offsetY);

        GameObject bullet = Instantiate(bulletPrefab, pos,
            bulletPrefab.transform.rotation);

        NetworkServer.Spawn(bullet);

        BulletScript script = bullet.GetComponent<BulletScript>();
        script.dirX = player.LookToRight() ? 1f : -1f;
        script.player = player;
    }
}
