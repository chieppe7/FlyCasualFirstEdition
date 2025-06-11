using System;
using Tokens;
using Upgrade;
using System.Collections.Generic;
using System.Linq;
using BoardTools;
using Movement;

namespace Ship
{
    namespace FirstEdition.TIEPhantom
    {
        public class Echo : TIEPhantom
        {
            public Echo() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "\"Echo\"",
                    6,
                    30,
                    isLimited: true,
                    abilityType: typeof(Abilities.FirstEdition.EchoAbility),
                    extraUpgradeIcon: UpgradeType.Talent
                );
            }
        }
    }
}

namespace Abilities.FirstEdition
{
    public class EchoAbility : GenericAbility
    {

        public override void ActivateAbility()
        {
            HostShip.OnGetAvailableDecloakTemplates += ChangeDecloakTemplates;
        }

        public override void DeactivateAbility()
        {
            HostShip.OnGetAvailableDecloakTemplates -= ChangeDecloakTemplates;
        }

        private void ChangeDecloakTemplates(List<ManeuverTemplate> availableTemplates)
        {
            if (availableTemplates.Any(n => n.Name == "Straight 2"))
            {
                availableTemplates.RemoveAll(n => n.Name == "Straight 2");
                availableTemplates.Add(new ManeuverTemplate(ManeuverBearing.Bank, ManeuverDirection.Left, ManeuverSpeed.Speed2));
                availableTemplates.Add(new ManeuverTemplate(ManeuverBearing.Bank, ManeuverDirection.Right, ManeuverSpeed.Speed2));
            }
        }
    }

}

