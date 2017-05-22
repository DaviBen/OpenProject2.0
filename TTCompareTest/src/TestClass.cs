using NUnit.Framework;
using System;
using TTCompare;

namespace UnitTests
{
	[TestFixture]
	public class MyUnitTests
	{
		Compare CompareMenu;
		Manage ManageMenu;
		Timetable Timetable;

		[SetUp]
		public void Init()
		{
			CompareMenu = new Compare ();
			ManageMenu = new Manage ();
			Timetable = new Timetable ();
			Timetable.Create ();
		}

		[Test]
		public void TestCompareFileLoading()
		{
			CompareMenu.LoadFile ("Test1");
			Assert.IsTrue(CompareMenu.TTnames.Contains("Test1"));
			Assert.IsTrue(CompareMenu.ToCompare[0].GetString() 
				== "YNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNM");
		}

		[Test]
		public void TestCompare()
		{
			CompareMenu.LoadFile ("Test1");
			CompareMenu.LoadFile ("Test2");
			CompareMenu.ToPrint.Create ();
			CompareMenu.CompareTimetables ();
			Assert.IsTrue (CompareMenu.ToPrint.GetString ()
				== "MNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
					"NNNNNNNNNNNNNNNNNNNNNNNNNNM");
		}

		[Test]
		public void TestCompare2()
		{
			CompareMenu.LoadFile ("Test3");
			CompareMenu.LoadFile ("Test4");
			CompareMenu.ToPrint.Create ();
			CompareMenu.CompareTimetables ();
			Assert.IsTrue (CompareMenu.ToPrint.GetString ()
				== "NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
				"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
				"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
				"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
				"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
				"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
				"NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN" +
				"NNNNNNNNNNNNNNNNNNNNNNNNNNN");
		}

		[Test]
		public void TestNameValidation()
		{
			string testname = "Test*";
			ManageMenu.ValidateNameEntry (testname.ToCharArray());
			Assert.IsFalse (ManageMenu.CorrectName);
		}

		[Test]
		public void TestNameValidation2()
		{
			string testname = "Test";
			ManageMenu.ValidateNameEntry (testname.ToCharArray());
			Assert.IsTrue (ManageMenu.CorrectName);
		}
	}
}
