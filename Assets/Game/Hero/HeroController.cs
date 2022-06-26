using UnityEngine;

namespace Assets.Game.Hero
{
    public class HeroController : MonoBehaviour, IStackPointCube
    {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private GameObject _cubeHolder;

        private int _countOfCubes = 1;

        public void AddToStackCube(GameObject cube)
        {
            cube.transform.SetParent(_cubeHolder.transform);
            cube.transform.position = new Vector3(_cubeHolder.transform.position.x, _cubeHolder.transform.position.y + cube.transform.localScale.y * _countOfCubes, _cubeHolder.transform.position.z);
            _countOfCubes++;

            _player.transform.position = new Vector3(_player.transform.position.x, cube.transform.position.y, _player.transform.position.z);
        }
    }
}
