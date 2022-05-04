using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DelaunayVoronoi.Tests;

[TestFixture]
internal sealed class VoronoiFactoryTests {

	private const int Width = 1000;
	private const int Height = 1000;
	private IDelaunatorFactory _delaunatorFactory;
	private IVoronoiFactory _voronoiFactory;
	private Voronoi _voronoi;
	private Point _p1;
	private Point _p2;
	private Point _p3;
	private Point _p4;
	private Point _p5;

	[OneTimeSetUp]
	public void OneTimeSetUp() {
		_delaunatorFactory = new DelaunatorFactory();
		_voronoiFactory = new VoronoiFactory();
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
		_voronoi = _voronoiFactory.Create( delaunator, Width, Height );
	}

	[Test]
	public void Create_ReferencePoints_1Closed4Open() {
		Assert.AreEqual( 1, _voronoi.Cells.Count( c => !c.IsOpen ) );
		Assert.AreEqual( 4, _voronoi.Cells.Count( c => c.IsOpen ) );

		Point p1 = new Point( 500, 250 );
		Point p2 = new Point( 750, 500 );
		Point p3 = new Point( 500, 750 );
		Point p4 = new Point( 250, 500 );

		Cell c = _voronoi.Cells[2];
		Assert.IsFalse( c.IsOpen );
		Assert.AreEqual( p1, c.Points[0] );
		Assert.AreEqual( p2, c.Points[1] );
		Assert.AreEqual( p3, c.Points[2] );
		Assert.AreEqual( p4, c.Points[3] );
	}

	[Test]
	[Ignore("Used to create visual output for inspection.")]
	public void Visualize() {
		string folder = Path.Combine( Path.GetTempPath(), "delaunayvoronoi" );
		int width = 1000;
		int height = 1000;
		Directory.CreateDirectory( folder );

		/*
		List<Point> points = new List<Point>();
		while( points.Count < 4000 ) {
			Point newPoint = new Point(
				_random.Next( width ),
				_random.Next( height )
			);
			// Ensure there are no adjacent or coincident points
			if( !points.Any( p =>
					Math.Abs( p.X - newPoint.X ) <= 1
					&& Math.Abs( p.Y - newPoint.Y ) <= 1
				)
			) {
				points.Add( newPoint );
			}
		}
		*/
		List<Point> points = new List<Point>() {
			_p1,
			_p2,
			_p3,
			_p4,
			_p5
		};
		IDelaunatorFactory delaunatorFactory = new DelaunatorFactory();
		Delaunator delaunator = delaunatorFactory.Create( points );

		Voronoi voronoi = _voronoiFactory.Create( delaunator, width, height );

		using Image<Rgba32> image = new Image<Rgba32>( width, height );

		PointF[] plots = new PointF[2];


		image.Mutate( i => i.Fill( Color.Black ) );
		foreach( Edge e in voronoi.Edges ) {
			plots[0].X = e.A.X;
			plots[0].Y = e.A.Y;
			plots[1].X = e.B.X;
			plots[1].Y = e.B.Y;
			image.Mutate( i => i.DrawLines( Color.Blue, 1.0f, plots ) );
		}
		foreach( Point p in points ) {
			image[p.X, p.Y] = Color.White;
		}
		image.Save( Path.Combine( folder, "voronoi_edges.png" ) );

		image.Mutate( i => i.Fill( Color.Black ) );
		foreach( Cell c in voronoi.Cells.Where( c => !c.IsOpen ) ) {
			for (int i = 0; i < c.Points.Count-1; i++) {
				plots[0].X = c.Points[i].X;
				plots[0].Y = c.Points[i].Y;
				plots[1].X = c.Points[i+1].X;
				plots[1].Y = c.Points[i+1].Y;
				image.Mutate( i => i.DrawLines( Color.Blue, 1.0f, plots ) );
			}
			plots[0].X = c.Points[^1].X;
			plots[0].Y = c.Points[^1].Y;
			plots[1].X = c.Points[0].X;
			plots[1].Y = c.Points[0].Y;
			image.Mutate( i => i.DrawLines( Color.Blue, 1.0f, plots ) );
		}
		foreach( Point p in points ) {
			image[p.X, p.Y] = Color.White;
		}
		image.Save( Path.Combine( folder, "voronoi_cells.png" ) );
	}
}
