using Ship;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.TIEAggressor
    {
        public class DoubleEdge : TIEAggressor
        {
            public DoubleEdge() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Double Edge",
                    4,
                    19,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.DoubleEdgeAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.FirstEdition
{
    public class DoubleEdgeAbility : GenericAbility
    {

        private string LastWeaponUsed;

        public override void ActivateAbility()
        {
            HostShip.OnAttackMissedAsAttacker += CheckSecondAttack;
            Phases.Events.OnRoundEnd += ClearIsAbilityUsedFlag;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnAttackMissedAsAttacker -= CheckSecondAttack;
            Phases.Events.OnRoundEnd -= ClearIsAbilityUsedFlag;
        }

        private void CheckSecondAttack()
        {
            if (!IsAbilityUsed && !HostShip.IsCannotAttackSecondTime && Combat.ChosenWeapon.WeaponType != Ship.WeaponTypes.PrimaryWeapon)
            {
                IsAbilityUsed = true;
                LastWeaponUsed = Combat.ChosenWeapon.Name;
                // Trigger must be registered just before it's resolution
                HostShip.OnCombatCheckExtraAttack += RegisterSecondAttackTrigger;
            }
        }
        
        private void RegisterSecondAttackTrigger(GenericShip ship)
        {
            HostShip.OnCombatCheckExtraAttack -= RegisterSecondAttackTrigger;

            RegisterAbilityTrigger(TriggerTypes.OnCombatCheckExtraAttack, UseDoubleEdgeAbility);
        }

        private void UseDoubleEdgeAbility(object sender, System.EventArgs e)
        {
            if (!HostShip.IsCannotAttackSecondTime)
            {
                HostShip.IsCannotAttackSecondTime = true;

                Combat.StartSelectAttackTarget(
                    HostShip,
                    FinishAdditionalAttack,
                    IsDifferentWeaponShot,
                    HostShip.PilotName,
                    "You may perform a different weapon attack",
                    HostShip
                );
            }
            else
            {
                Messages.ShowErrorToHuman(string.Format("{0} cannot attack an additional time", HostShip.PilotInfo.PilotName));
                Triggers.FinishTrigger();
            }
        }

        private void FinishAdditionalAttack()
        {
            // If attack is skipped, set this flag, otherwise regular attack can be performed second time
            HostShip.IsAttackPerformed = true;

            //if bonus attack was skipped, allow bonus attacks again
            if (HostShip.IsAttackSkipped) HostShip.IsCannotAttackSecondTime = false;

            Triggers.FinishTrigger();
        }

        private bool IsDifferentWeaponShot(GenericShip defender, IShipWeapon weapon, bool isSilent)
        {
            bool result = false;

            if (Combat.ChosenWeapon.Name != LastWeaponUsed)
            {
                result = true;
            }
            else
            {
                if (!isSilent) Messages.ShowError("This attack must be performed using a different weapon");
            }

            return result;
        }
    }
}
