using Core;
using Microsoft.Xna.Framework;

namespace UU_GameProject
{
    class CMopWeapon : Component
    {
        private bool iniated = false;
        private bool isMeleeing = false;
        private float duration = 1;
        private float totalTime = 0;
        private float wobbleVar1 = 0;
        private float wobbleVar2 = 8;
        private float wobbleSpeed = 8;
        private float wobbleLimit = 7;
        private float spinFrame = 0;
        private CPlayerMovement playermovement;
        private CAnimatedSprite animWeapon;
        private GameObject playerWeapon;

        private new void Init()
        {
            iniated = true;
            Create();
            playermovement = GO.GetComponent<CPlayerMovement>();
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!iniated) Init();
            if (!isMeleeing)
                WobbleWeapon(time);
            else
                MeleeSpin();
            totalTime += time;
        }

        //creates the mop
        private void Create()
        {
            playerWeapon = new GameObject("playerWeapon", GO.Context, 1);
            animWeapon = new CAnimatedSprite();
            animWeapon.AddAnimation("weaponNormal", "playerWeapon");
            animWeapon.AddAnimation("weaponFire", "playerWeaponLit");
            animWeapon.AddAnimation("weaponLightning", "playerWeaponLightning");
            animWeapon.PlayAnimation("weaponNormal", 2);
            playerWeapon.AddComponent(animWeapon);
            playerWeapon.Pos = new Vector2(1, 1);
            playerWeapon.Size = new Vector2(0.25f, 1);
        }

        //changes animation to default mop
        public void ChangeAnimationDefault()
        {
            animWeapon.PlayAnimationIfDifferent("weaponNormal", 2);
        }

        //changes animation to fire staff
        public void ChangeAnimationFire()
        {
            animWeapon.PlayAnimationIfDifferent("weaponFire", 2);
            Timers.Remove("animationDuration");
            Timers.Add("animationDuration", duration, () => ChangeAnimationDefault());
        }

        //changes animation to lightning staff
        public void ChangeAnimationLightning()
        {
            animWeapon.PlayAnimationIfDifferent("weaponLightning", 2);
            Timers.Remove("animationDuration");
            Timers.Add("animationDuration", duration, () => ChangeAnimationDefault());
        }

        //ensures the normal wobble doesn't override the melee 'animation'
        public void Melee()
        {
            isMeleeing = true;
        }

        //adds melee 'animation' to the mop
        public void MeleeSpin()
        {
            if (playermovement.intendedDir < 0)
            {
                playerWeapon.Pos = playermovement.playerPosition + new Vector2(GO.Size.X / 2 - playerWeapon.Size.X, 0);
                if (spinFrame > 30)
                {
                    spinFrame = 0;
                    isMeleeing = false;
                }
                else
                {
                    playerWeapon.Renderer.SetRotation(-5 * spinFrame);
                    spinFrame += 1;
                }
            }
            else
            {
                playerWeapon.Pos = playermovement.playerPosition + new Vector2(GO.Size.X / 2, 0);
                if (spinFrame > 30)
                {
                    spinFrame = 0;
                    isMeleeing = false;
                }
                else
                {
                    playerWeapon.Renderer.SetRotation(5 * spinFrame);
                    spinFrame += 1;
                }
            }
        }

        //wiggles the weapon when the player runs
        public void WobbleWeapon(float time)
        {
            if (playermovement.intendedDir < 0)
            {
                playerWeapon.Pos = playermovement.playerPosition + new Vector2(GO.Size.X / 2 - playerWeapon.Size.X, 0);
                if (totalTime > .1f && playermovement.velocity.X != 0)
                {
                    if (wobbleVar1 > wobbleLimit && wobbleVar2 > 0)
                        wobbleVar2 = -wobbleSpeed;
                    else if (wobbleVar1 < -wobbleLimit && wobbleVar2 < 0)
                        wobbleVar2 = wobbleSpeed;
                    wobbleVar1 += wobbleVar2;
                    playerWeapon.Renderer.SetRotation(-40 + wobbleVar1);
                    totalTime = 0;
                }
            }
            else
            {
                playerWeapon.Pos = playermovement.playerPosition + new Vector2(GO.Size.X / 2, 0);
                if (totalTime > .1f && playermovement.velocity.X != 0)
                {
                    if (wobbleVar1 > wobbleLimit && wobbleVar2 > 0)
                        wobbleVar2 = -wobbleSpeed;
                    else if (wobbleVar1 < -wobbleLimit && wobbleVar2 < 0)
                        wobbleVar2 = wobbleSpeed;
                    wobbleVar1 += wobbleVar2;
                    playerWeapon.Renderer.SetRotation(40 + wobbleVar1);
                    totalTime = 0;
                }
            }
        }
    }
}
