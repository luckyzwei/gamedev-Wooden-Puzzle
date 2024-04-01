using UnityEngine;
using VKSdk;
using VKSdk.Notify;
using VKSdk.UI;
using Zenject;

namespace NutBolts.Scripts.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private VKLayerController _vkLayerController;
        [SerializeField] private VKNotifyController _vkNotifyController;
        [SerializeField] private VKAudioController _vkAudioController;
        
        public override void InstallBindings()
        {
            Container.Bind<VKLayerController>().FromInstance(_vkLayerController).AsSingle();
            Container.Bind<VKNotifyController>().FromInstance(_vkNotifyController).AsSingle();
            Container.Bind<VKAudioController>().FromInstance(_vkAudioController).AsSingle();
        }
    }
}