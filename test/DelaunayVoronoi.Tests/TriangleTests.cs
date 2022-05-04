using NUnit.Framework;

namespace DelaunayVoronoi.Tests;

[TestFixture]
internal sealed class TriangleTests {

	private Point _p1;
	private Point _p2;
	private Point _p3;
	private Triangle _triangle;

	[SetUp]
	public void SetUp() {
		_p1 = new Point( 1, 0 );
		_p2 = new Point( 0, 2 );
		_p3 = new Point( 2, 2 );
		_triangle = new Triangle( _p1, _p2, _p3 );
	}

	[Test]
	public void Ctor_ValidParameters_PropertiesAssignedCorrectly() {
		Assert.AreSame( _p1, _triangle.P1 );
		Assert.AreSame( _p2, _triangle.P2 );
		Assert.AreSame( _p3, _triangle.P3 );
	}
}
