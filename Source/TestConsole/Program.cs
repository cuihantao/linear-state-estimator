using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using SynchrophasorAnalytics.Psse;
using SynchrophasorAnalytics.Modeling;
using SynchrophasorAnalytics.Measurements;
using SynchrophasorAnalytics.Networks;
using SynchrophasorAnalytics.Hdb;
using SynchrophasorAnalytics.Testing;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            #region [ Commented Out Code ]

            //string columnMappingsFile = @"C:\Users\kevin\OneDrive\Documents\VT Data\Models\column-mappings.txt";
            //string csvFile = @"C:\Users\kevin\OneDrive\Documents\VT Data\Dynamic File\118BusTestRun.csv";

            //RawMeasurements.MakeSampleFilesFromCsv(csvFile, columnMappingsFile, true);
            string m_configurationPathName = @"C:\Users\kevin\OneDrive\Documents\VT Data\Models\prune test.xml";
            Network network = Network.DeserializeFromXml(m_configurationPathName);
            network.Initialize();
            network.Model.InPruningMode = true;

            if (network.Model.InPruningMode)
            {
                network.Model.DetermineActiveCurrentFlows();
                network.Model.DetermineActiveCurrentInjections();
                network.Model.ResolveToObservedBuses();
                network.Model.ResolveToSingleFlowBranches();

                List<Company> companiesToPrune = new List<Company>();
                List<Division> divisionsToPrune = new List<Division>();
                List<Substation> substationsToPrune = new List<Substation>();
                List<TransmissionLine> transmissionLinesToPrune = new List<TransmissionLine>();
                
                Console.WriteLine("Searching for companies to prune.");
                // Identify prunable companies
                foreach (Company company in network.Model.Companies)
                {
                    if (!company.RetainWhenPruning)
                    {
                        companiesToPrune.Add(company);
                        Console.WriteLine($"Identified {company.Name} for pruning.");
                    }
                }

                // prune companies
                foreach (Company company in companiesToPrune)
                {
                    network.Model.Companies.Remove(company);
                }

                Console.WriteLine("Searching for divisions to prune.");
                // identify prunable divisions
                foreach (Company company in network.Model.Companies)
                {
                    foreach (Division division in company.Divisions)
                    {
                        if (!division.RetainWhenPruning)
                        {
                            divisionsToPrune.Add(division);
                            Console.WriteLine($"Identified {division.Name} for pruning.");
                        }
                    }
                }

                // prune divisions
                foreach (Company company in network.Model.Companies)
                {
                    foreach (Division division in divisionsToPrune)
                    {
                        if (company.Divisions.Contains(division))
                        {
                            company.Divisions.Remove(division);
                        }
                    }
                }

                Console.WriteLine("Searching for substations to prune.");
                // identify prunable substations
                foreach (Company company in network.Model.Companies)
                {
                    foreach (Division division in company.Divisions)
                    {
                        foreach (Substation substation in division.Substations)
                        {
                            if (!substation.RetainWhenPruning)
                            {
                                substationsToPrune.Add(substation);
                                Console.WriteLine($"Identified {substation.Name} for pruning.");
                            }
                        }
                    }
                }

                // prune substations
                foreach (Company company in network.Model.Companies)
                {
                    foreach (Division division in company.Divisions)
                    {
                        foreach (Substation substation in substationsToPrune)
                        {
                            if (division.Substations.Contains(substation))
                            {
                                division.Substations.Remove(substation);
                            }
                        }
                    }
                }

                Console.WriteLine("Searching for transmissionLines to prune.");
                // identify prunable transmission lines
                foreach (Company company in network.Model.Companies)
                {
                    foreach (Division division in company.Divisions)
                    {
                        foreach (TransmissionLine transmissionLine in division.TransmissionLines)
                        {
                            if (!transmissionLine.RetainWhenPruning)
                            {
                                transmissionLinesToPrune.Add(transmissionLine);
                            }
                        }
                    }
                }

                // prune transmission lines
                foreach (Company company in network.Model.Companies)
                {
                    foreach (Division division in company.Divisions)
                    {
                        foreach (TransmissionLine transmissionLine in transmissionLinesToPrune)
                        {
                            if (division.TransmissionLines.Contains(transmissionLine))
                            {
                                division.TransmissionLines.Remove(transmissionLine);
                            }
                        }
                    }
                }

                network.Initialize();

                Console.WriteLine(network.Model.ComponentList());
            }

            //bool keyify = true;

            //string path = @"D:\opspln\data\exports\2017_02_05_23_50";

            //ModelFiles hdbModelFiles = new ModelFiles()
            //{
            //    AreaFile = $@"{path}\area.out",
            //    CircuitBreakerFile = $@"{path}\cb.out",
            //    CompanyFile = $@"{path}\co.out",
            //    DivisionFile = $@"{path}\dv.out",
            //    LineSegmentFile = $@"{path}\ln.out",
            //    NodeFile = $@"{path}\nd.out",
            //    ShuntFile = $@"{path}\cp.out",
            //    StationFile = $@"{path}\st.out",
            //    TransformerFile = $@"{path}\xf.out",
            //    ParentTransformerFile = $@"{path}\xfmr.out",
            //    TransformerTapFile = $@"{path}\tapty.out",
            //    TransmissionLineFile = $@"{path}\line.out",
            //};

            //hdbModelFiles.SerializeToXml($@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\modelFiles.xml");

            //Console.WriteLine(hdbModelFiles.ToString());

            //HdbContext context = new HdbContext(hdbModelFiles);

            ////string rawFileName = "IEEE 14 bus Substations v33";
            ////string xmlFileName = "IEEE 14 Bus - Nodal Variety";
            ////Network.ConvertFromPsseRawFile($@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\{xmlFileName}.xml", $@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\{rawFileName}.raw", "33");

            //NetworkModel networkModel = new NetworkModel();
            //List<Company> companies = new List<Company>();
            //List<Division> divisions = new List<Division>();
            //List<Substation> substations = new List<Substation>();
            //List<Node> nodes = new List<Node>();
            //List<VoltageLevel> voltageLevels = new List<VoltageLevel>();
            //List<CircuitBreaker> breakers = new List<CircuitBreaker>();
            //List<Switch> switches = new List<Switch>();
            //List<ShuntCompensator> shunts = new List<ShuntCompensator>();
            //List<LineSegment> lineSegments = new List<LineSegment>();
            //List<TransmissionLine> transmissionLines = new List<TransmissionLine>();
            //List<Transformer> transformers = new List<Transformer>();
            //List<TapConfiguration> taps = new List<TapConfiguration>();
            //List<BreakerStatus> breakerStatuses = new List<BreakerStatus>();

            //#region [ Modeling Companies ]

            //for (int i = 0; i < context.Companies.Count; i++)
            //{
            //    companies.Add(new Company()
            //    {
            //        InternalID = context.Companies[i].Number,
            //        Number = context.Companies[i].Number,
            //        Acronym = context.Companies[i].Name.ToUpper(),
            //        Name = context.Companies[i].Name,
            //        Description = context.Companies[i].ToString(),
            //    });
            //}

            //#endregion

            //#region [ Modeling Divisions ]

            //for (int i = 0; i < context.Divisions.Count; i++)
            //{
            //    Division division = new Division()
            //    {
            //        InternalID = context.Divisions[i].Number,
            //        Number = context.Divisions[i].Number,
            //        Acronym = context.Divisions[i].Name.ToUpper(),
            //        Name = context.Divisions[i].Name,
            //        Description = context.Divisions[i].Name,
            //    };

            //    divisions.Add(division);
            //}

            //#endregion

            //#region [ Modeling Substations ]

            //for (int i = 0; i < context.Stations.Count; i++)
            //{
            //    substations.Add(new Substation()
            //    {
            //        InternalID = context.Stations[i].Number,
            //        Number = context.Stations[i].Number,
            //        Acronym = context.Stations[i].Name.ToUpper(),
            //        Name = context.Stations[i].Name,
            //        Description = $"{context.Stations[i].Name} Substation"
            //    });
            //}

            //#endregion

            //#region [ Modeling Nodes, Voltage Phasor Groups,  & Voltage Levels ]

            //for (int i = 0; i < context.Nodes.Count; i++)
            //{
            //    Substation parentSubstation = substations.Find(x => x.Name == context.Nodes[i].StationName);
            //    Division parentDivision = divisions.Find(x => x.Name == context.Nodes[i].DivisionName);
            //    Company parentCompany = companies.Find(x => x.Name == context.Nodes[i].CompanyName);

            //    VoltageLevel voltageLevel = voltageLevels.Find(x => x.Value == context.Nodes[i].BaseKv);

            //    if (voltageLevel == null)
            //    {
            //        voltageLevel = new VoltageLevel(voltageLevels.Count + 1, context.Nodes[i].BaseKv);
            //        voltageLevels.Add(voltageLevel);
            //    }

            //    Node node = new Node()
            //    {
            //        InternalID = context.Nodes[i].Number,
            //        Number = context.Nodes[i].Number,
            //        Acronym = context.Nodes[i].Id.ToUpper(),
            //        Name = context.Nodes[i].StationName + "_" + context.Nodes[i].Id,
            //        Description = context.Nodes[i].StationName + "_" + context.Nodes[i].Id,
            //        ParentSubstation = parentSubstation,
            //        BaseKV = voltageLevel
            //    };

            //    VoltagePhasorGroup voltage = new VoltagePhasorGroup()
            //    {
            //        InternalID = node.InternalID,
            //        Number = node.Number,
            //        Acronym = node.Name.ToUpper()+ "-V",
            //        Name = node.Name + " Voltage Phasor Group",
            //        Description = node.Description + " Voltage Phasor Group",
            //        IsEnabled = true,
            //        UseStatusFlagForRemovingMeasurements = true,
            //        MeasuredNode = node,
            //    };

            //    voltage.ZeroSequence.Measurement.BaseKV = node.BaseKV;
            //    voltage.ZeroSequence.Estimate.BaseKV = node.BaseKV;
            //    voltage.NegativeSequence.Measurement.BaseKV = node.BaseKV;
            //    voltage.NegativeSequence.Estimate.BaseKV = node.BaseKV;
            //    voltage.PositiveSequence.Measurement.BaseKV = node.BaseKV;
            //    voltage.PositiveSequence.Estimate.BaseKV = node.BaseKV;
            //    voltage.PhaseA.Measurement.BaseKV = node.BaseKV;
            //    voltage.PhaseA.Estimate.BaseKV = node.BaseKV;
            //    voltage.PhaseB.Measurement.BaseKV = node.BaseKV;
            //    voltage.PhaseB.Estimate.BaseKV = node.BaseKV;
            //    voltage.PhaseC.Measurement.BaseKV = node.BaseKV;
            //    voltage.PhaseC.Estimate.BaseKV = node.BaseKV;

            //    if (keyify)
            //    {
            //        voltage.Keyify(node.Name);
            //    }

            //    node.Voltage = voltage;
            //    nodes.Add(node);

            //    parentSubstation.Nodes.Add(node);
            //    if (!parentDivision.Substations.Contains(parentSubstation))
            //    {
            //        parentDivision.Substations.Add(parentSubstation);
            //    }
            //    if (!parentCompany.Divisions.Contains(parentDivision))
            //    {
            //        parentCompany.Divisions.Add(parentDivision);
            //    }
            //}


            //#endregion

            //#region [ Modeling Circuit Breakers & Switches ]

            //for (int i = 0; i < context.CircuitBreakers.Count; i++)
            //{
            //    string fromNodeName = context.CircuitBreakers[i].StationName + "_" + context.CircuitBreakers[i].FromNodeId;
            //    string toNodeName = context.CircuitBreakers[i].StationName + "_" + context.CircuitBreakers[i].ToNodeId;

            //    Node fromNode = nodes.Find(x => x.Name == fromNodeName);
            //    Node toNode = nodes.Find(x => x.Name == toNodeName);

            //    string measurementKey = "Undefined";

            //    if (keyify)
            //    {
            //        measurementKey = $"{context.CircuitBreakers[i].StationName}.{context.CircuitBreakers[i].Id}.{context.CircuitBreakers[i].Type}";
            //    }

            //    if (context.CircuitBreakers[i].Type == "CB")
            //    {

            //        CircuitBreaker circuitBreaker = new CircuitBreaker()
            //        {
            //            InternalID = context.CircuitBreakers[i].Number,
            //            Number = context.CircuitBreakers[i].Number,
            //            Name = context.CircuitBreakers[i].StationName + "_" + context.CircuitBreakers[i].Id,
            //            Description = context.CircuitBreakers[i].ToString(),
            //            FromNode = fromNode,
            //            ToNode = toNode,
            //            ParentSubstation = fromNode.ParentSubstation,
            //            NormalState = SwitchingDeviceNormalState.Closed,
            //            ActualState = SwitchingDeviceActualState.Closed,
            //            MeasurementKey = measurementKey,
            //        };

            //        if (context.CircuitBreakers[i].IsNormallyOpen == "T")
            //        {
            //            circuitBreaker.NormalState = SwitchingDeviceNormalState.Open;
            //        }

            //        BreakerStatus breakerStatus = new BreakerStatus()
            //        {
            //            InternalID = circuitBreaker.InternalID,
            //            Number = circuitBreaker.Number,
            //            Name = circuitBreaker.Name,
            //            Description = circuitBreaker.Description,
            //            BitPosition = BreakerStatusBit.PSV64,
            //            ParentCircuitBreaker = circuitBreaker,
            //            IsEnabled = false,
            //        };

            //        if (keyify)
            //        {
            //            breakerStatus.Keyify($"{circuitBreaker.Name}");
            //        }

            //        breakerStatuses.Add(breakerStatus);

            //        fromNode.ParentSubstation.CircuitBreakers.Add(circuitBreaker);
            //        breakers.Add(circuitBreaker);
            //    }
            //    else
            //    {
            //        // switch
            //        Switch circuitSwitch = new Switch()
            //        {
            //            InternalID = context.CircuitBreakers[i].Number,
            //            Number = context.CircuitBreakers[i].Number,
            //            Name = context.CircuitBreakers[i].StationName + "_" + context.CircuitBreakers[i].Id,
            //            Description = context.CircuitBreakers[i].ToString(),
            //            FromNode = fromNode,
            //            ToNode = toNode,
            //            ParentSubstation = fromNode.ParentSubstation,
            //            NormalState = SwitchingDeviceNormalState.Closed,
            //            ActualState = SwitchingDeviceActualState.Closed,
            //            MeasurementKey = measurementKey,
            //        };

            //        if (context.CircuitBreakers[i].IsNormallyOpen == "T")
            //        {
            //            circuitSwitch.NormalState = SwitchingDeviceNormalState.Open;
            //        }

            //        fromNode.ParentSubstation.Switches.Add(circuitSwitch);
            //        switches.Add(circuitSwitch);
            //    }
            //}



            //#endregion

            //#region [ Modeling Shunt Compensators ]

            //for (int i = 0; i < context.Shunts.Count; i++)
            //{
            //    string nodeName = context.Shunts[i].StationName + "_" + context.Shunts[i].NodeId;

            //    Node node = nodes.Find(x => x.Name == nodeName);

            //    ShuntCompensator shunt = new ShuntCompensator()
            //    {
            //        InternalID = context.Shunts[i].Number,
            //        Number = context.Shunts[i].Number,
            //        Name = context.Shunts[i].StationName + "_" + context.Shunts[i].Id,
            //        Description = $"{context.Shunts[i].StationName}_{context.Shunts[i].Id} ({context.Shunts[i].NominalMvar})",
            //        ConnectedNode = node,
            //        NominalMvar = context.Shunts[i].NominalMvar,
            //        ParentSubstation = node.ParentSubstation,
            //        ImpedanceCalculationMethod = ShuntImpedanceCalculationMethod.CalculateFromRating,
            //        RawImpedanceParameters = new Impedance(),
            //    };

            //    shunt.Current = new CurrentInjectionPhasorGroup()
            //    {
            //        InternalID = shunt.InternalID,
            //        Number = shunt.Number,
            //        Acronym = shunt.Name.ToUpper(),
            //        Name = shunt.Name + " Current Injection Phasor Group",
            //        Description = shunt.Name + " Current Injection Phasor Group",
            //        IsEnabled = true,
            //        UseStatusFlagForRemovingMeasurements = true,
            //        MeasuredBranch = shunt,
            //        MeasuredConnectedNode = shunt.ConnectedNode
            //    };

            //    shunt.Current.ZeroSequence.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.ZeroSequence.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.NegativeSequence.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.NegativeSequence.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.PositiveSequence.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.PositiveSequence.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.PhaseA.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.PhaseA.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.PhaseB.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.PhaseB.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.PhaseC.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
            //    shunt.Current.PhaseC.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;

            //    if (keyify)
            //    {
            //        shunt.Current.Keyify($"{shunt.Name}");
            //    }
            //    node.ParentSubstation.Shunts.Add(shunt);
            //    shunts.Add(shunt);
            //}


            //#endregion

            //#region [ Modeling Line Segments, Transmission Lines, & Current Flow Phasor Groups ]

            //int currentFlowIntegerIndex = 1;

            //for (int i = 0; i < context.LineSegments.Count; i++)
            //{
            //    string fromNodeName = $"{context.LineSegments[i].FromStationName}_{context.LineSegments[i].FromNodeId}";
            //    string toNodeName = $"{context.LineSegments[i].ToStationName}_{context.LineSegments[i].ToNodeId}";

            //    Node fromNode = nodes.Find(x => x.Name == fromNodeName);
            //    Node toNode = nodes.Find(x => x.Name == toNodeName);

            //    Division parentDivision = divisions.Find(x => x.Name == context.LineSegments[i].DivisionName);

            //    Impedance impedance = new Impedance()
            //    {
            //        R1 = context.LineSegments[i].Resistance,
            //        X1 = context.LineSegments[i].Reactance,
            //        B1 = context.LineSegments[i].LineCharging,
            //        R3 = context.LineSegments[i].Resistance,
            //        X3 = context.LineSegments[i].Reactance,
            //        B3 = context.LineSegments[i].LineCharging,
            //        R6 = context.LineSegments[i].Resistance,
            //        X6 = context.LineSegments[i].Reactance,
            //        B6 = context.LineSegments[i].LineCharging,
            //    };

            //    LineSegment lineSegment = new LineSegment()
            //    {
            //        InternalID = context.LineSegments[i].Number,
            //        Number = context.LineSegments[i].Number,
            //        Acronym = context.LineSegments[i].TransmissionLineId,
            //        Name = context.LineSegments[i].TransmissionLineId,
            //        Description = context.LineSegments[i].TransmissionLineId,
            //        FromNode = fromNode,
            //        ToNode = toNode,
            //        RawImpedanceParameters = impedance,
            //    };

            //    var line = context.TransmissionLines.Find(x => x.Id == context.LineSegments[i].TransmissionLineId);

            //    TransmissionLine transmissionLine = new TransmissionLine()
            //    {
            //        InternalID = line.Number,
            //        Number = line.Number,
            //        Name = lineSegment.Name,
            //        Acronym = lineSegment.Acronym,
            //        Description = lineSegment.Description,
            //        FromNode = fromNode,
            //        ToNode = toNode,
            //        FromSubstation = fromNode.ParentSubstation,
            //        ToSubstation = toNode.ParentSubstation,
            //        ParentDivision = parentDivision,
            //    };

            //    transmissionLine.FromNode.ParentTransmissionLine = transmissionLine;
            //    transmissionLine.ToNode.ParentTransmissionLine = transmissionLine;

            //    transmissionLine.FromSubstationCurrent = new CurrentFlowPhasorGroup()
            //    {
            //        InternalID = currentFlowIntegerIndex,
            //        Number = currentFlowIntegerIndex,
            //        Acronym = fromNode.Acronym + "-I",
            //        Name = fromNode.Name + " Current Flow Phasor Group",
            //        Description = fromNode.Description + " Current Flow Phasor Group",
            //        IsEnabled = true,
            //        UseStatusFlagForRemovingMeasurements = true,
            //        MeasuredBranch = transmissionLine,
            //        MeasuredFromNode = transmissionLine.FromNode,
            //        MeasuredToNode = transmissionLine.ToNode
            //    };

            //    transmissionLine.FromSubstationCurrent.ZeroSequence.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.ZeroSequence.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.NegativeSequence.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.NegativeSequence.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.PositiveSequence.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.PositiveSequence.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.PhaseA.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.PhaseA.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.PhaseB.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.PhaseB.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.PhaseC.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
            //    transmissionLine.FromSubstationCurrent.PhaseC.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;

            //    transmissionLine.ToSubstationCurrent = new CurrentFlowPhasorGroup()
            //    {
            //        InternalID = currentFlowIntegerIndex + 1,
            //        Number = currentFlowIntegerIndex + 1,
            //        Acronym = toNode.Acronym + "-I",
            //        Name = toNode.Name + " Current Flow Phasor Group",
            //        Description = toNode.Description + " Current Flow Phasor Group",
            //        IsEnabled = true,
            //        UseStatusFlagForRemovingMeasurements = true,
            //        MeasuredBranch = transmissionLine,
            //        MeasuredFromNode = transmissionLine.ToNode,
            //        MeasuredToNode = transmissionLine.FromNode
            //    };

            //    transmissionLine.ToSubstationCurrent.ZeroSequence.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.ZeroSequence.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.NegativeSequence.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.NegativeSequence.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.PositiveSequence.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.PositiveSequence.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.PhaseA.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.PhaseA.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.PhaseB.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.PhaseB.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.PhaseC.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
            //    transmissionLine.ToSubstationCurrent.PhaseC.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;

            //    if (keyify)
            //    {
            //        transmissionLine.FromSubstationCurrent.Keyify($"{fromNode.Name}");
            //        transmissionLine.ToSubstationCurrent.Keyify($"{toNode.Name}");
            //    }

            //    lineSegment.ParentTransmissionLine = transmissionLine;

            //    transmissionLine.LineSegments.Add(lineSegment);
            //    parentDivision.TransmissionLines.Add(transmissionLine);

            //    transmissionLines.Add(transmissionLine);
            //    lineSegments.Add(lineSegment);

            //    currentFlowIntegerIndex += 2;
            //}

            //#endregion

            //#region [ Modeling Transformer Taps ]

            //for (int i = 0; i < context.TransformerTaps.Count; i++)
            //{
            //    int minPosition = context.TransformerTaps[i].MinimumPosition;
            //    int maxPosition = context.TransformerTaps[i].MaximumPosition;
            //    int nominalPosition = context.TransformerTaps[i].NominalPosition;
            //    double stepSize = context.TransformerTaps[i].StepSize;

            //    TapConfiguration tap = new TapConfiguration()
            //    {
            //        InternalID = context.TransformerTaps[i].Number,
            //        Number = context.TransformerTaps[i].Number,
            //        Acronym = context.TransformerTaps[i].Id.ToUpper(),
            //        Name = context.TransformerTaps[i].Id,
            //        Description = context.TransformerTaps[i].Id,
            //        PositionLowerBounds = minPosition,
            //        PositionUpperBounds = maxPosition,
            //        PositionNominal = nominalPosition,
            //        LowerBounds = 1.0 + (minPosition - nominalPosition) * stepSize,
            //        UpperBounds = 1.0 + (maxPosition - nominalPosition) * stepSize,
            //    };

            //    taps.Add(tap);
            //}

            //#endregion

            //#region [ Modeling Transformers ] 

            //for (int i = 0; i < context.Transformers.Count; i++)
            //{
            //    string fromNodeName = $"{context.Transformers[i].StationName}_{context.Transformers[i].FromNodeId}";
            //    string toNodeName = $"{context.Transformers[i].StationName}_{context.Transformers[i].ToNodeId}";

            //    Node fromNode = nodes.Find(x => x.Name == fromNodeName);
            //    Node toNode = nodes.Find(x => x.Name == toNodeName);

            //    Impedance impedance = new Impedance()
            //    {
            //        R1 = context.Transformers[i].Resistance,
            //        X1 = context.Transformers[i].Reactance,
            //        G1 = context.Transformers[i].MagnetizingConductance,
            //        B1 = context.Transformers[i].MagnetizingSusceptance,
            //        R3 = context.Transformers[i].Resistance,
            //        X3 = context.Transformers[i].Reactance,
            //        G3 = context.Transformers[i].MagnetizingConductance,
            //        B3 = context.Transformers[i].MagnetizingSusceptance,
            //        R6 = context.Transformers[i].Resistance,
            //        X6 = context.Transformers[i].Reactance,
            //        G6 = context.Transformers[i].MagnetizingConductance,
            //        B6 = context.Transformers[i].MagnetizingSusceptance,
            //    };


            //    Transformer transformer = new Transformer()
            //    {
            //        InternalID = context.Transformers[i].Number,
            //        Number = context.Transformers[i].Number,
            //        Name = $"{context.Transformers[i].StationName}_{context.Transformers[i].Parent}",
            //        Description = $"{context.Transformers[i].StationName}_{context.Transformers[i].Parent}",
            //        FromNode = fromNode,
            //        ToNode = toNode,
            //        ParentSubstation = fromNode.ParentSubstation,
            //        RawImpedanceParameters = impedance,
            //        UltcIsEnabled = false,
            //        FromNodeConnectionType = TransformerConnectionType.Wye,
            //        ToNodeConnectionType = TransformerConnectionType.Wye,                    
            //    };

            //    TapConfiguration tap = taps.Find(x => x.Name == context.Transformers[i].FromNodeTap);

            //    if (tap != null)
            //    {
            //        transformer.Tap = tap;
            //    }

            //    transformer.FromNodeCurrent = new CurrentFlowPhasorGroup()
            //    {
            //        InternalID = currentFlowIntegerIndex,
            //        Number = currentFlowIntegerIndex,
            //        Acronym = fromNode.Acronym + "-I",
            //        Name = fromNode.Name + " Current Flow Phasor Group",
            //        Description = fromNode.Description + " Current Flow Phasor Group",
            //        IsEnabled = true,
            //        UseStatusFlagForRemovingMeasurements = true,
            //        MeasuredBranch = transformer,
            //        MeasuredFromNode = transformer.FromNode,
            //        MeasuredToNode = transformer.ToNode
            //    };

            //    transformer.FromNodeCurrent.ZeroSequence.Measurement.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.ZeroSequence.Estimate.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.NegativeSequence.Measurement.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.NegativeSequence.Estimate.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.PositiveSequence.Measurement.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.PositiveSequence.Estimate.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.PhaseA.Measurement.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.PhaseA.Estimate.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.PhaseB.Measurement.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.PhaseB.Estimate.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.PhaseC.Measurement.BaseKV = transformer.FromNode.BaseKV;
            //    transformer.FromNodeCurrent.PhaseC.Estimate.BaseKV = transformer.FromNode.BaseKV;

            //    transformer.ToNodeCurrent = new CurrentFlowPhasorGroup()
            //    {
            //        InternalID = currentFlowIntegerIndex + 1,
            //        Number = currentFlowIntegerIndex + 1,
            //        Acronym = toNode.Acronym + "-I",
            //        Name = toNode.Name + " Current Flow Phasor Group",
            //        Description = toNode.Description + " Current Flow Phasor Group",
            //        IsEnabled = true,
            //        UseStatusFlagForRemovingMeasurements = true,
            //        MeasuredBranch = transformer,
            //        MeasuredFromNode = transformer.ToNode,
            //        MeasuredToNode = transformer.FromNode
            //    };

            //    transformer.ToNodeCurrent.ZeroSequence.Measurement.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.ZeroSequence.Estimate.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.NegativeSequence.Measurement.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.NegativeSequence.Estimate.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.PositiveSequence.Measurement.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.PositiveSequence.Estimate.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.PhaseA.Measurement.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.PhaseA.Estimate.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.PhaseB.Measurement.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.PhaseB.Estimate.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.PhaseC.Measurement.BaseKV = transformer.ToNode.BaseKV;
            //    transformer.ToNodeCurrent.PhaseC.Estimate.BaseKV = transformer.ToNode.BaseKV;

            //    if (keyify)
            //    {
            //        transformer.FromNodeCurrent.Keyify($"{fromNode.Name}");
            //        transformer.ToNodeCurrent.Keyify($"{toNode.Name}");
            //        transformer.Keyify($"{transformer.Name}");
            //    }

            //    currentFlowIntegerIndex += 2;

            //    fromNode.ParentSubstation.Transformers.Add(transformer);
            //    transformers.Add(transformer);
            //}

            //#endregion



            //networkModel.Companies = companies;
            //networkModel.VoltageLevels = voltageLevels;
            //networkModel.TapConfigurations = taps;
            //networkModel.BreakerStatuses = breakerStatuses;


            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //Network network = Network.FromHdbExport($@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\modelFiles.xml", true);
            ////Network network = Network.DeserializeFromXml(@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\hdb_test.xml");
            //Console.WriteLine($"Composed Model: {stopwatch.Elapsed}");
            ////network.Initialize();
            //network.Model.LinkTransmissionLineReferencesToSubstations();
            //Console.WriteLine($"LinkTransmissionLineReferencesToSubstations: {stopwatch.Elapsed}");
            //network.Model.LinkHierarchicalReferences();
            //Console.WriteLine($"LinkHierarchicalReferences: {stopwatch.Elapsed}");
            //network.Model.LinkVoltageLevelReferences();
            //Console.WriteLine($"LinkVoltageLevelReferences: {stopwatch.Elapsed}");
            //network.Model.LinkTapConfigurationReferences();
            //Console.WriteLine($"LinkTapConfigurationReferences: {stopwatch.Elapsed}");
            //network.Model.ListNetworkComponents();
            //Console.WriteLine($"ListNetworkComponents: {stopwatch.Elapsed}");
            //network.Model.ListNetworkMeasurements();
            //Console.WriteLine($"ListNetworkMeasurements: {stopwatch.Elapsed}");
            //network.Model.LinkBreakerStatusToCircuitBreakers();
            //Console.WriteLine($"LinkBreakerStatusToCircuitBreakers: {stopwatch.Elapsed}");
            //network.Model.LinkStatusWordsToPhasorGroups();
            //Console.WriteLine($"LinkStatusWordsToPhasorGroups: {stopwatch.Elapsed}");
            //network.Model.LinkVoltageLevelsToPhasorGroups();
            //Console.WriteLine($"LinkVoltageLevelsToPhasorGroups: {stopwatch.Elapsed}");
            //network.Model.InitializeComplexPowerCalculations();
            //Console.WriteLine($"InitializeComplexPowerCalculations: {stopwatch.Elapsed}");
            //Console.WriteLine("Initialized Model");
            //network.SerializeToXml($@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\hdb_test_3.xml");

            //Console.WriteLine($"Saved the file successfully: {stopwatch.Elapsed}");
            //stopwatch.Stop();
            //Network network2 = Network.DeserializeFromXml($@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\hdb_test.xml");
            //Console.WriteLine("Read the file successfully");

            //network2.SerializeToXml($@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\hdb_test2.xml");
            //Console.WriteLine("Saved the second file successfully");

            //Network network3 = Network.DeserializeFromXml($@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\hdb_test2.xml");
            //Console.WriteLine("Read the file successfully");

            //network2.SerializeToXml($@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\hdb_test2.xml");
            //Console.WriteLine("Saved the third file successfull");


            //List<string> lines = new List<string>();

            //Network network = Network.DeserializeFromXml(@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\IEEE 118 Bus - Nodal Variety.xml");
            //network.Initialize();

            //lines.Add("PRAGMA foreign_keys = ON;");
            //lines.Add("");
            //foreach (Substation substation in network.Model.Substations)
            //{
            //    string nodeId = "e57b4a6d-ca9e-403c-ad2e-9a1db9a8a707";
            //    string acronym = substation.Acronym;
            //    string name = substation.Name;
            //    int isConcentrator = 0;
            //    int companyId = 30;
            //    int historianId = 1;
            //    int accessId = 2;
            //    int vendorDeviceId = 2;
            //    int protocolId = 3;
            //    int interconnectionId = 1;
            //    int loadOrder = 0;
            //    int enabled = 1;
            //    lines.Add($"INSERT INTO Device(NodeID, Acronym, Name, IsConcentrator, CompanyID, HistorianID, AccessID, VendorDeviceID, ProtocolID, Longitude, Latitude, InterconnectionID, ConnectionString, MeasuredLines, LoadOrder, Enabled) VALUES('{nodeId}', '{acronym}', '{name}', 0, 30, 1, 2, 2, 3, -89.8038, 35.3871, 1, '', 3, 0, 1);");
            //}

            //using (StreamWriter writer = new StreamWriter(@"C:\Users\kevin\OneDrive\Documents\VT Data\Models\ieee118bus.sql"))
            //{
            //    foreach (string line in lines)
            //    {
            //        writer.WriteLine(line);
            //    }
            //}

            Console.ReadLine();
            #endregion
        }
    }
}
