using Avalonia.Controls;
using GUI.ViewModels.Components;
using System;
using System.Collections.Generic;

namespace GUI.Views.Components
{
	public static class ComponentFactory
	{
		private static readonly Func<IControl> defaultCreator = () => new IconComponent();
		private static readonly IReadOnlyDictionary<string, Func<IControl>> creatorDictionary = new Dictionary<string, Func<IControl>>
		{
			[Model.Components.InputComponent.InputTypeId] = () => new InputComponent(),
			[Model.Components.OutputComponent.OutputTypeId] = () => new OutputComponent()
		};

		public static Component Create(ComponentViewModel component)
		{
			IControl view = creatorDictionary.GetValueOrDefault(component.TypeId, defaultCreator).Invoke();
			view.DataContext = component;

			return new Component
			{
				Name = component.Id,
				DataContext = component,
				Content = view
			};
		}
	}
}