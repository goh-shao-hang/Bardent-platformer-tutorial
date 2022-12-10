using UnityEngine;

namespace Gamecells.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private Animator anim;
        private GameObject baseGO;

        private static readonly int activeHash = Animator.StringToHash("active");

        private void Awake()
        {
            baseGO = transform.GetChild(0).gameObject;
            anim = baseGO.GetComponent<Animator>();
        }

        public void Enter()
        {
            print($"{transform.name} entered.");

            anim.SetBool(activeHash, true);
        }
    } 
}

