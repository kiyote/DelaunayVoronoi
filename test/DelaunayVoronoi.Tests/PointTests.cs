using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DelaunayVoronoi.Tests;

[TestFixture]
internal sealed class PointTests {

	private const int X = 1;
	private const int Y = 2;
	private Point _point;

	[SetUp]
	public void SetUp() {
		_point = new Point( X, Y );
	}

	[Test]
	public void Ctor_ValidProperties_PropertiesAssignedCorrectly() {
		int x = 3;
		int y = 4;
		Point point = new Point( x, y );

		Assert.AreEqual( x, point.X );
		Assert.AreEqual( y, point.Y );
	}

	[TestCaseSource(nameof(EqualsIntIntTestCaseSource))]
	public void EqualsIntInt(
		int x,
		int y,
		bool expected
	) {
		Assert.AreEqual( expected, _point.Equals( x, y) );
	}

	private static IEnumerable<object> EqualsIntIntTestCaseSource() {
		yield return new object[] { X, Y, true };
		yield return new object[] { Y, X, false };
	}


	[TestCaseSource(nameof(EqualsIPointTestCaseSource))]
	public void EqualsIPoint(
		IPoint other,
		bool expected
	) {
		Assert.AreEqual( expected, _point.Equals( other ) );
	}

	private static IEnumerable<object> EqualsIPointTestCaseSource() {
		yield return new object[] { null, false };
		yield return new object[] { new Point( X, Y ), true };
		yield return new object[] { new Point( Y, X ), false };
	}

	[TestCaseSource(nameof(EqualsObjectTestCaseSource))]
	public void EqualsObject(
		object other,
		bool expected
	) {
		Assert.AreEqual( expected, _point.Equals( other ) );
	}

	private static IEnumerable<object> EqualsObjectTestCaseSource() {
		yield return new object[] { null, false };
		yield return new object[] { 1.0f, false };
		yield return new object[] { new Point( X, Y ), true };
		yield return new object[] { new Point( Y, X ), false };
		yield return new object[] { new { X = X, Y = Y}, false };
	}

	[TestCaseSource(nameof(HashCodeTestCaseSource))]
	public void HashCode(
		Point other,
		bool expected
	) {
		Assert.AreEqual( expected, _point.GetHashCode() == other.GetHashCode() );
	}

	private static IEnumerable<object> HashCodeTestCaseSource() {
		yield return new object[] { new Point( X, Y ), true };
		yield return new object[] { new Point( Y, X ), false };
	}
}
