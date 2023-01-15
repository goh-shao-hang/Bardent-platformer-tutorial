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

        protected override void Awake()
        {
            base.Awake();

            baseSpriteRenderer = transform.Find("Base").GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = transform.Find("WeaponSprite").GetComponent<SpriteRenderer>();

            //TODO: fix this when we create weapon data
            //baseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            //weaponSpriteRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
        }
    }
}
