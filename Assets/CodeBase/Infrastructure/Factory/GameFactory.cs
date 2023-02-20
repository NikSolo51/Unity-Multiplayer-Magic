using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Network;
using CodeBase.Services.Audio;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.SoundManager;
using CodeBase.UI;
using CodeBase.Weapons;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory, IPunPrefabPool
    {
        private IAssets _asset;
        private IStaticDataService _staticData;
        private ISaveLoadService _saveLoadService;
        private DiContainer _container;

        public GameFactory(IAssets assets,
            IStaticDataService staticData,
            ISaveLoadService saveLoadService,
            DiContainer container)
        {
            _asset = assets;
            _staticData = staticData;
            _saveLoadService = saveLoadService;
            _container = container;
            PhotonNetwork.PrefabPool = this;
        }

        public async Task WarmUp()
        {
            await _asset.Load<GameObject>(AssetsAdress.Hero);
            await _asset.Load<GameObject>(AssetsAdress.UpdateManager);
            await _asset.Load<GameObject>(AssetsAdress.WeaponUI);
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetsAdress.Hud);
            return hud;
        }

        public async Task<GameObject> CreateWeaponUI(WeaponStaticData weaponStaticData, Transform parent)
        {
            GameObject weaponUIGO = await InstantiateRegisteredAsync(AssetsAdress.WeaponUI, parent);
            WeaponUI weaponUI = weaponUIGO.GetComponent<WeaponUI>();
            _container.Inject(weaponUI);
            weaponUI.Initialize(weaponStaticData);
            return weaponUIGO;
        }

        public GameObject CreateProjectile(ProjectileType projectileType, Vector3 at, Quaternion rotation)
        {
            return PhotonNetwork.Instantiate(projectileType.ToString(), at, rotation);
        }

        public async Task<GameObject> CreateWeapon(WeaponType weaponType, Transform parent)
        {
            WeaponStaticData weaponStaticData = _staticData.ForWeapon(weaponType);

            GameObject weaponGO =
                PhotonNetwork.Instantiate(weaponType.ToString(), parent.position, Quaternion.identity);

            PlayerWeapon playerWeapon = weaponGO.GetComponentInChildren<PlayerWeapon>();

            playerWeapon.MaxAmmo = weaponStaticData.MagazineCount;
            playerWeapon.WeaponType = weaponStaticData.WeaponType;
            playerWeapon.ShootDelay = weaponStaticData.ShootDelay;
            playerWeapon.ReloadDelay = weaponStaticData.ReloadDelay;
            playerWeapon.ProjectileSpeed = weaponStaticData.ProjectileSpeed;
            playerWeapon.ProjectileType = weaponStaticData.ProjectileType;

            return weaponGO;
        }

        public async void CreateRoomButton(RoomInfo roomInfo, NetworkLauncher networkLauncher, Transform parent)
        {
            GameObject roomButton = await InstantiateRegisteredAsync(AssetsAdress.RoomButtonUI, parent);
            roomButton.transform.SetParent(parent);
            RoomListItem roomListItem = roomButton.GetComponent<RoomListItem>();
            roomListItem.Constructor(roomInfo, networkLauncher);
        }

        public async void CreatePlayerRoomButton(Player playerInfo, NetworkLauncher networkLauncher, Transform parent)
        {
            GameObject roomButton = await InstantiateRegisteredAsync(AssetsAdress.PlayerButtonUI, parent);
            roomButton.transform.SetParent(parent);
            PlayerListItem playerListItem = roomButton.GetComponent<PlayerListItem>();
            playerListItem.Constructor(playerInfo);
        }

        public async Task<ISoundService> CreateSoundManager(SoundManagerData soundManagerData)
        {
            SoundManagerStaticData soundManagerManagerStaticData =
                _staticData.ForSoundManager(soundManagerData._soundManagerType);

            if (soundManagerData._soundManagerType == SoundManagerType.Nothing)
                Debug.Log("SoundManager Type is Nothing");

            GameObject soundManagerPrefab = await _asset.Load<GameObject>(soundManagerManagerStaticData.SoundManager);

            GameObject soundManagerObject = InstantiateRegistered(soundManagerPrefab);
            SoundManagerAbstract soundManagerAbstract = soundManagerObject.GetComponent<SoundManagerAbstract>();

            soundManagerAbstract.sounds = soundManagerData._sounds;
            soundManagerAbstract.clips = soundManagerData._clips;

            return soundManagerAbstract;
        }


        public GameObject CreateHero(Vector3 at)
        {
            //GameObject HeroGameObject = await InstantiateRegisteredAsync(AssetsAdress.Hero, at);
            GameObject HeroGameObject = PhotonNetwork.Instantiate(AssetsAdress.Hero, at, Quaternion.identity);
            return HeroGameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }


        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath, at: at);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Transform parent)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath, parent);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }


        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at, Quaternion rotation)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, at, rotation);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            ISavedProgressReader[] readers = gameObject.GetComponentsInChildren<ISavedProgressReader>();
            for (var index = 0; index < readers.Length; index++)
            {
                ISavedProgressReader progressReader = readers[index];
                _saveLoadService.Register(progressReader);
            }
        }

        public void CleanUp()
        {
            _saveLoadService.CleanUp();
            _asset.CleanUp();
        }

        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = _asset.LoadSynchronously<GameObject>(prefabId);

            if (rotation == Quaternion.identity) rotation = prefab.transform.rotation;
             
            GameObject gameObject = InstantiateRegistered(prefab, position, rotation);

            return gameObject;
        }

        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
    }
}