using NutBolts.Scripts.Assistant;
using NutBolts.Scripts.Data;
using UnityEngine;
using Zenject;

namespace NutBolts.Scripts.Installers
{
    public class ProjectInstallers : MonoInstaller
    {
        [SerializeField] private ItemController _itemController;
        [SerializeField] private ItemsController _itemsController;
        [SerializeField] private DataMono _dataMono;
        [SerializeField] private GameManager _gameManager;
        public override void InstallBindings()
        {
            Container.Bind<ItemController>().FromInstance(_itemController).AsSingle();
            Container.Bind<ItemsController>().FromInstance(_itemsController).AsSingle();
            Container.Bind<DataMono>().FromInstance(_dataMono).AsSingle();
            Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle();
        }
    }
}