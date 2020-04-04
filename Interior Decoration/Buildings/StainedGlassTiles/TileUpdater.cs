using System.Collections;
using UnityEngine;

namespace InteriorDecoration.Buildings.StainedGlassTiles
{
    class TileUpdater : KMonoBehaviour
    {
        public int cell;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if(Game.Instance.GameStarted())
            {
                cell = Grid.PosToCell(gameObject.transform.position);
                StartCoroutine(UpdateTiles());
            }
        }

        IEnumerator UpdateTiles()
        {
            while (true)
            {
                World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);
                yield return new WaitForSeconds(.2f);
            }
        }
    }
}
