using Gespell.Gestures;
using UnityEngine;

namespace Gespell
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UnitManager unitManager;
        [SerializeField] private GestureRecognizer gestureRecognizer;
        [SerializeField] private SpellManager spellManager;

        private void Start()
        {
            unitManager.SpawnPlayer();
            unitManager.StartNextWave();
            gestureRecognizer.OnSpellDrawn += GestureRecognizerOnSpellDrawn;
        }

        private void OnDisable()
        {
            gestureRecognizer.OnSpellDrawn -= GestureRecognizerOnSpellDrawn;
        }

        private void GestureRecognizerOnSpellDrawn(SpellManager.SpellType obj)
        {
            spellManager.CastSpell(obj);
        }
    }
}
