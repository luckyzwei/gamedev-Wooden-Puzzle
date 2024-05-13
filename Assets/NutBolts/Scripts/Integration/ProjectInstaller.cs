using Game.Scripts.Shop;
using UnityEngine;
using Zenject;

namespace Integration
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ItemsSelectedData _itemsSelectedData;
        [SerializeField] 
        private AdMobController _adMobController;
        [SerializeField] 
        private IAPService _iapService;

        public override void InstallBindings()
        {
            Container.Bind<Bank>().AsSingle().NonLazy();
            Container.Bind<ItemsSelectedData>().FromInstance(_itemsSelectedData).AsSingle().NonLazy();
            Container.Bind<IAPService>().FromInstance(_iapService).AsSingle().NonLazy();
            Container.Bind<AdMobController>().FromInstance(_adMobController).AsSingle().NonLazy();
        
            Container.Bind<BannerViewController>().AsSingle().NonLazy();
            Container.Bind<InterstitialAdController>().AsSingle().NonLazy();
            Container.Bind<RewardedAdController>().AsSingle().NonLazy();
        }
    }
}
