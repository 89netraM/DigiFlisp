using Model;
using System;
using System.Collections.Generic;

namespace GUI.ViewModels.Components
{
	public static class ComponentFactory
	{
		private static readonly Func<Component, ComponentViewModel> defaultCreator = m => new ComponentViewModel(m);
		private static readonly IReadOnlyDictionary<string, Func<Component, ComponentViewModel>> creatorDictionary = new Dictionary<string, Func<Component, ComponentViewModel>>
		{
			[Model.Components.InputComponent.InputTypeId] = m => new InputComponentViewModel(m)
		};

		public static ComponentViewModel Create(Component component)
		{
			return creatorDictionary.GetValueOrDefault(component.TypeId, defaultCreator).Invoke(component);
		}
	}
}