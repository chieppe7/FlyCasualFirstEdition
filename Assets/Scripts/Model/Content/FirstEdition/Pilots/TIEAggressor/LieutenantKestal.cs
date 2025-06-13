using Ship;
using Tokens;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.TIEAggressor
    {
        public class LieutenantKestal : TIEAggressor
        {
            public LieutenantKestal() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Lieutenant Kestal",
                    7,
                    22,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.LieutenantKestalAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.FirstEdition
{
    public class LieutenantKestalAbility : GenericAbility
    {

        private string LastWeaponUsed;

        public override void ActivateAbility()
        {
            HostShip.OnGenerateDiceModificationsOpposite += LieutenantKestalActionEffect;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGenerateDiceModificationsOpposite -= LieutenantKestalActionEffect;
        }

        private void LieutenantKestalActionEffect(GenericShip host)
        {
            ActionsList.GenericAction newAction = new ActionsList.LieutenantKestalActionEffect()
            {
                
            };
            host.AddAvailableDiceModificationOwn(newAction);
        }
    }
}

namespace ActionsList
{
    public class LieutenantKestalActionEffect : GenericAction
    {

        public LieutenantKestalActionEffect()
        {
            Name = DiceModificationName = "Lieutenant Kestal";
            DiceModificationTiming = DiceModificationTimingType.Opposite;
        }

        public override int GetDiceModificationPriority()
        {
            int result = 100;

            return result;
        }

        public override bool IsDiceModificationAvailable()
        {
            bool result = false;
            
            if (HostShip.Tokens.HasToken<FocusToken>() && Combat.AttackStep == CombatStep.Defence && (Combat.DiceRollDefence.Blanks > 0 || Combat.DiceRollDefence.Focuses > 0))
            {
                result = true;
            }

            return result;
        }

        public override void ActionEffect(System.Action callBack)
        {
            Combat.DiceRollDefence.RemoveAllFailures();
            HostShip.Tokens.RemoveToken(
                typeof(FocusToken),
                callBack
            );
            callBack();
        }

    }

}
