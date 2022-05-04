namespace DelaunayVoronoi;

public interface IDelaunatorFactory {
	Delaunator Create( IEnumerable<IPoint> points );
}
