using JuanMartin.Kernel.Utilities.DataStructures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using JuanMartin.Kernel.Extesions;

namespace JuanMartin.Kernel.Utilities.DataStructures.Tests
{
    [TestFixture()]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018:Inline variable declaration", Justification = "<Pending>")]
    public class DirectedAcyclicGraphTests
    {
        private Vertex<int> v1, v2, v3, v0;
        private int expected_vertex_count = 6, expected_outgoing_edge_count = 6;
        private DirectedAcyclicGraph<int> graph;

        [SetUp]
        public void Init()
        {
            graph = CreateNumericTestGraph();
        }

        [TearDown]
        public void Dispose()
        {
            v1 = null;
            v2 = null;
            v3 = null;
            v0 = null;

            graph = null;
        }


        [Test()]
        public void TwoVerticesConnectedByAnEdge_MustBeAdjacent()
        {
            Assert.IsTrue(graph.Adjacent(v1, v2));

        }

        [Test()]
        public void TwoVerticesConnectedByAnEdge_MustBeNeighbors()
        {
            // there is an edge from v1 to v2
            var o = v1.OutgoingNeighbors();
            var i = v2.IncomingNeighbors();
            Assert.IsTrue(v1.OutgoingNeighbors().Contains(v2) && v2.IncomingNeighbors().Contains(v1));
        }

        [Test()]
        public void VertexCount_MustCountAllVerticesInGraph()
        {
            Assert.AreEqual(expected_vertex_count, graph.VertexCount());
        }

        [Test()]
        public void EdgeCount_MustCountAllEdgesInGraph()
        {
            Assert.AreEqual(expected_outgoing_edge_count, graph.EdgeCount(Edge<int>.EdgeType.outgoing));
        }

        [Test()]
        public void AddDuplicateVertex_MusNotAddNode()
        {
            // name should be unique
            Assert.IsFalse(graph.AddVertex(1000, v1.Name));
        }

        [Test()]
        public void GettingByNameEdgeRepeatedInGraph_MustUseFromVertexNameToDisambiguate()
        {
            var vertex_add_1 = graph.GetEdge("add", type: Edge<int>.EdgeType.outgoing); //belongs to v1
            var vertex_add_2 = graph.GetEdge("add", "two", type: Edge<int>.EdgeType.outgoing); //belongs to v2
            var vertex_add_3 = graph.GetEdge("add", "four", type: Edge<int>.EdgeType.outgoing); //belongs to v4
            var add_edges = graph.GetOutgoingEdges().Where(e => e.Name == "add").ToList();

            Assert.AreEqual(3, add_edges.Count, "Graph has two 'add' eges");
            Assert.AreNotEqual(vertex_add_1.From, vertex_add_2.From, "Both 'add' edges belong to different vertices");
            Assert.AreEqual("two", vertex_add_2.From.Name, "'vertex_add_2' comes from vertex 'two'");
            Assert.AreEqual("four", vertex_add_3.From.Name, "'vertex_add_3' comes from vertex 'four'");
        }

        [Test()]
        public void ReAddingEdgeBetweenTwoVerticesWithSameName_ShouldIncreaseByOne()
        {
            // check initial weight on edge
            var edge = graph.GetEdge("substract", type: Edge<int>.EdgeType.outgoing);
            Assert.AreEqual(2, edge.Weight);
            // readd edge between same nodes and with same name
            graph.AddEdge(from: v1, to: v2, type: Edge<int>.EdgeType.outgoing, name: "substract");

            // check new weight
            edge = graph.GetEdge("substract", type: Edge<int>.EdgeType.outgoing );

            const int WeightAterReAdd = 3;
            Assert.AreEqual(WeightAterReAdd, edge.Weight);
        }

        [Test()]
        public void MustRemoveVerticesByName()
        {
            var deleted_v1_vertex = graph.RemoveVertex(v1.Name);

            Assert.AreEqual(v1, deleted_v1_vertex);
            Assert.IsFalse(graph.Vertices.Contains(v1));
        }

        [Test()]
        public void AddCyclicEdge_MustThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => graph.AddEdge(from: v3, to: v3, type: Edge<int>.EdgeType.outgoing, name: "cycle", weight: 1));
        }

        [Test()]
        public void AddEdgeWithNoWeight_MustThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => graph.AddEdge(from: v3, to: v3, type: Edge<int>.EdgeType.outgoing, name: "noweight"));
        }

        [Test()]
        public void AddEdgeWithUnexistingFromVertex_MustThrowArgumentException()
        {
            var v = new Vertex<int>(-1, "uknown");

            Assert.Throws<ArgumentException>(() => graph.AddEdge(from: v, to: v3, type: Edge<int>.EdgeType.outgoing, name: "unexisting vertex"));
        }

        [Test()]
        public void AddEdgeWithUnexistingToVertex_MustThrowArgumentException()
        {
            var v = new Vertex<int>(1, "uknown");

            Assert.Throws<ArgumentException>(() => graph.AddEdge(from: v3, to: v, type: Edge<int>.EdgeType.outgoing, name: "unexisting vertex"));
        }

        [Test()]
        public void AddingBidirectionalEdge_MustAddTwoNeighborsAndEgesInBothVerticesInEdge()
        {
            var v1 = new Vertex<string>("A");
            var v2 = new Vertex<string>("B");
            var g = new DirectedAcyclicGraph<string>(new List<Vertex<string>> { v1, v2 });

            g.AddEdge(v1, v2, Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional,"connect",1);

            Assert.IsTrue((v1.Neighbors.Count == 1) && (v2.Neighbors.Count == 1));
            Assert.IsTrue((v1.Edges.Count == 1) && (v2.Edges.Count == 1));
            Assert.IsTrue((v1.OutgoingEdges().Count == 1) && (v1.IncomingEdges().Count == 0) && (v2.OutgoingEdges().Count == 0) && (v2.IncomingEdges().Count == 1));
        }

        [Test()]
        public void IfVertexValueIsDuplicate_MustRemoveNothing()
        {
            var deleted_v1_vertex = graph.RemoveVertex(v1.Value);

            Assert.AreEqual(deleted_v1_vertex, null);
            Assert.IsTrue(graph.Vertices.Contains(v1));
        }

        [Test()]
        public void RemoveEdges_MustOnlyDeleteEdgesBetweenTwoGivenVertices()
        {
            // v1 has two eges to v2 and one to v4
            Assert.AreEqual(3, v1.OutgoingEdges().Count, "Initially there should be three outgoing edges from.");

            // delete edges between v1 and v2
            Assert.AreEqual(2, graph.RemoveEdges(v1, v2,Edge<int>.EdgeType.both).Count, "Between v1 and v2 there are two edges.");
            Assert.AreEqual(1, v1.OutgoingEdges().Count, "After removing the edges from v1 that go to v2 only one remains.");
        }


        [Test()]
        public void GetVertex_MustFindAVertexByName()
        {
            var v1actual = graph.GetVertex(v1.Name);

            Assert.AreEqual(v1, v1actual);
        }

        [Test()]
        public void GetEdgesByName_MustReturnAllEdgesInGraphThatHaveTheSameName()
        {
            var edges = graph.GetEdgesByName("add");

            Assert.AreEqual(3, edges.Count);
        }

        [Test()]
        public void GetRoot_MustRetunOnlyVerticesWithNoIncomingEdges()
        {
            var vertices = graph.GetRoot();

            Assert.AreEqual(1, vertices.Count, "There should be only one vertex returned");
            Assert.AreEqual(v0.Name, vertices[0].Name, "The single root should be the test vertex named zero");

        }


        [Test()]
        public void MultiplePathTestGraph_MustHaveOnlyOneRoot()
        {
            var g = Create_MultiplePathsTestNumericGraph();
            var vertices = g.GetRoot();

            Assert.AreEqual(1, vertices.Count, "There should be only one vertex returned");
            Assert.AreEqual("0", vertices[0].Name, "The single root should be the vertex named '0'");

        }

        [Test()]
        public void StringTestGraph_MustHaveTwoPaths()
        {
            var g = CreateStringTestGraph();
            var paths = g.GetPaths();
            Assert.AreEqual(2, paths.Count);
        }

        [Test()]
        public void NumericTestGraph_MustHaveSixPaths()
        {
            var g = Create_MultiplePathsTestNumericGraph();
            var paths = g.GetPaths();
            Assert.AreEqual(6, paths.Count);
        }

        [Test()]
        public void GetLongestPath_MustGetPathWithLargestWeight()
        {
            var g = CreateStringTestGraph();
            var path = g.GetLongestPath();
            Assert.AreEqual(5, path.Weight);
        }

        [Test()]
        public void ToString_MustReturnStrinRepresentationOfThePlanarizedGraphAsCommaSeparatedListOfItsVerticesWithEdges()
        {
            var expected_representation = "a:[ oneword:a-n:outgoing:1 ],n:[ oneword - return:n-a:incoming:0, oneword:n-d:outgoing:1 ],d:[ oneword - return:d-n:incoming:0 ],";
            var g = CreateStringTestSinglePathGraph();
            var actual_representation = g.ToString(true);

            Assert.AreEqual(expected_representation, actual_representation);
        }

        [Test()]
        public void GetDijkstraShortestPathTest()
        {
            var expected_distance = 7;
            var expected_path = "A,D,E,C";
            var g = CreateComputerScienceGraph();

            var d = g.GetDijkstraShortestPath("A", "C");
            var actual_shortest_path = string.Join(",", d.ShotestPath.Vertices.Select(v=>v.Name));
            var actual_distance = d.Distance;

            Assert.AreEqual(expected_distance, actual_distance);
            Assert.AreEqual(expected_path, actual_shortest_path);
        }

        private DirectedAcyclicGraph<string> CreateStringTestGraph()
        {
            var vertices = new List<Vertex<string>>();

            var a = new Vertex<string>("a");
            var n = new Vertex<string>("n");
            var d = new Vertex<string>("d");
            var e = new Vertex<string>("e");
            var w = new Vertex<string>("w");
            vertices.Add(a);
            vertices.Add(n);
            vertices.Add(d);
            vertices.Add(e);
            vertices.Add(w);

            var g = new DirectedAcyclicGraph<string>(vertices);
            g.AddEdge(from: a, to: n, type: Edge<string>.EdgeType.outgoing, name: "oneword", weight: 1);
            g.AddEdge(from: n, to: d, type: Edge<string>.EdgeType.outgoing, name: "oneword", weight: 1);
            g.AddEdge(from: n, to: e, type: Edge<string>.EdgeType.outgoing, name: "twoword", weight: 2);
            g.AddEdge(from: e, to: w, type: Edge<string>.EdgeType.outgoing, name: "twoword", weight: 2);

            return g;
        }

        private DirectedAcyclicGraph<string> CreateStringTestSinglePathGraph()
        {
            var vertices = new List<Vertex<string>>();

            var a = new Vertex<string>("a");
            var n = new Vertex<string>("n");
            var d = new Vertex<string>("d");
            vertices.Add(a);
            vertices.Add(n);
            vertices.Add(d);

            var g = new DirectedAcyclicGraph<string>(vertices);
            g.AddEdge(from: a, to: n, type: Edge<string>.EdgeType.outgoing, name: "oneword", weight: 1);
            g.AddEdge(from: n, to: d, type: Edge<string>.EdgeType.outgoing, name: "oneword", weight: 1);

            return g;
        }

        private static readonly int[] n = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        /// <summary>
        /// Numeric graph with with six paths one root and one leaf 
        /// </summary>
        /// <returns></returns>
        private DirectedAcyclicGraph<int> Create_MultiplePathsTestNumericGraph()
        {
            var vertices = new List<Vertex<int>>();

            foreach (var i in n)
            {
                vertices.Add(new Vertex<int>(i));
            }

            var g = new DirectedAcyclicGraph<int>(vertices);

            g.AddEdge(vertices[0], vertices[1], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 1);
            g.AddEdge(vertices[0], vertices[2], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 2);
            g.AddEdge(vertices[0], vertices[3], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 3);

            g.AddEdge(vertices[1], vertices[4], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 4);
            g.AddEdge(vertices[1], vertices[5], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 5);

            g.AddEdge(vertices[2], vertices[6], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 6);
            g.AddEdge(vertices[2], vertices[7], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 7);

            g.AddEdge(vertices[3], vertices[8], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 8);
            g.AddEdge(vertices[3], vertices[9], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 9);

            g.AddEdge(vertices[4], vertices[10], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 10);
            g.AddEdge(vertices[5], vertices[10], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 11);
            g.AddEdge(vertices[6], vertices[10], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 12);
            g.AddEdge(vertices[7], vertices[10], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 13);
            g.AddEdge(vertices[8], vertices[10], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 14);
            g.AddEdge(vertices[9], vertices[10], type: Edge<int>.EdgeType.outgoing, Edge<int>.EdgeDirection.bidirectional, null, 15);

            return g;
        }

        /// <summary>
        /// Graph used in FitCode's Dijstra's algorithm video
        /// <see cref="https://www.youtube.com/watch?v=J12sfRYpW-M"/>
        /// </summary>
        /// <returns></returns>
        private DirectedAcyclicGraph<string> CreateFitCodeGraph()
        {
            var vertices = new List<Vertex<string>>();

            foreach (var i in Enumerable.Range(0, 9))
            {
                vertices.Add(new Vertex<string>(i.ToString()));
            }

            var g = new DirectedAcyclicGraph<string>(vertices);

            g.AddEdge(vertices[0], vertices[1], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 4);
            g.AddEdge(vertices[0], vertices[7], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 8);

            g.AddEdge(vertices[1], vertices[7], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 11);
            g.AddEdge(vertices[1], vertices[2], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 8);

            g.AddEdge(vertices[2], vertices[8], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 2);
            g.AddEdge(vertices[2], vertices[5], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 4);
            g.AddEdge(vertices[2], vertices[3], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 7);

            g.AddEdge(vertices[8], vertices[7], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 7);
            g.AddEdge(vertices[8], vertices[6], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 6);

            g.AddEdge(vertices[3], vertices[4], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 9);
            g.AddEdge(vertices[3], vertices[5], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 0);

            g.AddEdge(vertices[5], vertices[6], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 2);

            g.AddEdge(vertices[7], vertices[6], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 1);

            g.AddEdge(vertices[6], vertices[5], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 2);

            g.AddEdge(vertices[5], vertices[4], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 10);


            return g;
        }

        /// <summary>ComputerScience's Dijstra's algorithm video
        /// <see cref="https://www.youtube.com/watch?v=pVfj6mxhdMw"/>
        /// </summary>
        /// <returns></returns>
        private DirectedAcyclicGraph<string> CreateComputerScienceGraph()
        {
            var vertices = new List<Vertex<string>>();

            foreach (var i in Enumerable.Range(0, 5))
            {
                vertices.Add(new Vertex<string>(Convert.ToChar(i + 65).ToString()));
            }

            var g = new DirectedAcyclicGraph<string>(vertices);


            g.AddEdge(vertices[0], vertices[1], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 6);
            g.AddEdge(vertices[0], vertices[3], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 1);

            g.AddEdge(vertices[3], vertices[4], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 1);
            g.AddEdge(vertices[3], vertices[1], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 2);

            g.AddEdge(vertices[4], vertices[1], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 2);
            g.AddEdge(vertices[4], vertices[2], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 5);

            g.AddEdge(vertices[1], vertices[2], type: Edge<string>.EdgeType.outgoing, Edge<string>.EdgeDirection.bidirectional, null, 5);



            return g;
        }

        /// <summary>
        /// Create numeric values graph with three paths and one root
        /// </summary
        private DirectedAcyclicGraph<int> CreateNumericTestGraph()
        {
            var vertices = new List<Vertex<int>>();

            v0 = new Vertex<int>(0, "zero"); // this will be the only root of this graph: no incoming edges to it
            v1 = new Vertex<int>(1, "one");
            v2 = new Vertex<int>(2, "two");
            v3 = new Vertex<int>(3, "three");
            var v4 = new Vertex<int>(1, "four");
            var v5 = new Vertex<int>(5, "five");
            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            vertices.Add(v5);
            expected_vertex_count = vertices.Count;

            var g = new DirectedAcyclicGraph<int>(vertices);
            g.AddEdge(from: v0, to: v1, name: "start", type: Edge<int>.EdgeType.outgoing, weight: 5);
            g.AddEdge(from: v1, to: v2, name: "add", type: Edge<int>.EdgeType.outgoing, weight: 4);
            g.AddEdge(from: v2, to: v3, name: "add", type: Edge<int>.EdgeType.outgoing, weight: 1);
            g.AddEdge(from: v1, to: v2, name: "substract", type: Edge<int>.EdgeType.outgoing, weight: 2);
            g.AddEdge(from: v1, to: v4, name: "copy", type: Edge<int>.EdgeType.outgoing, weight: 3);
            g.AddEdge(from: v4, to: v5, name: "add", type: Edge<int>.EdgeType.outgoing, weight: 6);

            expected_outgoing_edge_count = 6;

            return g;
            // graph:
            // vertex (neighbors,edges)
            // vertex -weight-> vertex
            //
            // v0 (1,1) -5-> v1 (3,3) -2-> v2 (2,1) -1-> v3 (1,0)
            //              |            ^                
            //              |_____4______|                
            //              |-----3---------> v4 (2,1) --6-> v5,0)
        }
    }
}