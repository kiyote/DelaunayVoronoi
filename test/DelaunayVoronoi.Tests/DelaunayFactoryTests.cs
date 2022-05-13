using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DelaunayVoronoi.Tests;

[TestFixture]
internal sealed class DelaunayFactoryTests {

	private IDelaunayFactory _delaunayFactory;
	private IDelaunatorFactory _delaunatorFactory;
	private Delaunay _delaunay;
	private Point _p1;
	private Point _p2;
	private Point _p3;
	private Point _p4;
	private Point _p5;

	[OneTimeSetUp]
	public void OneTimeSetUp() {
		_delaunayFactory = new DelaunayFactory();
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
		Delaunator delaunator = _delaunatorFactory.Create( points );
		_delaunay = _delaunayFactory.Create( delaunator );
	}

	[Test]
	public void Create_ReferencePoints_PointsMatch() {
		Assert.AreEqual( 5, _delaunay.Points.Count );

		Assert.AreEqual( _p1.X, _delaunay.Points[0].X );
		Assert.AreEqual( _p1.Y, _delaunay.Points[0].Y );
		Assert.AreEqual( _p2.X, _delaunay.Points[1].X );
		Assert.AreEqual( _p2.Y, _delaunay.Points[1].Y );
		Assert.AreEqual( _p3.X, _delaunay.Points[2].X );
		Assert.AreEqual( _p3.Y, _delaunay.Points[2].Y );
		Assert.AreEqual( _p4.X, _delaunay.Points[3].X );
		Assert.AreEqual( _p4.Y, _delaunay.Points[3].Y );
		Assert.AreEqual( _p5.X, _delaunay.Points[4].X );
		Assert.AreEqual( _p5.Y, _delaunay.Points[4].Y );
	}

	[Test]
	public void Create_ReferencePoints_ReturnsDistinctPoints() {
		Assert.AreEqual( _delaunay.Points.Count, _delaunay.Points.Distinct().Count() );
	}

	[Test]
	public void Create_ReferencePoints_ReturnsDistinctEdges() {
		Assert.AreEqual( _delaunay.Edges.Count, _delaunay.Edges.Distinct().Count() );
	}

	[Test]
	public void Create_ReferencePoints_CorrectEdges() {
		Assert.AreEqual( 8, _delaunay.Edges.Count );

		Assert.AreEqual( _p2, _delaunay.Edges[0].A );
		Assert.AreEqual( _p1, _delaunay.Edges[0].B );

		Assert.AreEqual( _p1, _delaunay.Edges[1].A );
		Assert.AreEqual( _p4, _delaunay.Edges[1].B );

		Assert.AreEqual( _p3, _delaunay.Edges[2].A );
		Assert.AreEqual( _p1, _delaunay.Edges[2].B );

		Assert.AreEqual( _p4, _delaunay.Edges[3].A );
		Assert.AreEqual( _p5, _delaunay.Edges[3].B );

		Assert.AreEqual( _p3, _delaunay.Edges[4].A );
		Assert.AreEqual( _p4, _delaunay.Edges[4].B );

		Assert.AreEqual( _p3, _delaunay.Edges[5].A );
		Assert.AreEqual( _p5, _delaunay.Edges[5].B );

		Assert.AreEqual( _p5, _delaunay.Edges[6].A );
		Assert.AreEqual( _p2, _delaunay.Edges[6].B );

		Assert.AreEqual( _p2, _delaunay.Edges[7].A );
		Assert.AreEqual( _p3, _delaunay.Edges[7].B );
	}

	[Test]
	public void Create_ReferencePoints_4Triangles() {
		Assert.AreEqual( 4, _delaunay.Triangles.Count );

		Assert.AreEqual( _p3, _delaunay.Triangles[0].P1 );
		Assert.AreEqual( _p2, _delaunay.Triangles[0].P2 );
		Assert.AreEqual( _p1, _delaunay.Triangles[0].P3 );

		Assert.AreEqual( _p1, _delaunay.Triangles[1].P1 );
		Assert.AreEqual( _p4, _delaunay.Triangles[1].P2 );
		Assert.AreEqual( _p3, _delaunay.Triangles[1].P3 );

		Assert.AreEqual( _p4, _delaunay.Triangles[2].P1 );
		Assert.AreEqual( _p5, _delaunay.Triangles[2].P2 );
		Assert.AreEqual( _p3, _delaunay.Triangles[2].P3 );

		Assert.AreEqual( _p3, _delaunay.Triangles[3].P1 );
		Assert.AreEqual( _p5, _delaunay.Triangles[3].P2 );
		Assert.AreEqual( _p2, _delaunay.Triangles[3].P3 );
	}

	[Test]
	public void Create_ReferencePoints_CorrectNeighbours() {
		Assert.AreEqual( 4, _delaunay.Neighbours.Count );

		Assert.AreEqual( 2, _delaunay.Neighbours[_delaunay.Triangles[0]].Count );
		Assert.AreSame( _delaunay.Triangles[1], _delaunay.Neighbours[_delaunay.Triangles[0]][0] );
		Assert.AreSame( _delaunay.Triangles[3], _delaunay.Neighbours[_delaunay.Triangles[0]][1] );

		Assert.AreEqual( 2, _delaunay.Neighbours[_delaunay.Triangles[1]].Count );
		Assert.AreSame( _delaunay.Triangles[0], _delaunay.Neighbours[_delaunay.Triangles[1]][0] );
		Assert.AreSame( _delaunay.Triangles[2], _delaunay.Neighbours[_delaunay.Triangles[1]][1] );

		Assert.AreEqual( 2, _delaunay.Neighbours[_delaunay.Triangles[2]].Count );
		Assert.AreSame( _delaunay.Triangles[1], _delaunay.Neighbours[_delaunay.Triangles[2]][0] );
		Assert.AreSame( _delaunay.Triangles[3], _delaunay.Neighbours[_delaunay.Triangles[2]][1] );

		Assert.AreEqual( 2, _delaunay.Neighbours[_delaunay.Triangles[3]].Count );
		Assert.AreSame( _delaunay.Triangles[0], _delaunay.Neighbours[_delaunay.Triangles[3]][0] );
		Assert.AreSame( _delaunay.Triangles[2], _delaunay.Neighbours[_delaunay.Triangles[3]][1] );
	}

	[Test]
	[Ignore("Used to visualize output for inspection.")]
	public void Visualize() {
		string folder = Path.Combine( Path.GetTempPath(), "delaunayvoronoi" );
		Directory.CreateDirectory( folder );

		List<Point> points = new List<Point>() {
			new Point( 250, 250 ),
			new Point( 750, 250 ),
			new Point( 500, 500 ),
			new Point( 250, 750 ),
			new Point( 750, 750 )
		};
		Delaunator delaunator = _delaunatorFactory.Create( points );
		Delaunay delaunay = _delaunayFactory.Create( delaunator );

		using Image<Rgba32> image = new Image<Rgba32>( 1000, 1000 );
		image.Mutate( i => i.Fill( Color.Black ) );

		PointF[] plots = new PointF[2];
		foreach( Edge e in delaunay.Edges ) {
			plots[0].X = e.A.X;
			plots[0].Y = e.A.Y;
			plots[1].X = e.B.X;
			plots[1].Y = e.B.Y;
			image.Mutate( i => i.DrawLines( Color.Maroon, 1.0f, plots ) );
		}

		foreach( Point p in points ) {
			image[p.X, p.Y] = Color.White;
		}

		image.Save( Path.Combine( folder, "delaunay_edges.png" ) );
	}
}
