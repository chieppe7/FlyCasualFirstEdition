using Arcs;
using BoardTools;
using Ship;
using SubPhases;
using System.Collections.Generic;
using System.Linq;
using Upgrade;

namespace Ship
{
    namespace FirstEdition.TIEAdvanced
    {
        public class CommanderAlozen : TIEAdvanced
        {
            public CommanderAlozen() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Commander Alozen",
                    5,
                    25,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.CommanderAlozenAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.FirstEdition
{
    public class CommanderAlozenAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers += RegisterAskTargetLock;
        }

        public override void DeactivateAbility()
        {
            Phases.Events.OnCombatPhaseStart_Triggers -= RegisterAskTargetLock;
        }

        private void RegisterAskTargetLock()
        {
            RegisterAbilityTrigger(TriggerTypes.OnCombatPhaseStart, SelectShipForAbility);
        }
        private void SelectShipForAbility(object sender, System.EventArgs e)
        {
            SelectTargetForAbility(
                AcquireTargetLock,
                FilterTargetInRange,
                GetAiAbilityPriority,
                HostShip.Owner.PlayerNo,
                HostShip.PilotInfo.PilotName,
                "Acqure a Target Lock on an enemy ship at Range 1",
                HostShip
            );
        }

        private bool FilterTargetInRange(GenericShip ship)
        {
            return FilterByTargetType(ship, new List<TargetTypes>() { TargetTypes.Enemy }) && FilterTargetsByRange(ship, 1, 1);
        }

        private int GetAiAbilityPriority(GenericShip ship)
        {
            int result = 0;

            ShotInfo shotInfo = new ShotInfo(HostShip, ship, HostShip.PrimaryWeapons);
            result += (3 - shotInfo.Range) * 100;

            result += ship.PilotInfo.Cost + ship.UpgradeBar.GetUpgradesOnlyFaceup().Sum(n => n.UpgradeInfo.Cost);

            return result;
        }

        private void AcquireTargetLock()
        {
            ActionsHolder.AcquireTargetLock(HostShip, TargetShip, SuccessfullSelection, UnSuccessfullSelection);
        }

        private void SuccessfullSelection()
        {
            SelectShipSubPhase.FinishSelection();
        }

        private void UnSuccessfullSelection()
        {
            Messages.ShowErrorToHuman("Unable to acquire Target Lock");
        }
    }
}