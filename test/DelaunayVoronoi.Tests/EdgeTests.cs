using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DelaunayVoronoi.Tests;

[TestFixture]
internal sealed class EdgeTests {

	private Point _a;
	private Point _b;
	private Edge _edge;


	[SetUp]
	public void SetUp() {
		_a = new Point( 1, 2 );
		_b = new Point( 3, 4 );
		_edge = new Edge( _a, _b );
	}

	[Test]
	public void Ctor_ValidProperties_PropertiesAssignedCorrectly() {
		Point a = new Point( 10, 11 );
		Point b = new Point( -5, -6 );
		Edge edge = new Edge( a, b );

		Assert.AreSame( a, edge.A );
		Assert.AreSame( b, edge.B );
	}

	[TestCaseSource(nameof(EqualsPointPointTestCaseSource))]
	public void EqualsPointPoint(
		Point a,
		Point b,
		bool expected
	) {
		Assert.AreEqual( expected, _edge.Equals( a, b ) );
	}

	private static IEnumerable<object> EqualsPointPointTestCaseSource() {
		yield return new object[] {
			new Point( 1, 2 ),
			new Point( 3, 4 ),
			true
		};
		yield return new object[] {
			new Point( 3, 4 ),
			new Point( 1, 2 ),
			true
		};
		yield return new object[] {
			new Point( 4, 3 ),
			new Point( 2, 1 ),
			false
		};
	}

	[TestCaseSource(nameof(EqualsEdgeTestCaseSource))]
	public void EqualsEdge(
		Edge other,
		bool expected
	) {
		Assert.AreEqual( expected, _edge.Equals( other ) );
	}

	private static IEnumerable<object> EqualsEdgeTestCaseSource() {
		yield return new object[] {
			new Edge(
				new Point( 1, 2 ),
				new Point( 3, 4 )
			),
			true
		};
		yield return new object[] {
			new Edge(
				new Point( 3, 4 ),
				new Point( 1, 2 )
			),
			true
		};
		yield return new object[] {
			new Edge(
				new Point( 4, 3 ),
				new Point( 2, 1 )
			),
			false
		};
		yield return new object[] { null, false };
	}

	[TestCaseSource(nameof(EqualsObjectTestCaseSource))]
	public void EqualsObject(
		object other,
		bool expected
	) {
		Assert.AreEqual( expected, _edge.Equals( other ) );
	}

	private static IEnumerable<object> EqualsObjectTestCaseSource() {
		yield return new object[] { null, false };
		yield return new object[] { 123.4D, false };
		yield return new object[] {
			new Edge(
				new Point( 1, 2 ),
				new Point( 3, 4 )
			),
			true
		};
		yield return new object[] {
			new Edge(
				new Point( 3, 4 ),
				new Point( 1, 2 )
			),
			true
		};
		yield return new object[] {
			new Edge(
				new Point( 4, 3 ),
				new Point( 2, 1 )
			),
			false
		};
	}

	[TestCaseSource(nameof(GetHashCodeTestCaseSource))]
	public void GetHashCode(
		Edge other,
		bool expected
	) {
		Assert.AreEqual( expected, _edge.GetHashCode() == other.GetHashCode() );
	}

	private static IEnumerable<object> GetHashCodeTestCaseSource() {
		yield return new object[] {
			new Edge(
				new Point( 1, 2 ),
				new Point( 3, 4 )
			),
			true
		};
		/* Perhaps .Equals should be .Coincident
		yield return new object[] {
			new Edge(
				new Point( 3, 4 ),
				new Point( 1, 2 )
			),
			true
		};
		*/
		yield return new object[] {
			new Edge(
				new Point( 4, 3 ),
				new Point( 2, 1 )
			),
			false
		};
	}
}
