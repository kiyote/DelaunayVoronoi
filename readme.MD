#Delaunay-Voronoi
A library for producing Delauay triangulations and Voronoi graphs.

Basic usage for DI.  
Register the appropriate factory using `IServiceCollection.AddDelaunay` or `IServiceCollection.AddVoronoi`.
Then, create a Delaunator with your points, and then create a Voronoi diagram from that Delaunator.
```
public class Test(
	IDelaunatorFactory delaunatorFactory,
	IVoronoiFactory voronoiFactory
) {
	List<Point> points = new List<Point>() {
		new Point( 250, 250 ),
		new Point( 500, 250 ),
		new Point( 500, 500 ),
		new Point( 250, 720 ),
		new Point( 750, 750 )
	};

	int width = 1000;
	int height = 1000;
	Delaunator delaunator = delanaunatorFactory.Create( points );
	Voronoi voronoi = voronoiFactory.Create( delaunator, width, height );
}
```