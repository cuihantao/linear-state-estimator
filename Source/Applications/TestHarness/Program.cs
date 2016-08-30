using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SynchrophasorAnalytics.Csv;
using SynchrophasorAnalytics.Measurements;
using SynchrophasorAnalytics.Modeling;
using SynchrophasorAnalytics.Networks;
using SynchrophasorAnalytics.Graphs;
using SynchrophasorAnalytics.Testing;
using SynchrophasorAnalytics.Matrices;
using SynchrophasorAnalytics.Calibration;
using SynchrophasorAnalytics.DataConditioning;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Numerics;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            //PhasorMeasurement busAVoltage = new PhasorMeasurement()
            //    {
            //        Type = PhasorType.VoltagePhasor,
            //        BaseKV = new VoltageLevel(1, 230),
            //        Magnitude = 133518.0156,
            //        AngleInDegrees = -2.4644
            //    };
            //PhasorMeasurement busBVoltage = new PhasorMeasurement()
            //{
            //    Type = PhasorType.VoltagePhasor,
            //    BaseKV = new VoltageLevel(1, 230),
            //    Magnitude = 133758.7656,
            //    AngleInDegrees = 2.4317
            //};
            //PhasorMeasurement busCVoltage = new PhasorMeasurement()
            //{
            //    Type = PhasorType.VoltagePhasor,
            //    BaseKV = new VoltageLevel(1, 230),
            //    Magnitude = 133666.7188,
            //    AngleInDegrees = -2.1697
            //};
            //PhasorMeasurement busDVoltage = new PhasorMeasurement()
            //{
            //    Type = PhasorType.VoltagePhasor,
            //    BaseKV = new VoltageLevel(1, 230),
            //    Magnitude = 134102.8125,
            //    AngleInDegrees = 0.0096257
            //};
            //PhasorMeasurement busEVoltage = new PhasorMeasurement()
            //{
            //    Type = PhasorType.VoltagePhasor,
            //    BaseKV = new VoltageLevel(1, 230),
            //    Magnitude = 133088.9688,
            //    AngleInDegrees = -7.2477
            //};
            //PhasorMeasurement busFVoltage = new PhasorMeasurement()
            //{
            //    Type = PhasorType.VoltagePhasor,
            //    BaseKV = new VoltageLevel(1, 230),
            //    Magnitude = 133141.7344,
            //    AngleInDegrees = -6.3372
            //};
            //PhasorMeasurement busGVoltage = new PhasorMeasurement()
            //{
            //    Type = PhasorType.VoltagePhasor,
            //    BaseKV = new VoltageLevel(1, 230),
            //    Magnitude = 133346.1094,
            //    AngleInDegrees = -5.8259
            //};
            //PhasorMeasurement busHVoltage = new PhasorMeasurement()
            //{
            //    Type = PhasorType.VoltagePhasor,
            //    BaseKV = new VoltageLevel(1, 230),
            //    Magnitude = 133492.2969,
            //    AngleInDegrees = -4.6002
            //};
            /////
            /////
            ///// Current
            //PhasorMeasurement busBtoBusAFlow = new PhasorMeasurement()
            //    {
            //        Type = PhasorType.CurrentPhasor,
            //        BaseKV = new VoltageLevel(1, 230)
            //    };
            //PhasorMeasurement busBtoBusCFlow = new PhasorMeasurement()
            //{
            //    Type = PhasorType.CurrentPhasor,
            //    BaseKV = new VoltageLevel(1, 230)
            //};
            //PhasorMeasurement busDtoBusCFlow = new PhasorMeasurement()
            //{
            //    Type = PhasorType.CurrentPhasor,
            //    BaseKV = new VoltageLevel(1, 230)
            //};
            //PhasorMeasurement busDtoBusFFlow = new PhasorMeasurement()
            //{
            //    Type = PhasorType.CurrentPhasor,
            //    BaseKV = new VoltageLevel(1, 230)
            //};
            //PhasorMeasurement busAtoBusEFlow = new PhasorMeasurement()
            //{
            //    Type = PhasorType.CurrentPhasor,
            //    BaseKV = new VoltageLevel(1, 230)
            //};
            //PhasorMeasurement busFtoBusEFlow = new PhasorMeasurement()
            //{
            //    Type = PhasorType.CurrentPhasor,
            //    BaseKV = new VoltageLevel(1, 230)
            //};
            //PhasorMeasurement busAtoBusGFlow = new PhasorMeasurement()
            //{
            //    Type = PhasorType.CurrentPhasor,
            //    BaseKV = new VoltageLevel(1, 230)
            //};
            //PhasorMeasurement busHtoBusGFlow = new PhasorMeasurement()
            //{
            //    Type = PhasorType.CurrentPhasor,
            //    BaseKV = new VoltageLevel(1, 230)
            //};
            //PhasorMeasurement busDtoBusHFlow = new PhasorMeasurement()
            //{
            //    Type = PhasorType.CurrentPhasor,
            //    BaseKV = new VoltageLevel(1, 230)
            //};

            //busBtoBusAFlow.PerUnitComplexPhasor = (busBVoltage.PerUnitComplexPhasor - busAVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.01));
            //busBtoBusCFlow.PerUnitComplexPhasor = (busBVoltage.PerUnitComplexPhasor - busCVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.06));
            //busDtoBusCFlow.PerUnitComplexPhasor = (busDVoltage.PerUnitComplexPhasor - busCVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.06));
            //busDtoBusFFlow.PerUnitComplexPhasor = (busDVoltage.PerUnitComplexPhasor - busFVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.01));
            //busAtoBusEFlow.PerUnitComplexPhasor = (busAVoltage.PerUnitComplexPhasor - busEVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.005));
            //busFtoBusEFlow.PerUnitComplexPhasor = (busFVoltage.PerUnitComplexPhasor - busEVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.005));
            //busAtoBusGFlow.PerUnitComplexPhasor = (busAVoltage.PerUnitComplexPhasor - busGVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.005));
            //busHtoBusGFlow.PerUnitComplexPhasor = (busHVoltage.PerUnitComplexPhasor - busGVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.01));
            //busDtoBusHFlow.PerUnitComplexPhasor = (busDVoltage.PerUnitComplexPhasor - busHVoltage.PerUnitComplexPhasor) / (new Complex(0.0, 0.01));
            

            //Console.WriteLine("BusB.BusA: " + busBtoBusAFlow.Magnitude.ToString() + " " + (busBtoBusAFlow.AngleInDegrees).ToString());
            //Console.WriteLine("BusB.BusC: " + busBtoBusCFlow.Magnitude.ToString() + " " + (busBtoBusCFlow.AngleInDegrees).ToString());
            //Console.WriteLine("BusD.BusC: " + busDtoBusCFlow.Magnitude.ToString() + " " + (busDtoBusCFlow.AngleInDegrees).ToString());
            //Console.WriteLine("BusD.BusF: " + busDtoBusFFlow.Magnitude.ToString() + " " + (busDtoBusFFlow.AngleInDegrees).ToString());
            //Console.WriteLine("BusA.BusE: " + busAtoBusEFlow.Magnitude.ToString() + " " + (busAtoBusEFlow.AngleInDegrees).ToString());
            //Console.WriteLine("BusF.BusE: " + busFtoBusEFlow.Magnitude.ToString() + " " + (busFtoBusEFlow.AngleInDegrees).ToString());
            //Console.WriteLine("BusA.BusG: " + busAtoBusGFlow.Magnitude.ToString() + " " + (busAtoBusGFlow.AngleInDegrees).ToString());
            //Console.WriteLine("BusH.BusG: " + busHtoBusGFlow.Magnitude.ToString() + " " + (busHtoBusGFlow.AngleInDegrees).ToString());
            //Console.WriteLine("BusD.BusH: " + busDtoBusHFlow.Magnitude.ToString() + " " + (busDtoBusHFlow.AngleInDegrees).ToString());


            //Network network = Network.DeserializeFromXml(@"\\psf\Home\Documents\mc2\Linear State Estimation\EPG\LinearStateEstimator.OfflineModule\x86\TransmissionLineGraphTestFull4.xml");
            //Network network = Network.DeserializeFromXml(@"\\psf\Home\Documents\mc2\Projects\EPG - WECC SDVCA\Data\July 20, 2014 Test Cases\shunt_test_model.xml");

            //RawMeasurements rawMeasurements = RawMeasurements.DeserializeFromXml(@"\\psf\Home\Documents\mc2\Projects\EPG - WECC SDVCA\Data\July 20, 2014 Test Cases\ShuntSeriesTestCase151.xml");
            //network.Initialize();

            //network.Model.InputKeyValuePairs.Clear();

            //for (int i = 0; i < rawMeasurements.Items.Count(); i++)
            //{
            //    network.Model.InputKeyValuePairs.Add(rawMeasurements.Items[i].Key, Convert.ToDouble(rawMeasurements.Items[i].Value));
            //}

            //network.Model.OnNewMeasurements();

            //Console.WriteLine(network.Model.ComponentList());

            //Dictionary<string, double> receivedMeasurements = network.Model.GetReceivedMeasurements();

            //foreach (KeyValuePair<string, double> keyValuePair in receivedMeasurements)
            //{
            //    Console.WriteLine(keyValuePair.Key + " " + keyValuePair.Value.ToString());
            //}

            //Console.WriteLine(rawMeasurements.Items.Count().ToString());
            //Console.WriteLine(receivedMeasurements.Count.ToString());
            //Console.WriteLine();

            //network.RunNetworkReconstructionCheck();

            //network.Model.DetermineActiveCurrentFlows();
            //network.Model.DetermineActiveCurrentInjections();

            //Console.WriteLine(network.Model.MeasurementInclusionStatusList());

            //network.Model.ResolveToObservedBusses();

            //Console.WriteLine(network.Model.ObservedBusses.Count);

            //foreach (Substation substation in network.Model.Substations)
            //{
            //    Console.WriteLine(substation.Graph.AdjacencyList.ToString());
            //}

            //network.Model.ResolveToSingleFlowBranches();

            //foreach (TransmissionLine transmissionLine in network.Model.TransmissionLines)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine(transmissionLine.Name);
            //    Console.WriteLine("Has at least one flow path: " + transmissionLine.Graph.HasAtLeastOneFlowPath.ToString());
            //    Console.WriteLine(transmissionLine.Graph.DirectlyConnectedAdjacencyList.ToString());
            //    Console.WriteLine(transmissionLine.Graph.SeriesImpedanceConnectedAdjacencyList.ToString());
            //    Console.WriteLine(transmissionLine.Graph.RootNode.ToSubtreeString());
            //    List<SeriesBranchBase> seriesBranches = transmissionLine.Graph.SingleFlowPathBranches;
            //    foreach (SeriesBranchBase seriesBranch in seriesBranches)
            //    {
            //        seriesBranch.ToVerboseString();
            //    }
            //}

            //network.ComputeSystemState();
            //network.Model.ComputeEstimatedCurrentFlows();
            //network.Model.ComputeEstimatedCurrentInjections();

            //TransmissionLine transmissionLine = network.Model.Companies[0].Divisions[0].TransmissionLines[0];

            //foreach (Switch bypassSwitch in transmissionLine.Switches)
            //{
            //    bypassSwitch.IsInDefaultMode = false;
            //    bypassSwitch.ActualState = SwitchingDeviceActualState.Closed;
            //    if (bypassSwitch.InternalID == 1) { bypassSwitch.ActualState = SwitchingDeviceActualState.Closed; }
            //    Console.WriteLine(String.Format("ID:{0} Normally:{1} Actually:{2}", bypassSwitch.InternalID, bypassSwitch.NormalState, bypassSwitch.ActualState));
            //}

            //TransmissionLineGraph graph = new TransmissionLineGraph(transmissionLine);
            //graph.InitializeAdjacencyLists();
            //Console.WriteLine(graph.DireclyConnectedAdjacencyList.ToString());
            //graph.ResolveConnectedAdjacencies();
            //Console.WriteLine(graph.DireclyConnectedAdjacencyList.ToString());
            //Console.WriteLine(graph.SeriesImpedanceConnectedAdjacencyList.ToString());
            //if (graph.HasAtLeastOneFlowPath)
            //{
            //    Console.WriteLine(graph.SeriesImpedanceConnectedAdjacencyList.ToString());
            //    graph.InitializeTree();
            //    Console.WriteLine(graph.RootNode.ToSubtreeString());
            //    Console.WriteLine("Number of series branches: " + graph.SingleFlowPathBranches.Count);
            //    Console.WriteLine(graph.ResolveToSingleSeriesBranch().RawImpedanceParameters.ToString());
            //}
            //else
            //{
            //    Console.WriteLine("Graph does not have at least one flow path -> tree would be invalid!");
            //}
            //SequencedMeasurementSnapshotPathSet sequencedMeasurementSnapshotPathSet = new SequencedMeasurementSnapshotPathSet();
            //sequencedMeasurementSnapshotPathSet.MeasurementSnapshotPaths.Add(new MeasurementSnapshotPath("value"));

            //sequencedMeasurementSnapshotPathSet.SerializeToXml("\\\\psf\\Home\\Documents\\mc2\\Projects\\EPG - WECC SDVCA\\Data\\TestCases.xml");
            CsvFileWithHeader coFile = new CsvFileWithHeader("U:\\Documents\\Projects\\02 Linear State Estimator\\EMS to LSE\\lse_co.out");
            CsvFileWithHeader dvFile = new CsvFileWithHeader("U:\\Documents\\Projects\\02 Linear State Estimator\\EMS to LSE\\lse_dv.out");
            CsvFileWithHeader stFile = new CsvFileWithHeader("U:\\Documents\\Projects\\02 Linear State Estimator\\EMS to LSE\\lse_st.out");
            CsvFileWithHeader ndFile = new CsvFileWithHeader("U:\\Documents\\Projects\\02 Linear State Estimator\\EMS to LSE\\lse_nd.out");

            List<double> nodeBaseKvs = new List<double>();
            NetworkModel networkModel = new NetworkModel();

            
            foreach (Dictionary<string, string> node in ndFile.StructuredData)
            {
                if (!nodeBaseKvs.Contains(Convert.ToDouble(node["id_kv"].TrimEnd('\'').TrimStart('\''))))
                {
                    nodeBaseKvs.Add(Convert.ToDouble(node["id_kv"].TrimEnd('\'').TrimStart('\'')));
                }
            }
                        nodeBaseKvs.Sort();
            for (int i = 0; i < nodeBaseKvs.Count; i++)
            {
                networkModel.VoltageLevels.Add(new VoltageLevel(i + 1, nodeBaseKvs[i]));
            }

            foreach (Dictionary<string, string> row in coFile.StructuredData)
            {
                Company company = new Company()
                {
                    InternalID = Convert.ToInt32(row["%SUBSCRIPT"]),
                    Number = Convert.ToInt32(row["%SUBSCRIPT"]),
                    Name = row["id_area"],
                    Acronym = row["id_area"],
                };
                networkModel.Companies.Add(company);

                foreach (Dictionary<string, string> divisionRecord in dvFile.StructuredData)
                {
                    if (company.Number == Convert.ToInt32(divisionRecord["i$area_dv"]))
                    {
                        Division division = new Division()
                        {
                            InternalID = Convert.ToInt32(divisionRecord["%SUBSCRIPT"]),
                            Number = Convert.ToInt32(divisionRecord["%SUBSCRIPT"]),
                            Name = divisionRecord["id_dv"],
                            Acronym = divisionRecord["id_dv"],
                        };
                        company.Divisions.Add(division);

                        foreach (Dictionary<string, string> substationRecord in stFile.StructuredData)
                        {
                            if (division.Name == substationRecord["id_dv"])
                            {
                                Substation substation = new Substation()
                                {
                                    InternalID = Convert.ToInt32(substationRecord["%SUBSCRIPT"]),
                                    Number = Convert.ToInt32(substationRecord["%SUBSCRIPT"]),
                                    Acronym = substationRecord["id_st"],
                                    Name = substationRecord["id_st"]
                                };
                                division.Substations.Add(substation);

                                foreach (Dictionary<string, string> nodeRecord in ndFile.StructuredData)
                                {
                                    if (substation.Name == nodeRecord["id_st"])
                                    {
                                        double baseKvValue = Convert.ToDouble(nodeRecord["id_kv"].TrimEnd('\'').TrimStart('\''));
                                        VoltageLevel baseKv = new VoltageLevel(1, 999);
                                        foreach (VoltageLevel voltageLevel in networkModel.VoltageLevels)
                                        {
                                            if (voltageLevel.Value == baseKv.Value)
                                            {
                                                baseKv = voltageLevel;
                                            }
                                        }
                                        Node node = new Node()
                                        {
                                            InternalID = Convert.ToInt32(nodeRecord["%SUBSCRIPT"]),
                                            Number = Convert.ToInt32(nodeRecord["%SUBSCRIPT"]),
                                            Acronym = nodeRecord["id_nd"],
                                            Name = nodeRecord["id_st"] + "_" + nodeRecord["id_nd"],
                                            Description = nodeRecord["id_st"] + " " + nodeRecord["id_kv"] + "kV Node " + nodeRecord["id_nd"],
                                            BaseKV = baseKv
                                        };
                                        
                                        VoltagePhasorGroup voltage = new VoltagePhasorGroup()
                                        {
                                            InternalID = node.InternalID,
                                            Number = node.Number,
                                            Acronym = "V " + node.InternalID.ToString(),
                                            Name = "Phasor Name",
                                            Description = "Voltage Phasor Group Description",
                                            IsEnabled = true,
                                            UseStatusFlagForRemovingMeasurements = true,
                                            MeasuredNode = node
                                        };

                                        voltage.ZeroSequence.Measurement.BaseKV = node.BaseKV;
                                        voltage.ZeroSequence.Estimate.BaseKV = node.BaseKV;
                                        voltage.NegativeSequence.Measurement.BaseKV = node.BaseKV;
                                        voltage.NegativeSequence.Estimate.BaseKV = node.BaseKV;
                                        voltage.PositiveSequence.Measurement.BaseKV = node.BaseKV;
                                        voltage.PositiveSequence.Estimate.BaseKV = node.BaseKV;
                                        voltage.PhaseA.Measurement.BaseKV = node.BaseKV;
                                        voltage.PhaseA.Estimate.BaseKV = node.BaseKV;
                                        voltage.PhaseB.Measurement.BaseKV = node.BaseKV;
                                        voltage.PhaseB.Estimate.BaseKV = node.BaseKV;
                                        voltage.PhaseC.Measurement.BaseKV = node.BaseKV;
                                        voltage.PhaseC.Estimate.BaseKV = node.BaseKV;

                                        node.Voltage = voltage;

                                        substation.Nodes.Add(node);
                                    }
                                }
                            }
                        }
                    }

                    
                }
            }






            Network network = new Network(networkModel);
            network.SerializeToXml("U:\\ems.xml");

            Console.ReadLine();
        }
    }
}
