using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    namespace FirstEdition.FWing
    {
        public class PhoenixSquadronPilot : FWing
        {
            public PhoenixSquadronPilot() : base()
            {
                PilotInfo = new PilotCardInfo(
                    "Phoenix Squadron Pilot",
                    10,
                    10
                );
            }
        }
    }
}
