namespace DelaunayVoronoi;

// Logic ported from https://github.com/d3/d3-delaunay
internal sealed class DelaunayFactory : IDelaunayFactory {
	Delaunay IDelaunayFactory.Create(
		Delaunator delaunator
	) {
		Point[] points = new Point[delaunator.Coords.Count / 2];
		List<Edge> edges;
		List<Triangle> triangles;

		int n = delaunator.Coords.Count / 2;
		for( int i = 0; i < n; i++ ) {
			points[i] = new Point(
				(int)delaunator.Coords[i * 2],
				(int)delaunator.Coords[( i * 2 ) + 1]
			);
		}

		triangles = new List<Triangle>();
		n = delaunator.Triangles.Count / 3;
		for( int i = 0; i < n; i++ ) {
			triangles.Add(
				new Triangle(
					points[delaunator.Triangles[( i * 3 )]],
					points[delaunator.Triangles[( i * 3 ) + 1]],
					points[delaunator.Triangles[( i * 3 ) + 2]]
				)
			);
		}

		edges = new List<Edge>();
		for( int i = 0; i < delaunator.Triangles.Count; i++ ) {
			if( i > delaunator.HalfEdges[i] ) {
				Point p = points[delaunator.Triangles[i]];
				Point q = points[delaunator.Triangles[Delaunator.NextHalfedge( i )]];
				edges.Add(
					new Edge( p, q )
				);
			}
		}

		IDictionary<Triangle, IReadOnlyList<Triangle>> neighbours = MakeNeighbours( delaunator, triangles );
		
		return new Delaunay( points, edges, triangles, neighbours );
	}

	private static IDictionary<Triangle, IReadOnlyList<Triangle>> MakeNeighbours(
		Delaunator delaunator,
		List<Triangle> triangles
	) {
		List<Triangle>[] neighbours = new List<Triangle>[triangles.Count];
		for (int i = 0; i < triangles.Count; i++) {
			neighbours[i] = new List<Triangle>();
		}
		for (int i = 0; i < delaunator.HalfEdges.Count;i++ ) {
			int e = delaunator.HalfEdges[i];
			if (e != -1) {
				int t = e / 3;
				Triangle adj = triangles[delaunator.HalfEdges[e] / 3];
				neighbours[t].Add( adj );
			}
		}

		Dictionary<Triangle, IReadOnlyList<Triangle>> result = new Dictionary<Triangle, IReadOnlyList<Triangle>>();
		for (int i = 0; i < triangles.Count; i++) {
			result[triangles[i]] = neighbours[i];
		}
		return result;
	}
}
