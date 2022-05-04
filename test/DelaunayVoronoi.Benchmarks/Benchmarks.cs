using BenchmarkDotNet.Attributes;

namespace DelaunayVoronoi.Benchmarks;

[MemoryDiagnoser]
public class Benchmarks {

	private const int Width = 1000;
	private const int Height = 1000;
	private readonly IVoronoiFactory _voronoiFactory;
	private readonly IDelaunatorFactory _delaunatorFactory;
	private readonly IDelaunayFactory _delaunayFactory;
	private readonly Delaunator _delaunator;
	private readonly List<Point> _points;

	public Benchmarks() {
		_delaunatorFactory = new DelaunatorFactory();
		_voronoiFactory = new VoronoiFactory();
		_delaunayFactory = new DelaunayFactory();
		_points = new List<Point>();
		Random random = new Random();
		while (_points.Count < 4000) {
			Point point = new Point(
				random.Next( Width ),
				random.Next( Height )
			);
			if (!_points.Any( p => p.Equals( point ))) {
				_points.Add( point );
			}
		}
		_delaunator = _delaunatorFactory.Create( _points );
	}

	[Benchmark]
	public void CreateDelaunator() {
		_delaunatorFactory.Create( _points );
	}

	[Benchmark]
	public void CreateVoronoi() {
		_voronoiFactory.Create( _delaunator, Width, Height );
	}

	[Benchmark]
	public void CreateDelaunay() {
		_delaunayFactory.Create( _delaunator );
	}
}
