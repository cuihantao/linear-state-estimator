using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            List<string> filter = new List<string>();
            filter.Add("TVA");

            //network.SerializeToXml(@"C:\Users\kdjones\Desktop\Sensitive Data\tva_model\tvaLseModel.xml");
            Console.WriteLine("Deserializing...");
            Network network = Network.FromHdbExport(@"C:\Users\kdjones\Desktop\Sensitive Data\tva_model\modelFiles.xml", true, filter, true);
            //Network network = Network.DeserializeFromXml(@"C:\Users\kdjones\Desktop\Sensitive Data\tva_model\tvaLseModel.xml");
            Console.WriteLine("Initializing...");
            network.Initialize();
            Console.WriteLine("Filtering...");
            List<int> substationFilter = new List<int>();
            substationFilter.Add(526);
            substationFilter.Add(542);
            substationFilter.Add(543);
            substationFilter.Add(628);
            substationFilter.Add(630);
            substationFilter.Add(640);
            substationFilter.Add(905);
            network.Model.PruneBySubstations(substationFilter);
            List<int> voltageLevelFilter = new List<int>();
            voltageLevelFilter.Add(9);
            network.Model.PruneByVoltageLevels(voltageLevelFilter);
            Console.WriteLine("Serializing...");
            network.SerializeToXml(@"C:\Users\kdjones\Desktop\Sensitive Data\tva_model\tvaLseModel_filtered_X.xml");
            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        //public static Network FromHdbExport(string modelFilesConfigPathName, bool keyify, List<string> companyFilter, bool useAreaLinks)
        //{
        //    Console.WriteLine("Parsing modeling references...");

        //    // Deserialize the list of model files from the xml file
        //    ModelFiles modelFiles = ModelFiles.DeserializeFromXml(modelFilesConfigPathName);

        //    Console.WriteLine("Parsing modeling records...");
        //    // Aggregate the data from all of the files into the context
        //    HdbContext context = new HdbContext(modelFiles);
        //    //Console.WriteLine("Filtering model records...");
        //    //context.Filter(companyFilter);
        //    // Create some placeholder objects for the model build
        //    NetworkModel networkModel = new NetworkModel();
        //    List<Company> companies = new List<Company>();
        //    List<Division> divisions = new List<Division>();
        //    List<Substation> substations = new List<Substation>();
        //    List<Node> nodes = new List<Node>();
        //    List<VoltageLevel> voltageLevels = new List<VoltageLevel>();
        //    List<CircuitBreaker> breakers = new List<CircuitBreaker>();
        //    List<Switch> switches = new List<Switch>();
        //    List<ShuntCompensator> shunts = new List<ShuntCompensator>();
        //    List<LineSegment> lineSegments = new List<LineSegment>();
        //    List<TransmissionLine> transmissionLines = new List<TransmissionLine>();
        //    List<Transformer> transformers = new List<Transformer>();
        //    List<TapConfiguration> taps = new List<TapConfiguration>();
        //    List<BreakerStatus> breakerStatuses = new List<BreakerStatus>();

        //    #region [ Modeling Companies ]

        //    Console.WriteLine("Adding Companies...");

        //    for (int i = 0; i < context.Companies.Count; i++)
        //    {
        //        companies.Add(new Company()
        //        {
        //            InternalID = context.Companies[i].Number,
        //            Number = context.Companies[i].Number,
        //            Acronym = context.Companies[i].Name.ToUpper(),
        //            Name = context.Companies[i].Name,
        //            Description = context.Companies[i].ToString(),
        //        });
        //    }

        //    #endregion

        //    #region [ Modeling Divisions ]

        //    Console.WriteLine("Adding Divisions...");

        //    for (int i = 0; i < context.Divisions.Count; i++)
        //    {

        //        Division division = new Division()
        //        {
        //            InternalID = context.Divisions[i].Number,
        //            Number = context.Divisions[i].Number,
        //            Acronym = context.Divisions[i].Name.ToUpper(),
        //            Name = context.Divisions[i].Name,
        //            Description = context.Divisions[i].Name,
        //        };

        //        divisions.Add(division);

        //        if (useAreaLinks)
        //        {
        //            string areaName = context.Areas.Find(x => x.Number == context.Divisions[i].AreaNumber).Name;
        //            Company parentCompany = companies.Find(x => x.Name == areaName);
        //            division.ParentCompany = parentCompany;
        //            parentCompany.Divisions.Add(division);
        //        }
        //    }

        //    #endregion

        //    #region [ Modeling Substations ]

        //    Console.WriteLine("Adding Substations...");


        //    for (int i = 0; i < context.Stations.Count; i++)
        //    {
        //        var record = context.Stations[i];

        //        substations.Add(new Substation()
        //        {
        //            InternalID = record.Number,
        //            Number = record.Number,
        //            Acronym = record.Name.ToUpper(),
        //            Name = record.Name,
        //            Description = $"{record.Name} Substation"
        //        });
        //    }

        //    #endregion

        //    #region [ Modeling Nodes, Voltage Phasor Groups,  & Voltage Levels ]

        //    Console.WriteLine("Adding Nodes...");

        //    for (int i = 0; i < context.Nodes.Count; i++)
        //    {
        //        var record = context.Nodes[i];
        //        Substation parentSubstation = substations.Find(x => x.Name == record.StationName);
        //        Company parentCompany = companies.Find(x => x.Name == record.CompanyName);

        //        Division parentDivision = null;

        //        if (useAreaLinks)
        //        {
        //            parentDivision = parentCompany.Divisions.Find(x => x.Name == record.DivisionName);
        //        }
        //        else
        //        {
        //            parentDivision = divisions.Find(x => x.Name == record.DivisionName);
        //        }

        //        VoltageLevel voltageLevel = null;
        //        lock (voltageLevels)
        //        {
        //            voltageLevel = voltageLevels.Find(x => x.Value == record.BaseKv);
        //            if (voltageLevel == null)
        //            {
        //                voltageLevel = new VoltageLevel(voltageLevels.Count + 1, record.BaseKv);
        //                voltageLevels.Add(voltageLevel);
        //            }
        //        }

        //        Node node = new Node()
        //        {
        //            InternalID = record.Number,
        //            Number = record.Number,
        //            Acronym = record.Id.ToUpper(),
        //            Name = record.StationName + "_" + record.Id,
        //            Description = record.StationName + "_" + record.Id,
        //            ParentSubstation = parentSubstation,
        //            BaseKV = voltageLevel
        //        };

        //        VoltagePhasorGroup voltage = new VoltagePhasorGroup()
        //        {
        //            InternalID = node.InternalID,
        //            Number = node.Number,
        //            Acronym = node.Name.ToUpper() + "-V",
        //            Name = node.Name + " Voltage Phasor Group",
        //            Description = node.Description + " Voltage Phasor Group",
        //            IsEnabled = true,
        //            UseStatusFlagForRemovingMeasurements = true,
        //            MeasuredNode = node,
        //        };

        //        voltage.ZeroSequence.Measurement.BaseKV = node.BaseKV;
        //        voltage.ZeroSequence.Estimate.BaseKV = node.BaseKV;
        //        voltage.NegativeSequence.Measurement.BaseKV = node.BaseKV;
        //        voltage.NegativeSequence.Estimate.BaseKV = node.BaseKV;
        //        voltage.PositiveSequence.Measurement.BaseKV = node.BaseKV;
        //        voltage.PositiveSequence.Estimate.BaseKV = node.BaseKV;
        //        voltage.PhaseA.Measurement.BaseKV = node.BaseKV;
        //        voltage.PhaseA.Estimate.BaseKV = node.BaseKV;
        //        voltage.PhaseB.Measurement.BaseKV = node.BaseKV;
        //        voltage.PhaseB.Estimate.BaseKV = node.BaseKV;
        //        voltage.PhaseC.Measurement.BaseKV = node.BaseKV;
        //        voltage.PhaseC.Estimate.BaseKV = node.BaseKV;

        //        if (keyify)
        //        {
        //            voltage.Keyify(node.Name);
        //        }

        //        node.Voltage = voltage;
        //        lock (nodes)
        //        {
        //            nodes.Add(node);
        //        }

        //        lock (parentSubstation.Nodes)
        //        {
        //            parentSubstation.Nodes.Add(node);
        //        }
        //        if (!parentDivision.Substations.Contains(parentSubstation))
        //        {
        //            lock (parentDivision)
        //            {
        //                parentDivision.Substations.Add(parentSubstation);
        //            }
        //        }
        //        if (!parentCompany.Divisions.Contains(parentDivision))
        //        {
        //            lock (parentCompany)
        //            {
        //                parentCompany.Divisions.Add(parentDivision);
        //            }
        //        }

        //    }

        //    #endregion

        //    #region [ Modeling Circuit Breakers & Switches ]

        //    Console.WriteLine("Adding Circuit Breakers and Switches");

        //    for (int i = 0; i < context.CircuitBreakers.Count; i++)
        //    {

        //       var record = context.CircuitBreakers[i];
        //       string fromNodeName = record.StationName + "_" + record.FromNodeId;
        //       string toNodeName = record.StationName + "_" + record.ToNodeId;

        //       Node fromNode = nodes.Find(x => x.Name == fromNodeName);
        //       Node toNode = nodes.Find(x => x.Name == toNodeName);

        //       string measurementKey = "Undefined";

        //       if (keyify)
        //       {
        //           measurementKey = $"{record.StationName}.{record.Id}.{record.Type}";
        //       }

        //       if (record.Type == "CB" || record.Type == "C")
        //       {

        //           CircuitBreaker circuitBreaker = new CircuitBreaker()
        //           {
        //               InternalID = record.Number,
        //               Number = record.Number,
        //               Name = record.StationName + "_" + record.Id,
        //               Description = record.ToString(),
        //               FromNode = fromNode,
        //               ToNode = toNode,
        //               ParentSubstation = fromNode.ParentSubstation,
        //               NormalState = SwitchingDeviceNormalState.Closed,
        //               ActualState = SwitchingDeviceActualState.Closed,
        //               MeasurementKey = measurementKey,
        //           };

        //           if (record.IsNormallyOpen == "T")
        //           {
        //               circuitBreaker.NormalState = SwitchingDeviceNormalState.Open;
        //           }

        //           BreakerStatus breakerStatus = new BreakerStatus()
        //           {
        //               InternalID = circuitBreaker.InternalID,
        //               Number = circuitBreaker.Number,
        //               Name = circuitBreaker.Name,
        //               Description = circuitBreaker.Description,
        //               BitPosition = BreakerStatusBit.PSV64,
        //               ParentCircuitBreaker = circuitBreaker,
        //               IsEnabled = false,
        //           };

        //           if (keyify)
        //           {
        //               breakerStatus.Keyify($"{circuitBreaker.Name}");
        //           }

        //           circuitBreaker.Status = breakerStatus;

        //           lock (breakerStatuses)
        //           {
        //               breakerStatuses.Add(breakerStatus);
        //           }
        //           lock (fromNode.ParentSubstation.CircuitBreakers)
        //           {
        //               fromNode.ParentSubstation.CircuitBreakers.Add(circuitBreaker);
        //           }
        //           lock (breakers)
        //           {
        //               breakers.Add(circuitBreaker);
        //           }
        //       }
        //       else
        //       {
        //            // switch
        //            Switch circuitSwitch = new Switch()
        //           {
        //               InternalID = record.Number,
        //               Number = record.Number,
        //               Name = record.StationName + "_" + record.Id,
        //               Description = record.ToString(),
        //               FromNode = fromNode,
        //               ToNode = toNode,
        //               ParentSubstation = fromNode.ParentSubstation,
        //               NormalState = SwitchingDeviceNormalState.Closed,
        //               ActualState = SwitchingDeviceActualState.Closed,
        //               MeasurementKey = measurementKey,
        //           };

        //           if (record.IsNormallyOpen == "T")
        //           {
        //               circuitSwitch.NormalState = SwitchingDeviceNormalState.Open;
        //           }

        //           lock (fromNode.ParentSubstation.Switches)
        //           {
        //               fromNode.ParentSubstation.Switches.Add(circuitSwitch);
        //           }
        //           lock (switches)
        //           {
        //               switches.Add(circuitSwitch);
        //           }
        //       }
        //   }

        //    #endregion

        //    #region [ Modeling Shunt Compensators ]

        //    Console.WriteLine("Adding Shunt Compensators...");

        //    for (int i = 0; i < context.Shunts.Count; i++)
        //    {
        //        string nodeName = context.Shunts[i].StationName + "_" + context.Shunts[i].NodeId;

        //        Node node = nodes.Find(x => x.Name == nodeName);

        //        ShuntCompensator shunt = new ShuntCompensator()
        //        {
        //            InternalID = context.Shunts[i].Number,
        //            Number = context.Shunts[i].Number,
        //            Name = context.Shunts[i].StationName + "_" + context.Shunts[i].Id,
        //            Description = $"{context.Shunts[i].StationName}_{context.Shunts[i].Id} ({context.Shunts[i].NominalMvar})",
        //            ConnectedNode = node,
        //            NominalMvar = context.Shunts[i].NominalMvar,
        //            ParentSubstation = node.ParentSubstation,
        //            ImpedanceCalculationMethod = ShuntImpedanceCalculationMethod.CalculateFromRating,
        //            RawImpedanceParameters = new Impedance(),
        //        };

        //        shunt.Current = new CurrentInjectionPhasorGroup()
        //        {
        //            InternalID = shunt.InternalID,
        //            Number = shunt.Number,
        //            Acronym = shunt.Name.ToUpper(),
        //            Name = shunt.Name + " Current Injection Phasor Group",
        //            Description = shunt.Name + " Current Injection Phasor Group",
        //            IsEnabled = true,
        //            UseStatusFlagForRemovingMeasurements = true,
        //            MeasuredBranch = shunt,
        //            MeasuredConnectedNode = shunt.ConnectedNode
        //        };

        //        shunt.Current.ZeroSequence.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.ZeroSequence.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.NegativeSequence.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.NegativeSequence.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.PositiveSequence.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.PositiveSequence.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.PhaseA.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.PhaseA.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.PhaseB.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.PhaseB.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.PhaseC.Measurement.BaseKV = shunt.ConnectedNode.BaseKV;
        //        shunt.Current.PhaseC.Estimate.BaseKV = shunt.ConnectedNode.BaseKV;

        //        if (keyify)
        //        {
        //            shunt.Current.Keyify($"{shunt.Name}");
        //        }
        //        node.ParentSubstation.Shunts.Add(shunt);
        //        shunts.Add(shunt);
        //    }


        //    #endregion

        //    #region [ Modeling Line Segments, Transmission Lines, & Current Flow Phasor Groups ]

        //    Console.WriteLine("Adding Line Segments, Transmission Lines, & Current Flow Phasor Groups...");

        //    int currentFlowIntegerIndex = 1;

        //    for (int i = 0; i < context.LineSegments.Count; i++)
        //    {
        //        string fromNodeName = $"{context.LineSegments[i].FromStationName}_{context.LineSegments[i].FromNodeId}";
        //        string toNodeName = $"{context.LineSegments[i].ToStationName}_{context.LineSegments[i].ToNodeId}";

        //        Node fromNode = nodes.Find(x => x.Name == fromNodeName);
        //        Node toNode = nodes.Find(x => x.Name == toNodeName);

        //        Division parentDivision = divisions.Find(x => x.Name == context.LineSegments[i].DivisionName);

        //        Impedance impedance = new Impedance()
        //        {
        //            R1 = context.LineSegments[i].Resistance,
        //            X1 = context.LineSegments[i].Reactance,
        //            B1 = context.LineSegments[i].LineCharging,
        //            R3 = context.LineSegments[i].Resistance,
        //            X3 = context.LineSegments[i].Reactance,
        //            B3 = context.LineSegments[i].LineCharging,
        //            R6 = context.LineSegments[i].Resistance,
        //            X6 = context.LineSegments[i].Reactance,
        //            B6 = context.LineSegments[i].LineCharging,
        //        };

        //        LineSegment lineSegment = new LineSegment()
        //        {
        //            InternalID = context.LineSegments[i].Number,
        //            Number = context.LineSegments[i].Number,
        //            Acronym = context.LineSegments[i].TransmissionLineId,
        //            Name = context.LineSegments[i].TransmissionLineId,
        //            Description = context.LineSegments[i].TransmissionLineId,
        //            FromNode = fromNode,
        //            ToNode = toNode,
        //            RawImpedanceParameters = impedance,
        //        };

        //        var line = context.TransmissionLines.Find(x => x.Id == context.LineSegments[i].TransmissionLineId);

        //        TransmissionLine transmissionLine = new TransmissionLine()
        //        {
        //            InternalID = line.Number,
        //            Number = line.Number,
        //            Name = lineSegment.Name,
        //            Acronym = lineSegment.Acronym,
        //            Description = lineSegment.Description,
        //            FromNode = fromNode,
        //            ToNode = toNode,
        //            FromSubstation = fromNode.ParentSubstation,
        //            ToSubstation = toNode.ParentSubstation,
        //            ParentDivision = parentDivision,
        //        };

        //        transmissionLine.FromNode.ParentTransmissionLine = transmissionLine;
        //        transmissionLine.ToNode.ParentTransmissionLine = transmissionLine;

        //        transmissionLine.FromSubstationCurrent = new CurrentFlowPhasorGroup()
        //        {
        //            InternalID = currentFlowIntegerIndex,
        //            Number = currentFlowIntegerIndex,
        //            Acronym = fromNode.Acronym + "-I",
        //            Name = fromNode.Name + " Current Flow Phasor Group",
        //            Description = fromNode.Description + " Current Flow Phasor Group",
        //            IsEnabled = true,
        //            UseStatusFlagForRemovingMeasurements = true,
        //            MeasuredBranch = transmissionLine,
        //            MeasuredFromNode = transmissionLine.FromNode,
        //            MeasuredToNode = transmissionLine.ToNode
        //        };

        //        transmissionLine.FromSubstationCurrent.ZeroSequence.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.ZeroSequence.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.NegativeSequence.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.NegativeSequence.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.PositiveSequence.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.PositiveSequence.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.PhaseA.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.PhaseA.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.PhaseB.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.PhaseB.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.PhaseC.Measurement.BaseKV = transmissionLine.FromNode.BaseKV;
        //        transmissionLine.FromSubstationCurrent.PhaseC.Estimate.BaseKV = transmissionLine.FromNode.BaseKV;

        //        transmissionLine.ToSubstationCurrent = new CurrentFlowPhasorGroup()
        //        {
        //            InternalID = currentFlowIntegerIndex + 1,
        //            Number = currentFlowIntegerIndex + 1,
        //            Acronym = toNode.Acronym + "-I",
        //            Name = toNode.Name + " Current Flow Phasor Group",
        //            Description = toNode.Description + " Current Flow Phasor Group",
        //            IsEnabled = true,
        //            UseStatusFlagForRemovingMeasurements = true,
        //            MeasuredBranch = transmissionLine,
        //            MeasuredFromNode = transmissionLine.ToNode,
        //            MeasuredToNode = transmissionLine.FromNode
        //        };

        //        transmissionLine.ToSubstationCurrent.ZeroSequence.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.ZeroSequence.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.NegativeSequence.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.NegativeSequence.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.PositiveSequence.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.PositiveSequence.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.PhaseA.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.PhaseA.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.PhaseB.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.PhaseB.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.PhaseC.Measurement.BaseKV = transmissionLine.ToNode.BaseKV;
        //        transmissionLine.ToSubstationCurrent.PhaseC.Estimate.BaseKV = transmissionLine.ToNode.BaseKV;

        //        if (keyify)
        //        {
        //            transmissionLine.FromSubstationCurrent.Keyify($"{fromNode.Name}");
        //            transmissionLine.ToSubstationCurrent.Keyify($"{toNode.Name}");
        //        }

        //        lineSegment.ParentTransmissionLine = transmissionLine;

        //        transmissionLine.LineSegments.Add(lineSegment);
        //        parentDivision.TransmissionLines.Add(transmissionLine);

        //        transmissionLines.Add(transmissionLine);
        //        lineSegments.Add(lineSegment);

        //        currentFlowIntegerIndex += 2;
        //    }

        //    #endregion

        //    #region [ Modeling Transformer Taps ]

        //    Console.WriteLine("Adding Transformer Taps...");

        //    for (int i = 0; i < context.TransformerTaps.Count; i++)
        //    {
        //        int minPosition = context.TransformerTaps[i].MinimumPosition;
        //        int maxPosition = context.TransformerTaps[i].MaximumPosition;
        //        int nominalPosition = context.TransformerTaps[i].NominalPosition;
        //        double stepSize = context.TransformerTaps[i].StepSize;

        //        TapConfiguration tap = new TapConfiguration()
        //        {
        //            InternalID = context.TransformerTaps[i].Number,
        //            Number = context.TransformerTaps[i].Number,
        //            Acronym = context.TransformerTaps[i].Id.ToUpper(),
        //            Name = context.TransformerTaps[i].Id,
        //            Description = context.TransformerTaps[i].Id,
        //            PositionLowerBounds = minPosition,
        //            PositionUpperBounds = maxPosition,
        //            PositionNominal = nominalPosition,
        //            LowerBounds = 1.0 + (minPosition - nominalPosition) * stepSize,
        //            UpperBounds = 1.0 + (maxPosition - nominalPosition) * stepSize,
        //        };

        //        taps.Add(tap);
        //    }

        //    #endregion

        //    #region [ Modeling Transformers ] 

        //    Console.WriteLine("Adding Transformers...");

        //    for (int i = 0; i < context.Transformers.Count; i++)
        //    {
        //        string fromNodeName = $"{context.Transformers[i].StationName}_{context.Transformers[i].FromNodeId}";
        //        string toNodeName = $"{context.Transformers[i].StationName}_{context.Transformers[i].ToNodeId}";

        //        Node fromNode = nodes.Find(x => x.Name == fromNodeName);
        //        Node toNode = nodes.Find(x => x.Name == toNodeName);

        //        Impedance impedance = new Impedance()
        //        {
        //            R1 = context.Transformers[i].Resistance,
        //            X1 = context.Transformers[i].Reactance,
        //            G1 = context.Transformers[i].MagnetizingConductance,
        //            B1 = context.Transformers[i].MagnetizingSusceptance,
        //            R3 = context.Transformers[i].Resistance,
        //            X3 = context.Transformers[i].Reactance,
        //            G3 = context.Transformers[i].MagnetizingConductance,
        //            B3 = context.Transformers[i].MagnetizingSusceptance,
        //            R6 = context.Transformers[i].Resistance,
        //            X6 = context.Transformers[i].Reactance,
        //            G6 = context.Transformers[i].MagnetizingConductance,
        //            B6 = context.Transformers[i].MagnetizingSusceptance,
        //        };


        //        Transformer transformer = new Transformer()
        //        {
        //            InternalID = context.Transformers[i].Number,
        //            Number = context.Transformers[i].Number,
        //            Name = $"{context.Transformers[i].StationName}_{context.Transformers[i].Parent}",
        //            Description = $"{context.Transformers[i].StationName}_{context.Transformers[i].Parent}",
        //            FromNode = fromNode,
        //            ToNode = toNode,
        //            ParentSubstation = fromNode.ParentSubstation,
        //            RawImpedanceParameters = impedance,
        //            UltcIsEnabled = false,
        //            FromNodeConnectionType = TransformerConnectionType.Wye,
        //            ToNodeConnectionType = TransformerConnectionType.Wye,
        //        };

        //        TapConfiguration tap = taps.Find(x => x.Name == context.Transformers[i].FromNodeTap);

        //        if (tap != null)
        //        {
        //            transformer.Tap = tap;
        //        }

        //        transformer.FromNodeCurrent = new CurrentFlowPhasorGroup()
        //        {
        //            InternalID = currentFlowIntegerIndex,
        //            Number = currentFlowIntegerIndex,
        //            Acronym = fromNode.Acronym + "-I",
        //            Name = fromNode.Name + " Current Flow Phasor Group",
        //            Description = fromNode.Description + " Current Flow Phasor Group",
        //            IsEnabled = true,
        //            UseStatusFlagForRemovingMeasurements = true,
        //            MeasuredBranch = transformer,
        //            MeasuredFromNode = transformer.FromNode,
        //            MeasuredToNode = transformer.ToNode
        //        };

        //        transformer.FromNodeCurrent.ZeroSequence.Measurement.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.ZeroSequence.Estimate.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.NegativeSequence.Measurement.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.NegativeSequence.Estimate.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.PositiveSequence.Measurement.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.PositiveSequence.Estimate.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.PhaseA.Measurement.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.PhaseA.Estimate.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.PhaseB.Measurement.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.PhaseB.Estimate.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.PhaseC.Measurement.BaseKV = transformer.FromNode.BaseKV;
        //        transformer.FromNodeCurrent.PhaseC.Estimate.BaseKV = transformer.FromNode.BaseKV;

        //        transformer.ToNodeCurrent = new CurrentFlowPhasorGroup()
        //        {
        //            InternalID = currentFlowIntegerIndex + 1,
        //            Number = currentFlowIntegerIndex + 1,
        //            Acronym = toNode.Acronym + "-I",
        //            Name = toNode.Name + " Current Flow Phasor Group",
        //            Description = toNode.Description + " Current Flow Phasor Group",
        //            IsEnabled = true,
        //            UseStatusFlagForRemovingMeasurements = true,
        //            MeasuredBranch = transformer,
        //            MeasuredFromNode = transformer.ToNode,
        //            MeasuredToNode = transformer.FromNode
        //        };

        //        transformer.ToNodeCurrent.ZeroSequence.Measurement.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.ZeroSequence.Estimate.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.NegativeSequence.Measurement.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.NegativeSequence.Estimate.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.PositiveSequence.Measurement.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.PositiveSequence.Estimate.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.PhaseA.Measurement.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.PhaseA.Estimate.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.PhaseB.Measurement.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.PhaseB.Estimate.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.PhaseC.Measurement.BaseKV = transformer.ToNode.BaseKV;
        //        transformer.ToNodeCurrent.PhaseC.Estimate.BaseKV = transformer.ToNode.BaseKV;

        //        if (keyify)
        //        {
        //            transformer.FromNodeCurrent.Keyify($"{fromNode.Name}");
        //            transformer.ToNodeCurrent.Keyify($"{toNode.Name}");
        //            transformer.Keyify($"{transformer.Name}");
        //        }

        //        currentFlowIntegerIndex += 2;

        //        fromNode.ParentSubstation.Transformers.Add(transformer);
        //        transformers.Add(transformer);
        //    }

        //    #endregion

        //    #region [ Composing the Network Model ]


        //    networkModel.Companies = companies;

        //    networkModel.VoltageLevels = voltageLevels;
        //    networkModel.TapConfigurations = taps;
        //    networkModel.BreakerStatuses = breakerStatuses;

        //    #endregion

        //    Network network = new Network(networkModel);
        //    Console.WriteLine("Serializing...");
        //    network.SerializeToXml(@"C:\Users\kdjones\Desktop\Sensitive Data\tva_model\tvaLseModel.xml");
        //    Console.WriteLine("Initializing...");
        //    network.Initialize();
        //    return network;
        //}

    }
}
