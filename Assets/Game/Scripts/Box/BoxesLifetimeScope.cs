using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Box
{
    public class BoxesLifetimeScope : LifetimeScope
    {
        [SerializeField] private BoxesScrollerView _boxesScrollerView;
        [SerializeField] private Transform _boxesContainer;
        [SerializeField] private BoxView _boxViewPrefab;
        [SerializeField] private Transform _boxParentAfterDrag;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_boxParentAfterDrag);
            builder.RegisterComponent(_boxesScrollerView);
            
            builder.Register<BoxesSpawnerService>(Lifetime.Singleton);
            builder.Register<BoxesScrollerService>(Lifetime.Singleton);

       
            builder.RegisterEntryPoint<BoxesPresenter>().AsSelf();
            
            
            builder.Register<Func<Color, BoxView>>(
                container => color =>
                {
                    var boxView = container.Instantiate(_boxViewPrefab, _boxesContainer);
                    boxView.Color = color;
                    return boxView;
                }, Lifetime.Scoped);
            
        }
    }
}