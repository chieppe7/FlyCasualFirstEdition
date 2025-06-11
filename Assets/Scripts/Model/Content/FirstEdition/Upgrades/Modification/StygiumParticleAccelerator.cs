using ActionsList;
using Ship;
using System.Collections.Generic;
using Upgrade;

namespace UpgradesList.FirstEdition
{
    public class StygiumParticleAccelerator : GenericUpgrade
    {
        public StygiumParticleAccelerator() : base()
        {
            UpgradeInfo = new UpgradeCardInfo(
                "Stygium Particle Accelerator",
                UpgradeType.Modification,
                cost: 2,
                abilityType: typeof(Abilities.FirstEdition.StygiumParticleAcceleratorAbility),
                restriction: new ShipRestriction(typeof(Ship.FirstEdition.TIEPhantom.TIEPhantom))
            );
        }
    }
}

namespace Abilities.FirstEdition
{
    public class StygiumParticleAcceleratorAbility : GenericAbility
    {
        public override void ActivateAbility()
        {
            HostShip.OnDecloak += RegisterPerformFreeEvadeAction;
            HostShip.OnActionIsPerformed += RegisterPerformFreeEvadeAction;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnDecloak -= RegisterPerformFreeEvadeAction;
            HostShip.OnActionIsPerformed -= RegisterPerformFreeEvadeAction;
        }

        private void RegisterPerformFreeEvadeAction()
        {
            Triggers.RegisterTrigger(new Trigger()
            {
                Name = "Stygium Particle Accelerator",
                TriggerType = TriggerTypes.OnDecloak,
                TriggerOwner = HostShip.Owner.PlayerNo,
                EventHandler = PerformFreeEvadeAction
            });
        }

        private void RegisterPerformFreeEvadeAction(GenericAction action)
        {
            if(action is CloakAction)
                Triggers.RegisterTrigger(new Trigger()
                {
                    Name = "Stygium Particle Accelerator",
                    TriggerType = TriggerTypes.OnActionIsPerformed,
                    TriggerOwner = HostShip.Owner.PlayerNo,
                    EventHandler = PerformFreeEvadeAction
                });
        }

        private void PerformFreeEvadeAction(object sender, System.EventArgs e)
        {
            HostShip.AskPerformFreeAction(
                new EvadeAction(),
                Triggers.FinishTrigger,
                HostUpgrade.UpgradeInfo.Name,
                "After you either Decloak or perform a Cloak action, you may perform a free Evade action",
                HostUpgrade
            );
        }
    }
}