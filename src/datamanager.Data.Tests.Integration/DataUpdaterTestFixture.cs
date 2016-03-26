﻿using System;
using NUnit.Framework;
using datamanager.Entities;

namespace datamanager.Data.Tests.Integration
{
	[TestFixture]
	public class DataUpdaterTestFixture : BaseDataIntegrationTestFixture
	{
		[TestFixture(Category="Integration")]
		[ExpectedException(typeof(EntityNotFoundException))]
		public void Test_Update_EntityNotFoundException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

			// Call the Update function which should throw an exception because it hasn't been saved yet
			Data.Updater.Update(entity);
		}
	}
}
