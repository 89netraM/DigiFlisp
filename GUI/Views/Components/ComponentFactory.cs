using GUI.ViewModels;
using GUI.ViewModels.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Views.Components
{
	public static class ComponentFactory
	{
		public static Component Create(ComponentViewModel component)
		{
			return new Component
			{
				Name = component.Id,
				DataContext = component,
				Content = new IconComponent
				{
					DataContext = component
				}
			};
		}
	}
}