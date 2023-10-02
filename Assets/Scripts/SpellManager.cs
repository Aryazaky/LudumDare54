using System;
using Gespell.Units;
using UnityEngine;

namespace Gespell
{
    public class SpellManager : MonoBehaviour
    {
        [SerializeField] private UnitManager unitManager;
        [SerializeField] private Vector3 spellPosition;
        // In the future, change this to just 1 spell prefab and use Initialize
        [Header("Spells")]
        [SerializeField] private SpellBase vSpell;
        [SerializeField] private SpellBase iSpell;
        [SerializeField] private SpellBase horizontalISpell;
        [SerializeField] private SpellBase oSpell;

        // This will be moved to SpellBase in the future
        public void CastSpell(SpellType type)
        {
            switch (type)
            {
                case SpellType.V:
                    Instantiate(vSpell, spellPosition, Quaternion.identity, transform);
                    unitManager.DoStuffToXNearest<Enemy>(_ => true, 1, unit => unit.Damage(5));
                    break;
                case SpellType.I:
                    Instantiate(iSpell, spellPosition, Quaternion.identity, transform);
                    unitManager.DoStuffToXNearest<Enemy>(_ => true, 1, unit => unit.KnockBack());
                    break;
                case SpellType.HorizontalI:
                    Instantiate(horizontalISpell, spellPosition, Quaternion.identity, transform);
                    unitManager.DoStuffToAll<Enemy>(_ => true, unit => unit.Damage(1));
                    break;
                case SpellType.O:
                    Instantiate(oSpell, spellPosition, Quaternion.identity, transform);
                    unitManager.DoStuffToAll<Enemy>(_ => true, unit => unit.KnockBack());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public enum SpellType
        {
            V, I, HorizontalI, O
        }
    }
}