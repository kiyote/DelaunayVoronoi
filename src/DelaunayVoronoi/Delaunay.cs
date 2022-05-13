namespace DelaunayVoronoi;

public sealed record Delaunay(
	IReadOnlyList<Point> Points,
	IReadOnlyList<Edge> Edges,
	IReadOnlyList<Triangle> Triangles,
	IDictionary<Triangle, IReadOnlyList<Triangle>> Neighbours
);
