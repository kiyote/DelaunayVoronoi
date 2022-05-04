using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DelaunayVoronoi.Tests;

[TestFixture]
internal sealed class DelaunatorFactoryTests {

	private IDelaunatorFactory _delaunatorFactory;
	private Delaunator _delaunator;
	private Point _p1;
	private Point _p2;
	private Point _p3;
	private Point _p4;
	private Point _p5;

	[OneTimeSetUp]
	public void OneTimeSetUp() {
		_delaunatorFactory = new DelaunatorFactory();
	}

	[SetUp]
	public void SetUp() {
		_p1 = new Point( 250, 250 );
		_p2 = new Point( 750, 250 );
		_p3 = new Point( 500, 500 );
		_p4 = new Point( 250, 750 );
		_p5 = new Point( 750, 750 );
		List<Point> points = new List<Point>() {
			_p1,
			_p2,
			_p3,
			_p4,
			_p5
		};
		_delaunator = _delaunatorFactory.Create( points );
	}

	[Test]
	public void Create_ReferencePoints_CoordsMatchPoints() {
		Assert.AreEqual( _p1.X, _delaunator.Coords[0] );
		Assert.AreEqual( _p1.Y, _delaunator.Coords[1] );
		Assert.AreEqual( _p2.X, _delaunator.Coords[2] );
		Assert.AreEqual( _p2.Y, _delaunator.Coords[3] );
		Assert.AreEqual( _p3.X, _delaunator.Coords[4] );
		Assert.AreEqual( _p3.Y, _delaunator.Coords[5] );
		Assert.AreEqual( _p4.X, _delaunator.Coords[6] );
		Assert.AreEqual( _p4.Y, _delaunator.Coords[7] );
		Assert.AreEqual( _p5.X, _delaunator.Coords[8] );
		Assert.AreEqual( _p5.Y, _delaunator.Coords[9] );
	}

	[Test]
	public void Create_InsufficientPoints_ThrowsException() {
		List<Point> points = new List<Point>() {
			new Point( 0, 0 ),
			new Point( 500, 500 )
		};

		Assert.Throws<ArgumentException>( () => _delaunatorFactory.Create( points ) );
	}

	[Test]
	public void Create_CoincidentPoints_ThrowsException() {
		Point p = new Point( 500, 500 );
		List<Point> points = new List<Point>() {
			p,
			p,
			p
		};

		Assert.Throws<ArgumentException>( () => _delaunatorFactory.Create( points ) );
	}

	[Test]
	public void Create_CollinearPoints_ThrowsException() {
		List<Point> points = new List<Point>() {
			new Point( 0, 0 ),
			new Point( 500, 500 ),
			new Point( 1000, 1000)
		};

		Assert.Throws<InvalidOperationException>( () => _delaunatorFactory.Create( points ) );
	}

	[Test]
	public void Create_ReferencePoints_4Triangles() {
		Assert.AreEqual( 4 * 3, _delaunator.Triangles.Count );

		int t = _delaunator.Triangles[0];
		Assert.AreEqual( _p3.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p3.Y, _delaunator.Coords[( t * 2 ) + 1] );
		t = _delaunator.Triangles[1];
		Assert.AreEqual( _p2.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p2.Y, _delaunator.Coords[( t * 2 ) + 1] );
		t = _delaunator.Triangles[2];
		Assert.AreEqual( _p1.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p1.Y, _delaunator.Coords[( t * 2 ) + 1] );

		t = _delaunator.Triangles[3];
		Assert.AreEqual( _p1.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p1.Y, _delaunator.Coords[( t * 2 ) + 1] );
		t = _delaunator.Triangles[4];
		Assert.AreEqual( _p4.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p4.Y, _delaunator.Coords[( t * 2 ) + 1] );
		t = _delaunator.Triangles[5];
		Assert.AreEqual( _p3.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p3.Y, _delaunator.Coords[( t * 2 ) + 1] );

		t = _delaunator.Triangles[6];
		Assert.AreEqual( _p4.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p4.Y, _delaunator.Coords[( t * 2 ) + 1] );
		t = _delaunator.Triangles[7];
		Assert.AreEqual( _p5.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p5.Y, _delaunator.Coords[( t * 2 ) + 1] );
		t = _delaunator.Triangles[8];
		Assert.AreEqual( _p3.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p3.Y, _delaunator.Coords[( t * 2 ) + 1] );

		t = _delaunator.Triangles[9];
		Assert.AreEqual( _p3.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p3.Y, _delaunator.Coords[( t * 2 ) + 1] );
		t = _delaunator.Triangles[10];
		Assert.AreEqual( _p5.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p5.Y, _delaunator.Coords[( t * 2 ) + 1] );
		t = _delaunator.Triangles[11];
		Assert.AreEqual( _p2.X, _delaunator.Coords[t * 2] );
		Assert.AreEqual( _p2.Y, _delaunator.Coords[( t * 2 ) + 1] );
	}

	[Test]
	public void Create_ReferencePoints_HullIsSquare() { 
		Assert.AreEqual( 4, _delaunator.Hull.Count );

		int c = _delaunator.Hull[0];
		Assert.AreEqual( _p4.X, _delaunator.Coords[c * 2] );
		Assert.AreEqual( _p4.Y, _delaunator.Coords[( c * 2 ) + 1] );

		c = _delaunator.Hull[1];
		Assert.AreEqual( _p5.X, _delaunator.Coords[c * 2] );
		Assert.AreEqual( _p5.Y, _delaunator.Coords[( c * 2 ) + 1] );

		c = _delaunator.Hull[2];
		Assert.AreEqual( _p2.X, _delaunator.Coords[c * 2] );
		Assert.AreEqual( _p2.Y, _delaunator.Coords[( c * 2 ) + 1] );

		c = _delaunator.Hull[3];
		Assert.AreEqual( _p1.X, _delaunator.Coords[c * 2] );
		Assert.AreEqual( _p1.Y, _delaunator.Coords[( c * 2 ) + 1] );
	}

	[Test]
	public void Create_ReferencePoints_HalfEdgesForTriangles() {
		Assert.AreEqual( 4 * 3, _delaunator.Triangles.Count );

		Assert.AreEqual( 11, _delaunator.HalfEdges[0] );
		Assert.AreEqual( -1, _delaunator.HalfEdges[1] );
		Assert.AreEqual( 5, _delaunator.HalfEdges[2] );

		Assert.AreEqual( -1, _delaunator.HalfEdges[3] );
		Assert.AreEqual( 8, _delaunator.HalfEdges[4] );
		Assert.AreEqual( 2, _delaunator.HalfEdges[5] );

		Assert.AreEqual( -1, _delaunator.HalfEdges[6] );
		Assert.AreEqual( 9, _delaunator.HalfEdges[7] );
		Assert.AreEqual( 4, _delaunator.HalfEdges[8] );

		Assert.AreEqual( 7, _delaunator.HalfEdges[9] );
		Assert.AreEqual( -1, _delaunator.HalfEdges[10] );
		Assert.AreEqual( 0, _delaunator.HalfEdges[11] );
	}

	[Test]
	[Ignore("Used to visualize output for inspection.")]
	public void Visualize() {
		string folder = Path.Combine( Path.GetTempPath(), "delaunayvoronoi" );
		Directory.CreateDirectory( folder );

		using Image<Rgba32> image = new Image<Rgba32>( 1000, 1000 );

		List<Point> points = new List<Point>() {
			new Point( 250, 250 ),
			new Point( 750, 250 ),
			new Point( 500, 500 ),
			new Point( 250, 750 ),
			new Point( 750, 750 )
		};
		Delaunator delaunator = _delaunatorFactory.Create( points );

		PointF[] plots = new PointF[2];

		image.Mutate( i => i.Fill( Color.Black ) );
		Color[] colors = new Color[4] { Color.Red, Color.Green, Color.Blue, Color.Yellow };
		for( int i = 0; i < delaunator.Hull.Count - 1; i++ ) {

			int c = delaunator.Hull[i] * 2;
			plots[0].X = (float)delaunator.Coords[c];
			plots[0].Y = (float)delaunator.Coords[c + 1];
			c = delaunator.Hull[i + 1] * 2;
			plots[1].X = (float)delaunator.Coords[c];
			plots[1].Y = (float)delaunator.Coords[c + 1];
			image.Mutate( img => img.DrawLines( colors[i], 1.0f, plots ) );
		}
		plots[0].X = (float)delaunator.Coords[delaunator.Hull[^1] * 2];
		plots[0].Y = (float)delaunator.Coords[( delaunator.Hull[^1] * 2 ) + 1];
		plots[1].X = (float)delaunator.Coords[delaunator.Hull[0] * 2];
		plots[1].Y = (float)delaunator.Coords[( delaunator.Hull[0] * 2 ) + 1];
		image.Mutate( i => i.DrawLines( colors[^1], 1.0f, plots ) );

		image.Save( Path.Combine( folder, "delaunator_hull.png" ) );

		Color[] colours = new Color[4] { Color.Red, Color.Green, Color.Blue, Color.Yellow };
		plots = new PointF[4];
		image.Mutate( i => i.Fill( Color.Black ) );
		for( int i = 0; i < delaunator.Triangles.Count; i += 3 ) {

			int c = delaunator.Triangles[i] * 2;
			plots[0].X = (float)delaunator.Coords[c];
			plots[0].Y = (float)delaunator.Coords[c + 1];
			c = delaunator.Triangles[i + 1] * 2;
			plots[1].X = (float)delaunator.Coords[c];
			plots[1].Y = (float)delaunator.Coords[c + 1];
			c = delaunator.Triangles[i + 2] * 2;
			plots[2].X = (float)delaunator.Coords[c];
			plots[2].Y = (float)delaunator.Coords[c + 1];
			c = delaunator.Triangles[i] * 2;
			plots[3].X = (float)delaunator.Coords[c];
			plots[3].Y = (float)delaunator.Coords[c + 1];
			image.Mutate( img => img.DrawLines( colours[i / 3], 1.0f, plots ) );
		}

		image.Save( Path.Combine( folder, "delaunator_triangles.png" ) );
	}
}
