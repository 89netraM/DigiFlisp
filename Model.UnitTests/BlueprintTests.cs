﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.UnitTests
{
	[TestClass]
	public class BlueprintTests
	{
		private Blueprint blueprint;

		private InputComponent inComponent;

		private Component notA;
		private Component notB;
		private Component notC;

		[TestInitialize]
		public void Initialize()
		{
			blueprint = new Blueprint();

			inComponent = ComponentFactory.CreateComponent(InputComponent.InputTypeId) as InputComponent;

			notA = ComponentFactory.CreateComponent(NotGate.NotTypeId);
			notB = ComponentFactory.CreateComponent(NotGate.NotTypeId);
			notC = ComponentFactory.CreateComponent(NotGate.NotTypeId);
		}

		[TestMethod]
		public void AddingComponents()
		{
			blueprint.AddComponent(notA);
			blueprint.AddComponent(notB);
			blueprint.AddComponent(notC);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void AddingTwice()
		{
			blueprint.AddComponent(notA);

			// Should throw the exception
			blueprint.AddComponent(notA);
		}

		[TestMethod]
		public void RemovingComponents()
		{
			AddingComponents();

			blueprint.RemoveComponent(notA);
			blueprint.RemoveComponent(notB);
			blueprint.RemoveComponent(notC);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void RemovingTwice()
		{
			AddingComponents();
			blueprint.RemoveComponent(notA);

			// Should throw the exception
			blueprint.RemoveComponent(notA);
		}

		private void ConnectComponents()
		{
			notA.SetInput(0, inComponent.OutputSignals[0]);
			blueprint.Connect(notA, 0, notB, 0);
			blueprint.Connect(notB, 0, notC, 0);
		}

		[TestMethod]
		public void ConnectingComponents()
		{
			AddingComponents();
			// Should not throw any exceptions
			ConnectComponents();

			Assert.IsTrue(notC.OutputSignals[0].Value);

			inComponent.State = true;
			Assert.IsFalse(notC.OutputSignals[0].Value);
		}

		[TestMethod]
		public void DisconnectingComponents()
		{
			AddingComponents();
			ConnectComponents();

			blueprint.Disconnect(notA, 0, notB, 0);

			inComponent.State = true;
			Assert.IsTrue(notC.OutputSignals[0].Value);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void ConnectingToOutsideComponent()
		{
			blueprint.AddComponent(notA);

			// Should throw the exception
			blueprint.Connect(notA, 0, notB, 0);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void ConnectingFromOutsideComponent()
		{
			blueprint.AddComponent(notC);

			// Should throw the exception
			blueprint.Connect(notB, 0, notC, 0);
		}

		[TestMethod]
		public void ConnectingTwice()
		{
			AddingComponents();
			notA.SetInput(0, inComponent.OutputSignals[0]);
			blueprint.Connect(notA, 0, notC, 0);

			inComponent.State = true;

			Assert.IsTrue(notC.OutputSignals[0].Value);

			blueprint.Connect(notB, 0, notC, 0);

			Assert.IsFalse(notC.OutputSignals[0].Value);
		}

		[TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ConnectingToIndexOutOfRange()
		{
			AddingComponents();

			// Should throw the exception
			blueprint.Connect(notA, 0, notB, -1);
		}

		[TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ConnectingFromIndexOutOfRange()
		{
			AddingComponents();

			// Should throw the exception
			blueprint.Connect(notA, -1, notB, 0);
		}

		[TestMethod]
		public void RemovingComponentsShouldRemoveConnections()
		{
			AddingComponents();
			ConnectComponents();

			bool hasListenerBeenInvoked = false;

			notC.OutputSignals[0].AddListener(
				"testListenerId",
				updateId =>
				{
					hasListenerBeenInvoked = true;
				}
			);

			inComponent.State = !inComponent.State;
			Assert.IsTrue(hasListenerBeenInvoked, "The listener should have been invoked.");

			blueprint.RemoveComponent(notB);

			hasListenerBeenInvoked = false;
			inComponent.State = false;
			Assert.IsFalse(hasListenerBeenInvoked, "The listener should not have been invoked.");
		}

		[TestMethod]
		public void ReplacingComponents()
		{
			AddingComponents();
			ConnectComponents();

			bool hasListenerBeenInvoked = false;

			notC.OutputSignals[0].AddListener(
				"testListenerId",
				updateId =>
				{
					hasListenerBeenInvoked = true;
				}
			);

			inComponent.State = !inComponent.State;
			Assert.IsTrue(hasListenerBeenInvoked, "The listener should have been invoked.");

			Component notD = ComponentFactory.CreateComponent(NotGate.NotTypeId);
			blueprint.ReplaceComponent(notB, notD);

			hasListenerBeenInvoked = false;
			inComponent.State = false;
			Assert.IsTrue(hasListenerBeenInvoked, "The listener should have been invoked.");
		}
	}
}