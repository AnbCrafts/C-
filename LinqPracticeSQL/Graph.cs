using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqPracticeSQL
{
    public class Graph
    {
        public List<string> Edges { get; set; } = new List<string>();

        public Graph() { }

        public Graph(List<string> edges)
        {
            Edges = edges;
        }

        // Static method: builds and returns an adjacency list dictionary from an edge list
        public static Dictionary<char, List<char>> GetAdjacencyList(List<string> edges)
        {
            var validEdges = edges.Where(e => !string.IsNullOrWhiteSpace(e)).ToList();

            var lookup = validEdges.ToLookup(e => e[0], e => e[e.Length - 1]);

            var allVertices = validEdges
                .SelectMany(e => new[] { e[0], e[e.Length - 1] })
                .Distinct();

            return allVertices.ToDictionary(
                v => v,
                v => lookup[v].ToList()
            );
        }

        // Static overload accepting a Graph instance directly
        public static Dictionary<char, List<char>> GetAdjacencyList(Graph graph)
        {
            return GetAdjacencyList(graph.Edges);
        }

        // Alias in case invoked directly as AdjacencyList
        public static Dictionary<char, List<char>> AdjacencyList(Graph graph) => GetAdjacencyList(graph.Edges);
        public static Dictionary<char, List<char>> AdjacencyList(List<string> edges) => GetAdjacencyList(edges);

        // Static factory method providing sample graphs
        public static List<Graph> GetGraphs()
        {
            var graphs = new List<Graph>();

            // Graph 1: Simple DAG
            graphs.Add(new Graph(new List<string> { "AB", "AC", "BD", "CD" }));

            // Graph 2: Vertex 'A' has out-degree > 3 (connects to B, C, D, E)
            graphs.Add(new Graph(new List<string> { "AB", "AC", "AD", "AE", "BC", "CD" }));

            // Graph 3: Disconnected components
            graphs.Add(new Graph(new List<string> { "AB", "BC", "XY", "YZ" }));

            return graphs;
        }
    }
}
