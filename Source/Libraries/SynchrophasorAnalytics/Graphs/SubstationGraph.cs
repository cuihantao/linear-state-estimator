﻿//******************************************************************************************************
//  SubstationGraph.cs
//
//  Copyright © 2013, Kevin D. Jones.  All Rights Reserved.
//
//  This file is licensed to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/01/2013 - Kevin D. Jones
//       Generated original version of source code.
//  07/02/2013 - Kevin D. Jones
//       Fixed error in casting switching devices when resolving observed busses.
//  06/14/2014 - Kevin D. Jones
//       Updated XML inline documentation.
//
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SynchrophasorAnalytics.Modeling;

namespace SynchrophasorAnalytics.Graphs
{
    /// <summary>
    /// A class which represents a graph representation of the network elements in a <see cref="LinearStateEstimator.Modeling.Substation"/>.
    /// </summary>
    public class SubstationGraph
    {
        #region [ Private Members ]

        private int m_internalID;
        private List<Node> m_vertexSet;
        private List<SwitchingDeviceBase> m_edgeSet;
        private VertexAdjacencyList m_adjacencyList;
        private List<ObservedBus> m_observedBuses;

        #endregion 

        #region [ Properties ]

        /// <summary>
        /// The set of <see cref="LinearStateEstimator.Modeling.Node"/> vertices.
        /// </summary>
        public List<Node> VertexSet
        {
            get
            {
                return m_vertexSet;
            }
            set
            {
                m_vertexSet = value;
            }
        }

        /// <summary>
        /// The set of <see cref="LinearStateEstimator.Modeling.SwitchingDeviceBase"/> edges.
        /// </summary>
        public List<SwitchingDeviceBase> EdgeSet
        {
            get
            {
                return m_edgeSet;
            }
            set
            {
                m_edgeSet = value;
            }
        }

        /// <summary>
        /// The adjacency list representation of the vertices and edges.
        /// </summary>
        public VertexAdjacencyList AdjacencyList
        {
            get
            {
                return m_adjacencyList;
            }
            set
            {
                m_adjacencyList = value;
            }
        }

        public List<ObservedBus> ObservedBuses
        {
            get
            {
                return m_observedBuses;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// The designated constructor for the <see cref="LinearStateEstimator.Graphs.SubstationGraph"/> class. Requires a reference to a <see cref="LinearStateEstimator.Modeling.Substation"/> of interest.
        /// </summary>
        /// <param name="substation">The <see cref="LinearStateEstimator.Modeling.Substation"/> desired to represent as a graph.</param>
        public SubstationGraph(Substation substation)
        {
            m_internalID = substation.InternalID;
            BuildVertexSet(substation);
            BuildeEdgeSet(substation);
            InitializeAdjacencyList();
            m_observedBuses = new List<ObservedBus>();
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// A method which resolves vertices connected through logical devices such as circuit breakers and switches which are energized.
        /// </summary>
        public void ResolveDirectlyConnectedAdjacencies()
        {
            while (ResolvingDirectlyConnectedAdjacencies()) { }
        }

        /// <summary>
        /// Intneded to function as a recursive function for use with <see cref="LinearStateEstimator.Graphs.SubstationGraph.ResolveDirectlyConnectedAdjacencies"/> which calls this method as the parameter of a while loop. This method will be called over and over again until all of the adjacencies which have direct electrical connections are resolved into clusters of directly connected nodes.
        /// </summary>
        /// <returns></returns>
        public bool ResolvingDirectlyConnectedAdjacencies()
        {
            foreach (VertexAdjacencyRow row in m_adjacencyList.Rows)
            {
                foreach (VertexCluster adjacency in row.Adjacencies)
                {
                    SwitchingDeviceBase connectingEdge = ConnectingEdgeBetween(row.Header, adjacency);

                    if (connectingEdge is CircuitBreaker)
                    {
                        CircuitBreaker circuitBreaker = (CircuitBreaker)connectingEdge;
                        if (circuitBreaker.IsClosed)
                        {
                            ConnectionEstablished(row.Header, adjacency);
                            return true;
                        }
                    }
                    else if (connectingEdge is Switch)
                    {
                        Switch circuitSwitch = (Switch)connectingEdge;
                        if (circuitSwitch.IsClosed)
                        {
                            ConnectionEstablished(row.Header, adjacency);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// A method used to take the groups of directly connected nodes and convert them into <see cref="LinearStateEstimator.Modeling.ObservedBus"/> objects.
        /// </summary>
        /// <returns></returns>
        public List<ObservedBus> ResolveToObservedBuses()
        {
            m_observedBuses.Clear();
            List<ObservedBus> observedBuses = new List<ObservedBus>();
            foreach (VertexAdjacencyRow vertexAdjacencyRow in m_adjacencyList.Rows)
            {
                List<Node> nodeCluster = new List<Node>();
                foreach (int nodeInternalID in vertexAdjacencyRow.Header.Vertices)
                {
                    nodeCluster.Add(m_vertexSet.Find(x => x.InternalID == nodeInternalID));
                }
                observedBuses.Add(new ObservedBus(m_internalID, nodeCluster));
                m_observedBuses.Add(new ObservedBus(m_internalID, nodeCluster));
            }
            return observedBuses;
        }

        /// <summary>
        /// Mergers two vertices together once a direct connection has been determined.
        /// </summary>
        /// <param name="fromVertexCluster">The vertex on the from side of the edge.</param>
        /// <param name="toVertexCluster">The vertex on the to side of the edge.</param>
        public void ConnectionEstablished(VertexCluster fromVertexCluster, VertexCluster toVertexCluster)
        {
            List<int> fromVertices = new List<int>();
            List<int> toVertices = new List<int>();

            foreach (int vertex in fromVertexCluster.Vertices)
            {
                fromVertices.Add(vertex);
            }

            foreach (int vertex in toVertexCluster.Vertices)
            {
                toVertices.Add(vertex);
            }

            VertexCluster fromCluster = new VertexCluster(fromVertices);
            VertexCluster toCluster = new VertexCluster(toVertices);

            VertexAdjacencyRow source = m_adjacencyList.RowWithHeader(fromVertexCluster);
            VertexAdjacencyRow target = m_adjacencyList.RowWithHeader(toVertexCluster);

            // Merge the two rows into one
            source.MergeWith(target);

            // Remove the old from the list
            m_adjacencyList.RemoveRow(target);

            // Update the vertices in the rest of the table
            foreach (VertexAdjacencyRow row in m_adjacencyList.Rows)
            {
                foreach (VertexCluster vertexCluster in row.Adjacencies)
                {
                    if (vertexCluster.Equals(fromCluster) || vertexCluster.Equals(toCluster))
                    {
                        vertexCluster.Vertices = source.Header.Vertices;
                    }
                }
                row.RemoveDuplicateVertexClusters();
            }
        }

        /// <summary>
        /// Finds and returns the edge which connects to vertices.
        /// </summary>
        /// <param name="fromVertexCluster">The vertex for the from side of the potential edge.</param>
        /// <param name="toVertexCluster">The vertex for the to side of the potential edge.</param>
        /// <returns></returns>
        public SwitchingDeviceBase ConnectingEdgeBetween(VertexCluster fromVertexCluster, VertexCluster toVertexCluster)
        {
            foreach (SwitchingDeviceBase switchingDevice in m_edgeSet)
            {
                foreach (int fromVertex in fromVertexCluster.Vertices)
                {
                    foreach (int toVertex in toVertexCluster.Vertices)
                    {
                        if (switchingDevice.FromNode.InternalID == fromVertex && switchingDevice.ToNode.InternalID == toVertex)
                        {
                            return switchingDevice;
                        }
                        else if (switchingDevice.FromNode.InternalID == toVertex && switchingDevice.ToNode.InternalID == fromVertex)
                        {
                            return switchingDevice;
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region [ Private Methods ]

        private void BuildVertexSet(Substation substation)
        {
            m_vertexSet = new List<Node>();

            // Build Vertex Set
            foreach (Node node in substation.Nodes)
            {
                m_vertexSet.Add(node);
            }
        }

        private void BuildeEdgeSet(Substation substation)
        {
            m_edgeSet = new List<SwitchingDeviceBase>();

            // Build Edge Set
            foreach (CircuitBreaker circuitBreaker in substation.CircuitBreakers)
            {
                m_edgeSet.Add(circuitBreaker);
            }
            foreach (Switch circuitSwitch in substation.Switches)
            {
                m_edgeSet.Add(circuitSwitch);
            }
        }

        private void InitializeAdjacencyList()
        {
            m_adjacencyList = new VertexAdjacencyList();

            List<VertexCluster> vertexClusters = new List<VertexCluster>();
            List<VertexAdjacencyRow> adjacencyRows = new List<VertexAdjacencyRow>();

            foreach (Node node in m_vertexSet)
            {
                VertexCluster vertexCluster = new VertexCluster(node.InternalID);
                List<VertexCluster> vertexAdjacencies = new List<VertexCluster>();

                foreach (SwitchingDeviceBase switchingDevice in m_edgeSet)
                {
                    if (node.InternalID == switchingDevice.FromNode.InternalID)
                    {
                        vertexAdjacencies.Add(new VertexCluster(switchingDevice.ToNode.InternalID));
                    }
                    else if (node.InternalID == switchingDevice.ToNode.InternalID)
                    {
                        vertexAdjacencies.Add(new VertexCluster(switchingDevice.FromNode.InternalID));
                    }
                }

                m_adjacencyList.Rows.Add(new VertexAdjacencyRow(vertexCluster, vertexAdjacencies));
            }
        }

        #endregion
    }
}
