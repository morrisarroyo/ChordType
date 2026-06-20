using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "EncounterList", menuName = "Data/EncounterList")]
    public class EncounterList : ScriptableObject
    {
        // TODO: Remove Test Encounter
        public EncounterDefinition testEncounterDefinition;   
        public List<EncounterDefinition> encounterDefinitions;
    }
}
