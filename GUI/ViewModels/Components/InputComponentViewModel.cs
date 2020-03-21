using Model;
using Model.Components;
using System;

namespace GUI.ViewModels.Components
{
	public class InputComponentViewModel : ComponentViewModel
	{
		private readonly InputComponent model;

		public bool State
		{
			get => model.State;
			set => model.State = value;
		}

		public InputComponentViewModel(Component model) : base(model)
		{
			this.model = (model as InputComponent) ?? throw new ArgumentException("The model of a InputComponentViewModel must be InputComponent", nameof(model));
		}
	}
}