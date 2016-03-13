﻿using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Unit
{
	[TestFixture]
	public class DataLinkerUnitTestFixture
	{
		/// <summary>
		/// Ensure that an exception is thrown when a linked entity hasn't been saved yet. This is necessary because the linker cannot synchronise
		/// links with an entity that isn't in the data store.
		/// // TODO: Add a way to disable this check
		/// </summary>
		[Test]
		public void Test_CommitLinks_NonSavedEntity()
		{
			var data = new DataManager();

			var left = new ExampleReferenceLeft ();
			var right = new ExampleReferenceRight ();

			right.Left = left;

			// Try to save the "right" object without first saving the "left" object. It should throw an exception because it can't sync with
			// a non-existent entity
			data.Linker.CommitLinks(right);
		}

	}
}
