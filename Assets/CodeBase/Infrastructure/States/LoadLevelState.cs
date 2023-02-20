using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.Inidcators;
using CodeBase.Logic.Interactive;
using CodeBase.PlayerScripts;
using CodeBase.Services.Audio;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.UI;
using CodeBase.Weapons;
using CodeBase.WeaponsInventory;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        private LoadingCurtain _curtain;
        private IGameFactory _gameFactory;
        private IStaticDataService _staticData;
        private IPlayerWeaponInventory _playerWeaponInventory;
        private ISaveLoadService _saveLoadService;
        private DiContainer _container;


        public LoadLevelState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain curtain,
            IGameFactory gameFactory,
            IStaticDataService staticData,
            IPlayerWeaponInventory playerWeaponInventory,
            ISaveLoadService saveLoadService, DiContainer container)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _playerWeaponInventory = playerWeaponInventory;
            _saveLoadService = saveLoadService;
            _container = container;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _playerWeaponInventory.CleanUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private async void OnLoaded()
        {
            await InitGameWorld();
            _saveLoadService.InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            if (!levelData)
                return;
            if (!levelData.InitGameWorld)
                return;

            GameObject player = CreatePlayer(levelData);
            Camera camera = player.GetComponentInChildren<Camera>();

            GameObject hud = await CreateHud();

            ReloadUIIndicator reloadUiIndicator = hud.GetComponent<ReloadUIIndicator>();
            HealthUIIndicator healthUiIndicator = hud.GetComponent<HealthUIIndicator>();
            AmmoCountUI ammoCountUi = hud.GetComponent<AmmoCountUI>();
            ReloadUiIndicator(reloadUiIndicator);
            AmmoCountUi(ammoCountUi);
            PlayerGun(player);

            WeaponSpawner weaponSpawner = WeaponSpawner(player);
            _playerWeaponInventory.WeaponSpawner = weaponSpawner;

            PlayerWeaponScroller(player);
            PlayerWeapon playerWeapon = PlayerWeapon(reloadUiIndicator);
            FirstPersonCamera(camera);
            InteractiveSystem(player);
            AmmoCountUI(ammoCountUi, playerWeapon);
            HeroMove(player, camera);
            PlayerJump(player);
            Health(player, healthUiIndicator);
            EffectSystem(player);
            PickUpObjects(player);

            await CreateAudio(levelData);
        }

        private void AmmoCountUi(AmmoCountUI ammoCountUi)
        {
            _container.Inject(ammoCountUi);
        }

        private void ReloadUiIndicator(ReloadUIIndicator reloadUiIndicator)
        {
            _container.Inject(reloadUiIndicator);
        }

        private PlayerWeaponScroller PlayerWeaponScroller(GameObject player)
        {
            PlayerWeaponScroller playerWeaponScroller = player.GetComponent<PlayerWeaponScroller>();
            _container.Inject(playerWeaponScroller);
            return playerWeaponScroller;
        }

        private WeaponSpawner WeaponSpawner(GameObject player)
        {
            WeaponSpawner weaponSpawner = player.GetComponent<WeaponSpawner>();
            _container.Inject(weaponSpawner);
            return weaponSpawner;
        }

        private PlayerWeapon PlayerWeapon(ReloadUIIndicator reloadUiIndicator)
        {
            CreateWeapon();
            PlayerWeapon playerWeapon = PlayerWeapon(_playerWeaponInventory.GetCurrentWeapon(), reloadUiIndicator);
            return playerWeapon;
        }

        private void PickUpObjects(GameObject player)
        {
            PickUpObject pickUpObject = player.GetComponent<PickUpObject>();
            _container.Inject(pickUpObject);
        }

        private void InteractiveSystem(GameObject player)
        {
            InteractiveSystem interactiveSystem = player.GetComponent<InteractiveSystem>();
            _container.Inject(interactiveSystem);
        }

        private void EffectSystem(GameObject player)
        {
            EffectsSystem effectsSystem = player.GetComponent<EffectsSystem>();
            _container.Inject(effectsSystem);
        }

        private void PlayerJump(GameObject player)
        {
            PlayerJump playerJump = player.GetComponent<PlayerJump>();
            _container.Inject(playerJump);
        }

        private static void Health(GameObject player, HealthUIIndicator healthUIIndicator)
        {
            IHealth health = player.GetComponent<IHealth>();
            health.OnHpPercent += healthUIIndicator.AnimateIndicator;
        }

        private static void AmmoCountUI(AmmoCountUI ammoCountUi, PlayerWeapon playerWeapon)
        {
            ammoCountUi.InitializeMaxAmmo(playerWeapon.MaxAmmo);
            playerWeapon.OnAmmoCount += ammoCountUi.UpdateAmmoText;
        }

        private PlayerWeapon PlayerWeapon(GameObject weapon, ReloadUIIndicator reloadUiIndicator)
        {
            PlayerWeapon playerWeapon = weapon.GetComponent<PlayerWeapon>();
            _container.Inject(playerWeapon);
            playerWeapon.OnReloadPercent += reloadUiIndicator.AnimateIndicator;
            _playerWeaponInventory.PlayerWeapon = playerWeapon;
            return playerWeapon;
        }

        private void CreateWeapon()
        {
            _playerWeaponInventory.AddWeapon(_playerWeaponInventory.WeaponType);
        }


        private void FirstPersonCamera(Camera camera)
        {
            FirstPersonView firstPersonView = camera.GetComponent<FirstPersonView>();
            _container.Inject(firstPersonView);
        }

        private PlayerGun PlayerGun(GameObject player)
        {
            PlayerGun playerGun = player.GetComponent<PlayerGun>();
            _container.Inject(playerGun);
            return playerGun;
        }

        private async Task<ISoundService> CreateAudio(LevelStaticData levelData)
        {
            ISoundService soundManager = await _gameFactory.CreateSoundManager(levelData.SoundManagerData);
            _container.Bind<ISoundService>().FromInstance(soundManager).AsSingle().NonLazy();
            return soundManager;
        }


        private GameObject CreatePlayer(LevelStaticData levelData)
        {
            Vector3 spawnPos = levelData.SpawnPoints[PhotonNetwork.CountOfPlayersInRooms];
            GameObject hero = _gameFactory.CreateHero(spawnPos);
            return hero;
        }

        private void HeroMove(GameObject hero, Camera camera)
        {
            HeroMove heroMove = hero.GetComponent<HeroMove>();
            _container.Inject(heroMove);
        }

        private async Task<GameObject> CreateHud()
        {
            GameObject hud = await _gameFactory.CreateHud();
            return hud;
        }

        private LevelStaticData LevelStaticData() => _staticData.ForLevel(SceneManager.GetActiveScene().name);
    }
}