using Gamecells.Weapons.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamecells.Weapons.Components
{
    public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
    {
        private SpriteRenderer baseSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;

        private int currentWeaponSpriteIndex = 0;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentWeaponSpriteIndex = 0;
        }

        private void HandleBaseSpriteChange(SpriteRenderer sr)
        {
            if (!isAttackActive)
            {
                weaponSpriteRenderer.sprite = null;
                return;
            }

            Sprite[] currentAttackSprites = currentAttackData.Sprites;

            if (currentWeaponSpriteIndex >= currentAttackSprites.Length)
            {
                Debug.LogWarning($"{weapon.name}: Weapon sprites length mismatch.");
                return;
            }

            weaponSpriteRenderer.sprite = currentAttackSprites[currentWeaponSpriteIndex];
            currentWeaponSpriteIndex++;
        }

        protected override void Start()
        {
            base.Start();

            baseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();

            baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
        }
    }
}
