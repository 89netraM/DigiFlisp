using Model;
using Model.Components;
using System;
using System.Collections.Generic;

namespace GUI.File
{
	struct ComponentJSON
	{
		public static ComponentJSON ToJSON(Component component)
		{
			return new ComponentJSON
			{
				TypeId = component.TypeId,
				Position = new PositionJSON
				{
					X = component.Position.X,
					Y = component.Position.Y
				}
			};
		}

		public static Component FromJSON(ComponentJSON componentJSON, string id, IDictionary<string, Blueprint> blueprints)
		{
			Component component;
			
			try
			{
				component = ComponentFactory.CreateComponent(componentJSON.TypeId, id);
			}
			catch
			{
				try
				{
					component = ComponentFactory.CreateCustomComponent(blueprints[componentJSON.TypeId], id);
				}
				catch
				{
					throw new Exception($"Could not find Blueprint {componentJSON.TypeId} to create CustomComponent.");
				}
			}

			component.Position.X = componentJSON.Position.X;
			component.Position.Y = componentJSON.Position.Y;
			return component;
		}

		public string TypeId { get; set; }
		public PositionJSON Position { get; set; }
	}

	struct PositionJSON
	{
		public int X { get; set; }
		public int Y { get; set; }
	}
}