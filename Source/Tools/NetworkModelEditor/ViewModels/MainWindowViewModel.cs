//******************************************************************************************************
//  MainWindowViewModel.cs
//
//  Copyright © 2014, Kevin D. Jones.  All Rights Reserved.
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
//  02/01/2014 - Kevin D. Jones
//       Generated original version of source code.
//
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;
using NetworkModelEditor.Commands;
using SynchrophasorAnalytics.Networks;
using SynchrophasorAnalytics.Matrices;
using SynchrophasorAnalytics.Measurements;
using SynchrophasorAnalytics.Modeling;
using SynchrophasorAnalytics.Testing;
using SynchrophasorAnalytics.Psse;


namespace NetworkModelEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region [ Private Members ] 

        #region [ Computational Milestones ] 

        private bool m_networkIsInitialized;
        private bool m_measurementsAreMapped;
        private bool m_activeCurrentFlowsHaveBeenDetermined;
        private bool m_activeCurrentInjectionsHaveBeenDetermined;
        private bool m_observedBussesHaveBeenResolved;
        private bool m_singleFlowBranchesHaveBeenResolved;
        private bool m_stateWasComputed;
        
        #endregion

        #region [ Data ] 

        private Network m_network;
        private List<RawMeasurements> m_measurementSamples;
        private RawMeasurements m_selectedMeasurementSample;

        #endregion

        #region [ Commands ] 

        #region [ File Opening ] 

        private RelayCommand m_openFileCommand;
        private RelayCommand m_openMeasurementSampleFileCommand;
        private RelayCommand m_openPsseRawFileCommand;
        private RelayCommand m_openHdbExportFilesCommand;

        #endregion

        #region [ File Saving ] 

        private RelayCommand m_saveFileCommand;
        private RelayCommand m_saveMeasurementSampleFilesCommand;
        private RelayCommand m_saveNetworkSnapshotFileCommand;

        #endregion

        #region [ User Interface ] 

        private RelayCommand m_changeSelectedElementCommand;
        private RelayCommand m_viewDetailCommand;
        private RelayCommand m_refreshNetworkTreeCommand;

        #endregion

        #region [ Utilities ] 

        private RelayCommand m_unkeyifyModelCommand;
        private RelayCommand m_generateMeasurementSamplesCommand;
        private RelayCommand m_pruneModelCommand;

        #endregion

        #region [ Setup ] 

        private RelayCommand m_initializeModelCommand;
        private RelayCommand m_selectMeasurementSampleCommand;
        private RelayCommand m_clearMeasurementsFromModelCommand;
        private RelayCommand m_mapMeasurementsToModelCommand;

        #endregion

        #region [ Observability Analysis ] 

        private RelayCommand m_determineActiveCurrentFlowsCommand;
        private RelayCommand m_determineActiveCurrentInjectionsCommand;
        private RelayCommand m_resolvetoObservedBusesCommand;
        private RelayCommand m_resolveToSingleFlowBranchesCommand;

        #endregion

        #region [ Computation ] 

        private RelayCommand m_computeSystemStateCommand;
        private RelayCommand m_computeLineFlowsCommand;
        private RelayCommand m_computeInjectionsCommand;
        private RelayCommand m_computePowerFlowsCommand;
        private RelayCommand m_computeSequenceComponentsCommand;

        #endregion

        #region [ Matrices ] 

        private RelayCommand m_viewAMatrixCommand;
        private RelayCommand m_viewIIMatrixCommand;
        private RelayCommand m_viewYMatrixCommand;
        private RelayCommand m_viewYsMatrixCommand;
        private RelayCommand m_viewYshMatrixCommand;

        #endregion

        #region [ Inspect ] 

        private RelayCommand m_viewReceivedMeasurementsCommand;
        private RelayCommand m_viewUnreceivedMeasurementsCommand;
        private RelayCommand m_viewComponentsCommand;
        private RelayCommand m_viewObservableNodesCommand;
        private RelayCommand m_viewSubstationAdjacencyListCommand;
        private RelayCommand m_viewTransmissionLineAdjacencyListCommand;
        private RelayCommand m_viewCalculatedImpedancesCommand;
        private RelayCommand m_viewSeriesCompensatorInferenceDataCommand;
        private RelayCommand m_viewSeriesCompensatorsCommand;
        private RelayCommand m_viewTransformersCommand;
        private RelayCommand m_viewOutputMeasurementsCommand;
        private RelayCommand m_viewModeledVoltagesCommand;
        private RelayCommand m_viewExpectedVoltagesCommand;
        private RelayCommand m_viewActiveVoltagesCommand;
        private RelayCommand m_viewInactiveVoltagesCommand;
        private RelayCommand m_viewModeledCurrentFlowsCommand;
        private RelayCommand m_viewExpectedCurrentFlowsCommand;
        private RelayCommand m_viewActiveCurrentFlowsCommand;
        private RelayCommand m_viewInactiveCurrentFlowsCommand;
        private RelayCommand m_viewIncludedCurrentFlowsCommand;
        private RelayCommand m_viewExcludedCurrentFlowsCommand;
        private RelayCommand m_viewModeledCurrentInjectionsCommand;
        private RelayCommand m_viewExpectedCurrentInjectionsCommand;
        private RelayCommand m_viewActiveCurrentInjectionsCommand;
        private RelayCommand m_viewInactiveCurrentInjectionsCommand;

        #endregion

        #endregion

        #region [ View Models ] 

        private List<MenuItemViewModel> m_menuBarItemViewModels;
        private NetworkTreeViewModel m_networkTreeViewModel;
        private RecordDetailViewModel m_recordDetailViewModel;
        private MeasurementSampleDetailViewModel m_measurementSampleViewModel;

        #endregion

        #region [ Menu Bar Items ]

        private MenuItemViewModel m_fileMenuItem;

        private MenuItemViewModel m_openMenuItem;
        private MenuItemViewModel m_saveMenuItem;

        // UTILITIES
        private MenuItemViewModel m_utilitiesMenuItem;

        private MenuItemViewModel m_modelActionsMenuItem;
        private MenuItemViewModel m_unkeyifyModelMenuItem;
        private MenuItemViewModel m_pruneModelMenuItem;

        private MenuItemViewModel m_measurementActionsMenuItem;
        private MenuItemViewModel m_generateMeasurementSamplesMenuItem;

        // OFFLINE ANALYSIS
        private MenuItemViewModel m_offlineAnalysisMenuItem;

        private MenuItemViewModel m_setupMenuItem;
        private MenuItemViewModel m_observabilityMenuItem;
        private MenuItemViewModel m_matricesMenuItem;
        private MenuItemViewModel m_computationMenuItem;
        private MenuItemViewModel m_inspectMenuItem;

        private MenuItemViewModel m_openNetworkModelMenuItem;
        private MenuItemViewModel m_openMeasurementSampleMenuItem;
        private MenuItemViewModel m_openPsseRawFileMenuItem;
        private MenuItemViewModel m_openHdbExportFilesMenuItem;
        private MenuItemViewModel m_saveNetworkModelMenuItem;
        private MenuItemViewModel m_saveNetworkSnapshotMenuItem;
        private MenuItemViewModel m_saveMeasurementSampleFileMenuItem;


        private MenuItemViewModel m_clearMeasurementsMenuItem;
        private MenuItemViewModel m_initializeModelMenuItem;
        private MenuItemViewModel m_mapMeasurementsMenuItem;

        private MenuItemViewModel m_determineActiveCurrentFlowsMenuItem;
        private MenuItemViewModel m_determineActiveCurrentInjectionsMenuItem;
        private MenuItemViewModel m_resolveToObservedBusesMenuItem;
        private MenuItemViewModel m_resolveToSingleFlowBranchesMenuItem;
        
        private MenuItemViewModel m_viewAMatrixMenuItem;
        private MenuItemViewModel m_viewIIMatrixMenuItem;
        private MenuItemViewModel m_viewYMatrixMenuItem;
        private MenuItemViewModel m_viewYsMatrixMenuItem;
        private MenuItemViewModel m_viewYshMatrixMenuItem;

        private MenuItemViewModel m_computeSystemStateMenuItem;
        private MenuItemViewModel m_computeLineFlowsMenuItem;
        private MenuItemViewModel m_computeInjectionsMenuItem;
        private MenuItemViewModel m_computePowerFlowsMenuItem;
        private MenuItemViewModel m_computeSequenceComponentsMenuItem;

        private MenuItemViewModel m_viewComponentSummaryMenuItem;
        private MenuItemViewModel m_viewReceivedMeasurementsMenuItem;
        private MenuItemViewModel m_viewUnreceivedMeasurementsMenuItem;
        private MenuItemViewModel m_viewOutputMeasurementsMenuItem;
        private MenuItemViewModel m_viewModeledVoltagesMenuItem;
        private MenuItemViewModel m_viewExpectedVoltagesMenuItem;
        private MenuItemViewModel m_viewActiveVoltagesMenuItem;
        private MenuItemViewModel m_viewInactiveVoltagesMenuItem;
        private MenuItemViewModel m_viewModeledCurrentFlowsMenuItem;
        private MenuItemViewModel m_viewExpectedCurrentFlowsMenuItem;
        private MenuItemViewModel m_viewActiveCurrentFlowsMenuItem;
        private MenuItemViewModel m_viewInactiveCurrentFlowsMenuItem;
        private MenuItemViewModel m_viewModeledCurrentInjectionsMenuItem;
        private MenuItemViewModel m_viewExpectedCurrentInjectionsMenuItem;
        private MenuItemViewModel m_viewActiveCurrentInjectionsMenuItem;
        private MenuItemViewModel m_viewInactiveCurrentInjectionsMenuItem;
        private MenuItemViewModel m_viewIncludedCurrentFlowsMenuItem;
        private MenuItemViewModel m_viewExcludedCurrentFlowsMenuItem;
        private MenuItemViewModel m_viewObservableNodesMenuItem;
        private MenuItemViewModel m_viewSubstationAdjacencyListMenuItem;
        private MenuItemViewModel m_viewTransmissionLineAdjacencyListMenuItem;
        private MenuItemViewModel m_viewCalculatedImpedancesMenuItem;
        private MenuItemViewModel m_viewSeriesCompensatorInferenceDataMenuItem;
        private MenuItemViewModel m_viewSeriesCompensatorsMenuItem;
        private MenuItemViewModel m_viewTransformersMenuItem;

        private MenuItemViewModel m_exitMenuItem;
        
        #endregion

        #region [ Status Bar ] 

        private string m_actionStatus;
        private string m_communicationStatus;
        private string m_specialStatus;

        #endregion

        #endregion

        #region [ Public Properties ] 

        #region [ View Models ] 

        public NetworkTreeViewModel NetworkTree
        {
            get
            {
                return m_networkTreeViewModel;
            }
            set
            {
                m_networkTreeViewModel = value;
            }
        }

        public RecordDetailViewModel RecordDetail
        {
            get
            {
                return m_recordDetailViewModel;
            }
            set
            {
                m_recordDetailViewModel = value;
            }
        }

        public ObservableCollection<MenuItemViewModel> MenuBarItems
        {
            get
            {
                return new ObservableCollection<MenuItemViewModel>(m_menuBarItemViewModels);
            }
        }

        public MeasurementSampleDetailViewModel MeasurementSample
        {
            get
            {
                return m_measurementSampleViewModel;
            }
            set
            {
                m_measurementSampleViewModel = value;
            }
        }

        #endregion

        #region [ Status Bar ]

        public string ActionStatus
        {
            get
            {
                return m_actionStatus;
            }
            set
            {
                m_actionStatus = value;
                OnPropertyChanged("ActionStatus");
            }
        }

        public string CommunicationStatus
        {
            get
            {
                return m_communicationStatus;
            }
            set
            {
                m_communicationStatus = value;
                OnPropertyChanged("CommunicationStatus");
            }
        }

        public string SpecialStatus
        {
            get
            {
                return m_specialStatus;
            }
            set
            {
                m_specialStatus = value;
                OnPropertyChanged("SpecialStatus");
            }
        }

        #endregion

        #region [ Data ]

        public RawMeasurements SelectedMeasurementSample
        {
            get
            {
                return m_selectedMeasurementSample;
            }
            set
            {
                SpecialStatus = $"Currently selected 'Sample {value.Identifier}'";
                m_selectedMeasurementSample = value;
            }
        }

        #endregion

        #region [ Commands ]

        public ICommand OpenFileCommand
        {
            get 
            {
                if (m_openFileCommand == null)
                {
                    m_openFileCommand = new RelayCommand(param => this.OpenFile(), param => true);
                }
                return m_openFileCommand;
            }
        }

        public ICommand OpenMeasurementSampleFileCommand
        {
            get
            {
                if (m_openMeasurementSampleFileCommand == null)
                {
                    m_openMeasurementSampleFileCommand = new RelayCommand(param => this.OpenMeasurementSampleFile(), param => true);
                }
                return m_openMeasurementSampleFileCommand;
            }
        }

        public ICommand OpenPsseRawFileCommand
        {
            get
            {
                if (m_openPsseRawFileCommand == null)
                {
                    m_openPsseRawFileCommand = new RelayCommand(param => this.OpenPsseRawFile(), param => true);
                }
                return m_openPsseRawFileCommand;
            }
        }

        public ICommand OpenHdbExportFilesCommand
        {
            get
            {
                if (m_openHdbExportFilesCommand == null)
                {
                    m_openHdbExportFilesCommand = new RelayCommand(param => this.OpenHdbExportFiles(), param => true);
                }
                return m_openHdbExportFilesCommand;
            }

        }

        public ICommand SaveFileCommand
        {
            get
            {
                if (m_saveFileCommand == null)
                {
                    m_saveFileCommand = new RelayCommand(param => this.SaveFile(), param => true);
                }
                return m_saveFileCommand;
            }
        }

        public ICommand ChangeSelectedElementCommand
        {
            get
            {
                if (m_changeSelectedElementCommand == null)
                {
                    m_changeSelectedElementCommand = new RelayCommand(param => this.ChangeSelectedElement(), param => true);
                }
                return m_changeSelectedElementCommand;
            }
        }

        public ICommand ViewDetailCommand
        {
            get
            {
                if (m_viewDetailCommand == null)
                {
                    m_viewDetailCommand = new RelayCommand(param => this.ViewDetail(), param => true);
                }
                return m_viewDetailCommand;
            }
        }

        public ICommand UpdateTreeViewCommand
        {
            get
            {
                if (m_refreshNetworkTreeCommand == null)
                {
                    m_refreshNetworkTreeCommand = new RelayCommand(param => this.RefreshNetworkTree(), param => true);
                }
                return m_refreshNetworkTreeCommand;
            }
        }

        public ICommand UnkeyifyModelCommand
        {
            get
            {
                if (m_unkeyifyModelCommand == null)
                {
                    m_unkeyifyModelCommand = new RelayCommand(param => this.UnkeyifyModel(), param => true);
                }
                return m_unkeyifyModelCommand;
            }
        }

        public ICommand PruneModelCommand
        {
            get
            {
                if (m_pruneModelCommand == null)
                {
                    m_pruneModelCommand = new RelayCommand(param => this.PruneModel(), param => true);
                }
                return m_pruneModelCommand;
            }
        }

        #region [ Setup Commands ] 

        public ICommand InitializeModelCommand
        {
            get
            {
                if (m_initializeModelCommand == null)
                {
                    m_initializeModelCommand = new RelayCommand(param => this.InitializeModel(), param => true);
                }
                return m_initializeModelCommand;
            }
        }

        public ICommand SaveMeasurementSampleFilesCommand
        {
            get
            {
                if (m_saveMeasurementSampleFilesCommand == null)
                {
                    m_saveMeasurementSampleFilesCommand = new RelayCommand(param => this.SaveMeasurementSampleFile(), param => true);
                }
                return m_saveMeasurementSampleFilesCommand;
            }
        }

        public ICommand SaveNetworkSnapshotFileCommand
        {
            get
            {
                if (m_saveNetworkSnapshotFileCommand == null)
                {
                    m_saveNetworkSnapshotFileCommand = new RelayCommand(param => this.SaveNetworkSnapshotFile(), param => true);
                }
                return m_saveNetworkSnapshotFileCommand;
            }
        }

        public ICommand SelectMeasurementSampleCommand
        {
            get
            {
                if (m_selectMeasurementSampleCommand == null)
                {
                    m_selectMeasurementSampleCommand = new RelayCommand(param => this.SelectMeasurementSample(), param => true);
                }
                return m_selectMeasurementSampleCommand;
            }
        }

        public ICommand ClearMeasurementsFromModelCommand
        {
            get
            {
                if (m_clearMeasurementsFromModelCommand == null)
                {
                    m_clearMeasurementsFromModelCommand = new RelayCommand(param => this.ClearMeasurementsFromModel(), param => true);
                }
                return m_clearMeasurementsFromModelCommand;
            }
        }

        public ICommand MapMeasurementsToModelCommand
        {
            get
            {
                if (m_mapMeasurementsToModelCommand == null)
                {
                    m_mapMeasurementsToModelCommand = new RelayCommand(param => this.MapMeasurementsToModel(), param => true);
                }
                return m_mapMeasurementsToModelCommand;
            }
        }

        public ICommand ViewUnreceivedMeasurementsCommand
        {
            get
            {
                if (m_viewUnreceivedMeasurementsCommand == null)
                {
                    m_viewUnreceivedMeasurementsCommand = new RelayCommand(param => this.ViewUnreceivedMeasurements(), param => true);
                }
                return m_viewUnreceivedMeasurementsCommand;
            }
        }

        public ICommand ViewReceivedMeasurementsCommand
        {
            get
            {
                if (m_viewReceivedMeasurementsCommand == null)
                {
                    m_viewReceivedMeasurementsCommand = new RelayCommand(param => this.ViewReceivedMeasurements(), param => true);
                }
                return m_viewReceivedMeasurementsCommand;
            }
        }

        public ICommand ViewComponentsCommand
        {
            get
            {
                if (m_viewComponentsCommand == null)
                {
                    m_viewComponentsCommand = new RelayCommand(param => this.ViewComponents(), param => true);
                }
                return m_viewComponentsCommand;
            }
        }

        public ICommand GenerateMeasurementSamplesCommand
        {
            get
            {
                if (m_generateMeasurementSamplesCommand == null)
                {
                    m_generateMeasurementSamplesCommand = new RelayCommand(param => this.GenerateMeasurementSamplesFromCsv(), param => true);
                }
                return m_generateMeasurementSamplesCommand;
            }
        }

        #endregion

        #region [ Observability Analysis Commands ] 

        public ICommand DetermineActiveCurrentFlowsCommand
        {
            get
            {
                if (m_determineActiveCurrentFlowsCommand == null)
                {
                    m_determineActiveCurrentFlowsCommand = new RelayCommand(param => this.DetermineActiveCurrentFlows(), param => true);
                }
                return m_determineActiveCurrentFlowsCommand;
            }
        }

        public ICommand DetermineActiveCurrentInjectionsCommand
        {
            get
            {
                if (m_determineActiveCurrentInjectionsCommand == null)
                {
                    m_determineActiveCurrentInjectionsCommand = new RelayCommand(param => this.DetermineActiveCurrentInjections(), param => true);
                }
                return m_determineActiveCurrentInjectionsCommand;
            }
        }

        public ICommand ResolvetoObservedBusesCommand
        {
            get
            {
                if (m_resolvetoObservedBusesCommand == null)
                {
                    m_resolvetoObservedBusesCommand = new RelayCommand(param => this.ResolveToObservedBuses(), param => true);
                }
                return m_resolvetoObservedBusesCommand;
            }
        }

        public ICommand ResolveToSingleFlowBranchesCommand
        {
            get
            {
                if (m_resolveToSingleFlowBranchesCommand == null)
                {
                    m_resolveToSingleFlowBranchesCommand = new RelayCommand(param => this.ResolveToSingleFlowBranches(), param => true);
                }
                return m_resolveToSingleFlowBranchesCommand;
            }
        }
        
        #endregion

        #region [ Computation Commands ] 

        public ICommand ComputeSystemStateCommand
        {
            get
            {
                if (m_computeSystemStateCommand == null)
                {
                    m_computeSystemStateCommand = new RelayCommand(param => this.ComputeSystemState(), param => true);
                }
                return m_computeSystemStateCommand;
            }
        }

        public ICommand ComputeLineFlowsCommand
        {
            get
            {
                if (m_computeLineFlowsCommand == null)
                {
                    m_computeLineFlowsCommand = new RelayCommand(param => this.ComputeLineFlows(), param => true);
                }
                return m_computeLineFlowsCommand;
            }
        }

        public ICommand ComputeInjectionsCommand
        {
            get
            {
                if (m_computeInjectionsCommand == null)
                {
                    m_computeInjectionsCommand = new RelayCommand(param => this.ComputeInjections(), param => true);
                }
                return m_computeInjectionsCommand;
            }
        }

        public ICommand ComputePowerFlowsCommand
        {
            get
            {
                if (m_computePowerFlowsCommand == null)
                {
                    m_computePowerFlowsCommand = new RelayCommand(param => this.ComputePowerFlows(), param => true);
                }
                return m_computePowerFlowsCommand;
            }
        }

        public ICommand ComputeSequenceComponentsCommand
        {
            get
            {
                if (m_computeSequenceComponentsCommand == null)
                {
                    m_computeSequenceComponentsCommand = new RelayCommand(param => this.ComputeSequenceComponents(), param => true);
                }
                return m_computeSequenceComponentsCommand;
            }
        }

        #endregion

        #region [ View Matrix Commands ]

        public ICommand ViewAMatrixCommand
        {
            get
            {
                if (m_viewAMatrixCommand == null)
                {
                    m_viewAMatrixCommand = new RelayCommand(param => this.ViewAMatrix(), param => true);
                }
                return m_viewAMatrixCommand;
            }
        }

        public ICommand ViewIIMatrixCommand
        {
            get
            {
                if (m_viewIIMatrixCommand == null)
                {
                    m_viewIIMatrixCommand = new RelayCommand(param => this.ViewIIMatrix(), param => true);
                }
                return m_viewIIMatrixCommand;
            }
        }

        public ICommand ViewYMatrixCommand
        {
            get
            {
                if (m_viewYMatrixCommand == null)
                {
                    m_viewYMatrixCommand = new RelayCommand(param => this.ViewYMatrix(), param => true);
                }
                return m_viewYMatrixCommand;
            }
        }

        public ICommand ViewYsMatrixCommand
        {
            get
            {
                if (m_viewYsMatrixCommand == null)
                {
                    m_viewYsMatrixCommand = new RelayCommand(param => this.ViewYsMatrix(), param => true);
                }
                return m_viewYsMatrixCommand;           
            }
        }

        public ICommand ViewYshMatrixCommand
        {
            get
            {
                if (m_viewYshMatrixCommand == null)
                {
                    m_viewYshMatrixCommand = new RelayCommand(param => this.ViewYshMatrix(), param => true);
                }
                return m_viewYshMatrixCommand;
            }
        }

        #endregion

        #region [ Inspect Commands ] 

        public ICommand ViewObservableNodesCommand
        {
            get
            {
                if (m_viewObservableNodesCommand == null)
                {
                    m_viewObservableNodesCommand = new RelayCommand(param => this.ViewObservableNodes(), param => true);
                }
                return m_viewObservableNodesCommand;
            }
        }

        public ICommand ViewSubstationAdjacencyListCommand
        {
            get
            {
                if (m_viewSubstationAdjacencyListCommand == null)
                {
                    m_viewSubstationAdjacencyListCommand = new RelayCommand(param => this.ViewSubstationAdjacencyList(), param => true);
                }
                return m_viewSubstationAdjacencyListCommand;
            }
        }

        public ICommand ViewTransmissionLineAdjacencyListCommand
        {
            get
            {
                if (m_viewTransmissionLineAdjacencyListCommand == null)
                {
                    m_viewTransmissionLineAdjacencyListCommand = new RelayCommand(param => this.ViewTransmissionLineAdjacencyLists(), param => true);
                }
                return m_viewTransmissionLineAdjacencyListCommand;
            }
        }

        public ICommand ViewCalculatedImpedancesCommand
        {
            get
            {
                if (m_viewCalculatedImpedancesCommand == null)
                {
                    m_viewCalculatedImpedancesCommand = new RelayCommand(param => this.ViewCalculatedImpedances(), param => true);
                }
                return m_viewCalculatedImpedancesCommand;
            }
        }

        public ICommand ViewSeriesCompensatorInferenceDataCommand
        {
            get
            {
                if (m_viewSeriesCompensatorInferenceDataCommand == null)
                {
                    m_viewSeriesCompensatorInferenceDataCommand = new RelayCommand(param => this.ViewSeriesCompensatorInferenceData(), param => true);
                }
                return m_viewSeriesCompensatorInferenceDataCommand;
            }
        }

        public ICommand ViewSeriesCompensatorsCommand
        {
            get
            {
                if (m_viewSeriesCompensatorsCommand == null)
                {
                    m_viewSeriesCompensatorsCommand = new RelayCommand(param => this.ViewSeriesCompensators(), param => true);
                }
                return m_viewSeriesCompensatorsCommand;
            }
        }

        public ICommand ViewTransformersCommand
        {
            get
            {
                if (m_viewTransformersCommand == null)
                {
                    m_viewTransformersCommand = new RelayCommand(param => this.ViewTransformers(), param => true);
                }
                return m_viewTransformersCommand;
            }
        }

        public ICommand ViewOutputMeasurementsCommand
        {
            get
            {
                if (m_viewOutputMeasurementsCommand == null)
                {
                    m_viewOutputMeasurementsCommand = new RelayCommand(param => this.ViewOutputMeasurements(), param => true);
                }
                return m_viewOutputMeasurementsCommand;
            }
        }

        public ICommand ViewModeledVoltagesCommand
        {
            get
            {
                if (m_viewModeledVoltagesCommand == null)
                {
                    m_viewModeledVoltagesCommand = new RelayCommand(param => this.ViewModeledVoltages(), param => true);
                }
                return m_viewModeledVoltagesCommand;
            }
        }

        public ICommand ViewExpectedVoltagesCommand
        {
            get
            {
                if (m_viewExpectedVoltagesCommand == null)
                {
                    m_viewExpectedVoltagesCommand = new RelayCommand(param => this.ViewExpectedVoltages(), param => true);
                }
                return m_viewExpectedVoltagesCommand;
            }
        }

        public ICommand ViewActiveVoltagesCommand
        {
            get
            {
                if (m_viewActiveVoltagesCommand == null)
                {
                    m_viewActiveVoltagesCommand = new RelayCommand(param => this.ViewActiveVoltages(), param => true);
                }
                return m_viewActiveVoltagesCommand;
            }
        }

        public ICommand ViewInactiveVoltagesCommand
        {
            get
            {
                if (m_viewInactiveVoltagesCommand == null)
                {
                    m_viewInactiveVoltagesCommand = new RelayCommand(param => this.ViewInactiveVoltages(), param => true);
                }
                return m_viewInactiveVoltagesCommand;
            }
        }

        public ICommand ViewModeledCurrentFlowsCommand
        {
            get
            {
                if (m_viewModeledCurrentFlowsCommand == null)
                {
                    m_viewModeledCurrentFlowsCommand = new RelayCommand(param => this.ViewModeledCurrentFlows(), param => true);
                }
                return m_viewModeledCurrentFlowsCommand;
            }
        }

        public ICommand ViewExpectedCurrentFlowsCommand
        {
            get
            {
                if (m_viewExpectedCurrentFlowsCommand == null)
                {
                    m_viewExpectedCurrentFlowsCommand = new RelayCommand(param => this.ViewExpectedCurrentFlows(), param => true);
                }
                return m_viewExpectedCurrentFlowsCommand;
            }
        }

        public ICommand ViewActiveCurrentFlowsCommand
        {
            get
            {
                if (m_viewActiveCurrentFlowsCommand == null)
                {
                    m_viewActiveCurrentFlowsCommand = new RelayCommand(param => this.ViewActiveCurrentFlows(), param => true);
                }
                return m_viewActiveCurrentFlowsCommand;
            }
        }

        public ICommand ViewInactiveCurrentFlowsCommand
        {
            get
            {
                if (m_viewInactiveCurrentFlowsCommand == null)
                {
                    m_viewInactiveCurrentFlowsCommand = new RelayCommand(param => this.ViewInactiveCurrentFlows(), param => true);
                }
                return m_viewInactiveCurrentFlowsCommand;
            }
        }

        public ICommand ViewIncludedCurrentFlowsCommand
        {
            get
            {
                if (m_viewIncludedCurrentFlowsCommand == null)
                {
                    m_viewIncludedCurrentFlowsCommand = new RelayCommand(param => this.ViewIncludedCurrentFlows(), param => true);
                }
                return m_viewIncludedCurrentFlowsCommand;
            }
        }

        public ICommand ViewExcludedCurrentFlowsCommand
        {
            get
            {
                if (m_viewExcludedCurrentFlowsCommand == null)
                {
                    m_viewExcludedCurrentFlowsCommand = new RelayCommand(param => this.ViewExcludedCurrentFlows(), param => true);
                }
                return m_viewExcludedCurrentFlowsCommand;
            }
        }

        public ICommand ViewModeledCurrentInjectionsCommand
        {
            get
            {
                if (m_viewModeledCurrentInjectionsCommand == null)
                {
                    m_viewModeledCurrentInjectionsCommand = new RelayCommand(param => this.ViewModeledCurrentInjections(), param => true);
                }
                return m_viewModeledCurrentInjectionsCommand;
            }
        }

        public ICommand ViewExpectedCurrentInjectionsCommand
        {
            get
            {
                if (m_viewExpectedCurrentInjectionsCommand == null)
                {
                    m_viewExpectedCurrentInjectionsCommand = new RelayCommand(param => this.ViewExpectedCurrentInjections(), param => true);
                }
                return m_viewExpectedCurrentInjectionsCommand;
            }
        }

        public ICommand ViewActiveCurrentInjectionsCommand
        {
            get
            {
                if (m_viewActiveCurrentInjectionsCommand == null)
                {
                    m_viewActiveCurrentInjectionsCommand = new RelayCommand(param => this.ViewActiveCurrentInjections(), param => true);
                }
                return m_viewActiveCurrentInjectionsCommand;
            }
        }

        public ICommand ViewInactiveCurrentInjectionsCommand
        {
            get
            {
                if (m_viewInactiveCurrentInjectionsCommand == null)
                {
                    m_viewInactiveCurrentInjectionsCommand = new RelayCommand(param => this.ViewInactiveCurrentInjections(), param => true);
                }
                return m_viewInactiveCurrentInjectionsCommand;
            }
        }

        #endregion

        #endregion

        #endregion

        #region [ Constructor ]

        public MainWindowViewModel()
        {
            InitializeMenuBar();
            InitializeNetworkTree();
            InitializeRecordDetail();

            m_networkIsInitialized = false;
            m_measurementsAreMapped = false;
            m_activeCurrentFlowsHaveBeenDetermined = false;
            m_activeCurrentInjectionsHaveBeenDetermined = false;
            m_observedBussesHaveBeenResolved = false;
            m_singleFlowBranchesHaveBeenResolved = false;
            m_stateWasComputed = false;

            DisableControls();
        }

        #endregion

        #region [ Enable Controls ]

        private void EnableControls()
        {
            if (m_selectedMeasurementSample != null && m_network != null)
            {
                m_initializeModelMenuItem.IsEnabled = true;
            }
            if (m_networkIsInitialized)
            {
                m_saveNetworkSnapshotMenuItem.IsEnabled = true;
                m_mapMeasurementsMenuItem.IsEnabled = true;
                m_viewComponentSummaryMenuItem.IsEnabled = true;
                m_viewSeriesCompensatorsMenuItem.IsEnabled = true;
                m_viewTransformersMenuItem.IsEnabled = true;
            }
            if (m_measurementsAreMapped)
            {
                m_determineActiveCurrentFlowsMenuItem.IsEnabled = true;
                m_viewReceivedMeasurementsMenuItem.IsEnabled = true;
                m_viewUnreceivedMeasurementsMenuItem.IsEnabled = true;

                m_viewModeledVoltagesMenuItem.IsEnabled = true;
                m_viewExpectedVoltagesMenuItem.IsEnabled = true;
                m_viewActiveVoltagesMenuItem.IsEnabled = true;
                m_viewInactiveVoltagesMenuItem.IsEnabled = true;
            }
            if (m_activeCurrentFlowsHaveBeenDetermined)
            {
                m_determineActiveCurrentInjectionsMenuItem.IsEnabled = true;
                m_viewCalculatedImpedancesMenuItem.IsEnabled = true;
                m_viewModeledCurrentFlowsMenuItem.IsEnabled = true;
                m_viewExpectedCurrentFlowsMenuItem.IsEnabled = true;
                m_viewActiveCurrentFlowsMenuItem.IsEnabled = true;
                m_viewInactiveCurrentFlowsMenuItem.IsEnabled = true;
                m_viewIncludedCurrentFlowsMenuItem.IsEnabled = true;
                m_viewExcludedCurrentFlowsMenuItem.IsEnabled = true;

            }
            if (m_activeCurrentInjectionsHaveBeenDetermined)
            {
                m_resolveToObservedBusesMenuItem.IsEnabled = true;
                m_resolveToSingleFlowBranchesMenuItem.IsEnabled = true;
                m_viewModeledCurrentInjectionsMenuItem.IsEnabled = true;
                m_viewExpectedCurrentInjectionsMenuItem.IsEnabled = true;
                m_viewActiveCurrentInjectionsMenuItem.IsEnabled = true;
                m_viewInactiveCurrentInjectionsMenuItem.IsEnabled = true;
            }
            if (m_observedBussesHaveBeenResolved)
            {
                m_viewModeledVoltagesMenuItem.IsEnabled = true;
                m_viewObservableNodesMenuItem.IsEnabled = true;
                m_viewSubstationAdjacencyListMenuItem.IsEnabled = true;
            }
            if (m_singleFlowBranchesHaveBeenResolved)
            {
                m_viewTransmissionLineAdjacencyListMenuItem.IsEnabled = true;
                m_viewSeriesCompensatorInferenceDataMenuItem.IsEnabled = true;
            }
            if (m_observedBussesHaveBeenResolved && m_singleFlowBranchesHaveBeenResolved)
            {
                m_viewAMatrixMenuItem.IsEnabled = true;
                m_viewIIMatrixMenuItem.IsEnabled = true;
                m_viewYMatrixMenuItem.IsEnabled = true;
                m_viewYsMatrixMenuItem.IsEnabled = true;
                m_viewYshMatrixMenuItem.IsEnabled = true;
                m_computeSystemStateMenuItem.IsEnabled = true;
            }
            if (m_stateWasComputed)
            {
                m_computeLineFlowsMenuItem.IsEnabled = true;
                m_computeInjectionsMenuItem.IsEnabled = true;
                m_computePowerFlowsMenuItem.IsEnabled = true;
                m_viewOutputMeasurementsMenuItem.IsEnabled = true;
                if (m_network.PhaseConfiguration == PhaseSelection.ThreePhase)
                {
                    m_computeSequenceComponentsMenuItem.IsEnabled = true;
                }
            }
            OnPropertyChanged("MenuBarItems");
        }

        #endregion

        #region [ Disable Controls ]

        private void DisableControls()
        {
            DisableSetupMenuBarControls();
            DisableObservabilityAnalysisMenuBarControls();
            DisableMatricesMenuBarControls();
            DisableComputationMenuBarControls();
            DisableInspectMenuBarControls();
            OnPropertyChanged("MenuBarItems");
        }

        private void DisableSetupMenuBarControls()
        {
            m_initializeModelMenuItem.IsEnabled = false;
            m_saveNetworkSnapshotMenuItem.IsEnabled = false;
            m_mapMeasurementsMenuItem.IsEnabled = false;
            m_viewReceivedMeasurementsMenuItem.IsEnabled = false;
            m_viewUnreceivedMeasurementsMenuItem.IsEnabled = false;
            m_viewComponentSummaryMenuItem.IsEnabled = false;
        }

        private void DisableObservabilityAnalysisMenuBarControls()
        {
            m_determineActiveCurrentFlowsMenuItem.IsEnabled = false;
            m_determineActiveCurrentInjectionsMenuItem.IsEnabled = false;
            m_resolveToObservedBusesMenuItem.IsEnabled = false;
            m_resolveToSingleFlowBranchesMenuItem.IsEnabled = false;
            m_viewActiveCurrentFlowsMenuItem.IsEnabled = false;
            m_viewModeledVoltagesMenuItem.IsEnabled = false;
            m_viewObservableNodesMenuItem.IsEnabled = false;
            m_viewSubstationAdjacencyListMenuItem.IsEnabled = false;
            m_viewTransmissionLineAdjacencyListMenuItem.IsEnabled = false;
            m_viewCalculatedImpedancesMenuItem.IsEnabled = false;
        }

        private void DisableMatricesMenuBarControls()
        {
            m_viewAMatrixMenuItem.IsEnabled = false;
            m_viewIIMatrixMenuItem.IsEnabled = false;
            m_viewYMatrixMenuItem.IsEnabled = false;
            m_viewYsMatrixMenuItem.IsEnabled = false;
            m_viewYshMatrixMenuItem.IsEnabled = false;
        }

        private void DisableComputationMenuBarControls()
        {
            m_computeSystemStateMenuItem.IsEnabled = false;
            m_computeLineFlowsMenuItem.IsEnabled = false;
            m_computeInjectionsMenuItem.IsEnabled = false;
            m_computePowerFlowsMenuItem.IsEnabled = false;
            m_computeSequenceComponentsMenuItem.IsEnabled = false;
        }

        private void DisableInspectMenuBarControls()
        {
            m_viewSeriesCompensatorInferenceDataMenuItem.IsEnabled = false;
            m_viewTransformersMenuItem.IsEnabled = false;
            m_viewSeriesCompensatorsMenuItem.IsEnabled = false;
            m_viewOutputMeasurementsMenuItem.IsEnabled = false;
            m_viewExpectedVoltagesMenuItem.IsEnabled = false;
            m_viewActiveVoltagesMenuItem.IsEnabled = false;
            m_viewInactiveVoltagesMenuItem.IsEnabled = false;

            m_viewModeledCurrentFlowsMenuItem.IsEnabled = false;
            m_viewExpectedCurrentFlowsMenuItem.IsEnabled = false;
            m_viewActiveCurrentFlowsMenuItem.IsEnabled = false;
            m_viewInactiveCurrentFlowsMenuItem.IsEnabled = false;
            m_viewIncludedCurrentFlowsMenuItem.IsEnabled = false;
            m_viewExcludedCurrentFlowsMenuItem.IsEnabled = false;

            m_viewModeledCurrentInjectionsMenuItem.IsEnabled = false;
            m_viewExpectedCurrentInjectionsMenuItem.IsEnabled = false;
            m_viewActiveCurrentInjectionsMenuItem.IsEnabled = false;
            m_viewInactiveCurrentInjectionsMenuItem.IsEnabled = false;

        }

        #endregion

        public bool CanOpenFile
        {
            get
            {
                return true;
            }
        }

        public void DeleteMeasurementSample(RawMeasurements measurementSample)
        {
            m_measurementSamples.Remove(measurementSample);
            m_networkTreeViewModel.MeasurementSamples = m_measurementSamples;
            ActionStatus = $"Deleted 'Sample {measurementSample.Identifier}'";
        }

        #region [ Initializing View Model Methods ] 

        private void InitializeMenuBar()
        {
            #region [ MENU BAR --> OFFLINE ANALYSIS --> SETUP ]
            m_clearMeasurementsMenuItem = new MenuItemViewModel("Clear Measurements From Model", ClearMeasurementsFromModelCommand);
            m_initializeModelMenuItem = new MenuItemViewModel("Initialize Model", InitializeModelCommand);
            m_mapMeasurementsMenuItem = new MenuItemViewModel("Map Measurements", MapMeasurementsToModelCommand);
            m_setupMenuItem = new MenuItemViewModel("Setup", null);
            m_setupMenuItem.AddMenuItem(m_clearMeasurementsMenuItem);
            m_setupMenuItem.AddMenuItem(m_initializeModelMenuItem);
            m_setupMenuItem.AddMenuItem(m_mapMeasurementsMenuItem);
            #endregion

            #region [ MENU BAR --> OFFLINE ANALYSIS --> OBSERVABILITY ANALYSIS ]
            m_determineActiveCurrentFlowsMenuItem = new MenuItemViewModel("Determine Active Current Flows", DetermineActiveCurrentFlowsCommand);
            m_determineActiveCurrentInjectionsMenuItem = new MenuItemViewModel("Determine Active Current Injections", DetermineActiveCurrentInjectionsCommand);
            m_resolveToObservedBusesMenuItem = new MenuItemViewModel("Resolve to Observed Buses", ResolvetoObservedBusesCommand);
            m_resolveToSingleFlowBranchesMenuItem = new MenuItemViewModel("Resolve to Single Flow Branches", ResolveToSingleFlowBranchesCommand);
            m_observabilityMenuItem = new MenuItemViewModel("Observability", null);
            m_observabilityMenuItem.AddMenuItem(m_determineActiveCurrentFlowsMenuItem);
            m_observabilityMenuItem.AddMenuItem(m_determineActiveCurrentInjectionsMenuItem);
            m_observabilityMenuItem.AddMenuItem(m_resolveToObservedBusesMenuItem);
            m_observabilityMenuItem.AddMenuItem(m_resolveToSingleFlowBranchesMenuItem);
            #endregion

            #region [ MENU BAR --> OFFLINE ANALYSIS --> MATRICES ]
            m_viewAMatrixMenuItem = new MenuItemViewModel("View 'A' Matrix", ViewAMatrixCommand);
            m_viewIIMatrixMenuItem = new MenuItemViewModel("View 'II' Matrix", ViewIIMatrixCommand);
            m_viewYMatrixMenuItem = new MenuItemViewModel("View 'Y' Matrix", ViewYMatrixCommand);
            m_viewYsMatrixMenuItem = new MenuItemViewModel("View 'Ys' Matrix", ViewYsMatrixCommand);
            m_viewYshMatrixMenuItem = new MenuItemViewModel("View 'Ysh' Matrix", ViewYshMatrixCommand);
            m_matricesMenuItem = new MenuItemViewModel("Matrices", null);
            m_matricesMenuItem.AddMenuItem(m_viewAMatrixMenuItem);
            m_matricesMenuItem.AddMenuItem(m_viewIIMatrixMenuItem);
            m_matricesMenuItem.AddMenuItem(m_viewYMatrixMenuItem);
            m_matricesMenuItem.AddMenuItem(m_viewYsMatrixMenuItem);
            m_matricesMenuItem.AddMenuItem(m_viewYshMatrixMenuItem);
            #endregion

            #region [ MENU BAR --> OFFLINE ANALYSIS --> COMPUTATION ]
            m_computeSystemStateMenuItem = new MenuItemViewModel("Compute System State", ComputeSystemStateCommand);
            m_computeLineFlowsMenuItem = new MenuItemViewModel("Compute Line Flows", ComputeLineFlowsCommand);
            m_computeInjectionsMenuItem = new MenuItemViewModel("Compute Injections", ComputeInjectionsCommand);
            m_computePowerFlowsMenuItem = new MenuItemViewModel("Compute Power Flows", ComputePowerFlowsCommand);
            m_computeSequenceComponentsMenuItem = new MenuItemViewModel("Compute Sequence Components", ComputeSequenceComponentsCommand);
            m_computationMenuItem = new MenuItemViewModel("Computation", null);
            m_computationMenuItem.AddMenuItem(m_computeSystemStateMenuItem);
            m_computationMenuItem.AddMenuItem(m_computeLineFlowsMenuItem);
            m_computationMenuItem.AddMenuItem(m_computeInjectionsMenuItem);
            m_computationMenuItem.AddMenuItem(m_computePowerFlowsMenuItem);
            m_computationMenuItem.AddMenuItem(m_computeSequenceComponentsMenuItem);
            #endregion

            #region [ MENU BAR --> OFFLINE ANALYSIS --> INSPECT ]
            m_viewComponentSummaryMenuItem = new MenuItemViewModel("Component Summary", ViewComponentsCommand);
            m_viewReceivedMeasurementsMenuItem = new MenuItemViewModel("Received Measurements", ViewReceivedMeasurementsCommand);
            m_viewUnreceivedMeasurementsMenuItem = new MenuItemViewModel("Unreceived Measurements", ViewUnreceivedMeasurementsCommand);
            m_viewOutputMeasurementsMenuItem = new MenuItemViewModel("View Output Measurements", ViewOutputMeasurementsCommand);
            m_viewModeledVoltagesMenuItem = new MenuItemViewModel("View Modeled Voltages", ViewModeledVoltagesCommand);
            m_viewExpectedVoltagesMenuItem = new MenuItemViewModel("View Expected Voltages", ViewExpectedVoltagesCommand);
            m_viewActiveVoltagesMenuItem = new MenuItemViewModel("View Active Voltages", ViewActiveVoltagesCommand);
            m_viewInactiveVoltagesMenuItem = new MenuItemViewModel("View Inactive Voltages", ViewInactiveVoltagesCommand);
            m_viewModeledCurrentFlowsMenuItem = new MenuItemViewModel("View Modeled Current Flows", ViewModeledCurrentFlowsCommand);
            m_viewExpectedCurrentFlowsMenuItem = new MenuItemViewModel("View Expected Current Flows", ViewExpectedCurrentFlowsCommand);
            m_viewActiveCurrentFlowsMenuItem = new MenuItemViewModel("View Active Current Flows", ViewActiveCurrentFlowsCommand);
            m_viewInactiveCurrentFlowsMenuItem = new MenuItemViewModel("View Inactive Current Flows", ViewInactiveCurrentFlowsCommand);
            m_viewIncludedCurrentFlowsMenuItem = new MenuItemViewModel("View Included Current Flows", ViewIncludedCurrentFlowsCommand);
            m_viewExcludedCurrentFlowsMenuItem = new MenuItemViewModel("View Excluded Current Flows", ViewExcludedCurrentFlowsCommand);
            m_viewModeledCurrentInjectionsMenuItem = new MenuItemViewModel("View Modeled Current Injections", ViewModeledCurrentInjectionsCommand);
            m_viewExpectedCurrentInjectionsMenuItem = new MenuItemViewModel("View Expected Current Injections", ViewExpectedCurrentInjectionsCommand);
            m_viewActiveCurrentInjectionsMenuItem = new MenuItemViewModel("View Active Current Injections", ViewActiveCurrentInjectionsCommand);
            m_viewInactiveCurrentInjectionsMenuItem = new MenuItemViewModel("View Inactive Current Injections", ViewInactiveCurrentInjectionsCommand);
            m_viewObservableNodesMenuItem = new MenuItemViewModel("View Observable Nodes", ViewObservableNodesCommand);
            m_viewSubstationAdjacencyListMenuItem = new MenuItemViewModel("View Substation Adjacency List", ViewSubstationAdjacencyListCommand);
            m_viewTransmissionLineAdjacencyListMenuItem = new MenuItemViewModel("View Transmission Line Adjacency List", ViewTransmissionLineAdjacencyListCommand);
            m_viewCalculatedImpedancesMenuItem = new MenuItemViewModel("View Calculated Impedances", ViewCalculatedImpedancesCommand);
            m_viewSeriesCompensatorInferenceDataMenuItem = new MenuItemViewModel("View Series Compensator Inference Data", m_viewSeriesCompensatorInferenceDataCommand);
            m_viewSeriesCompensatorsMenuItem = new MenuItemViewModel("View Series Compensators", ViewSeriesCompensatorsCommand);
            m_viewTransformersMenuItem = new MenuItemViewModel("View Transformers", ViewTransformersCommand);
            m_inspectMenuItem = new MenuItemViewModel("Inspect", null);
            m_inspectMenuItem.AddMenuItem(m_viewComponentSummaryMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewReceivedMeasurementsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewUnreceivedMeasurementsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewOutputMeasurementsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewModeledVoltagesMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewExpectedVoltagesMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewActiveVoltagesMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewInactiveVoltagesMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewModeledCurrentFlowsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewExpectedCurrentFlowsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewActiveCurrentFlowsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewInactiveCurrentFlowsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewIncludedCurrentFlowsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewExcludedCurrentFlowsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewObservableNodesMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewSubstationAdjacencyListMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewTransmissionLineAdjacencyListMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewCalculatedImpedancesMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewSeriesCompensatorInferenceDataMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewSeriesCompensatorsMenuItem);
            m_inspectMenuItem.AddMenuItem(m_viewTransformersMenuItem);
            #endregion

            #region [ MENU BAR --> OFFLINE ANALYSIS ]
            m_offlineAnalysisMenuItem = new MenuItemViewModel("Offline Analysis", null);
            m_offlineAnalysisMenuItem.AddMenuItem(m_setupMenuItem);
            m_offlineAnalysisMenuItem.AddMenuItem(m_observabilityMenuItem);
            m_offlineAnalysisMenuItem.AddMenuItem(m_matricesMenuItem);
            m_offlineAnalysisMenuItem.AddMenuItem(m_computationMenuItem);
            m_offlineAnalysisMenuItem.AddMenuItem(m_inspectMenuItem);
            #endregion

            #region [ MENU BAR --> UTILITIES --> MODEL ACTIONS ]
            m_unkeyifyModelMenuItem = new MenuItemViewModel("Unkeyify Model", UnkeyifyModelCommand);
            m_pruneModelMenuItem = new MenuItemViewModel("Prune Model", PruneModelCommand);
            m_modelActionsMenuItem = new MenuItemViewModel("Model Actions", null);
            m_modelActionsMenuItem.AddMenuItem(m_unkeyifyModelMenuItem);
            m_modelActionsMenuItem.AddMenuItem(m_pruneModelMenuItem);
            #endregion

            #region [ MENU BAR --> UTILITIES --> MEASUREMENT ACTIONS ]
            m_generateMeasurementSamplesMenuItem = new MenuItemViewModel("Generate Measurement Samples From CSV", GenerateMeasurementSamplesCommand);
            m_measurementActionsMenuItem = new MenuItemViewModel("Measurement Actions", null);
            m_measurementActionsMenuItem.AddMenuItem(m_generateMeasurementSamplesMenuItem);
            #endregion

            #region [ MENU BAR --> UTILITIES ]
            m_utilitiesMenuItem = new MenuItemViewModel("Utilities", null);
            m_utilitiesMenuItem.AddMenuItem(m_modelActionsMenuItem);
            m_utilitiesMenuItem.AddMenuItem(m_measurementActionsMenuItem);
            #endregion

            #region [ MENU BAR --> FILE --> OPEN ]
            m_openNetworkModelMenuItem = new MenuItemViewModel("Xml Network Model", OpenFileCommand);
            m_openMeasurementSampleMenuItem = new MenuItemViewModel("Xml Measurement Sample", OpenMeasurementSampleFileCommand);
            m_openPsseRawFileMenuItem = new MenuItemViewModel("PSSE *.raw File", OpenPsseRawFileCommand);
            m_openHdbExportFilesMenuItem = new MenuItemViewModel("Hdb Export List File", OpenHdbExportFilesCommand);
            m_openMenuItem = new MenuItemViewModel("Open", null);
            m_openMenuItem.AddMenuItem(m_openNetworkModelMenuItem);
            m_openMenuItem.AddMenuItem(m_openMeasurementSampleMenuItem);
            m_openMenuItem.AddMenuItem(m_openHdbExportFilesMenuItem);
            m_openMenuItem.AddMenuItem(m_openPsseRawFileMenuItem);
            #endregion

            #region [ MENU BAR --> FILE --> SAVE ]
            m_saveNetworkModelMenuItem = new MenuItemViewModel("Xml Network Model", SaveFileCommand);
            m_saveNetworkSnapshotMenuItem = new MenuItemViewModel("Xml Network Snapshot", SaveNetworkSnapshotFileCommand);
            m_saveMeasurementSampleFileMenuItem = new MenuItemViewModel("Xml Measurement Sample", SaveMeasurementSampleFilesCommand);
            m_saveMenuItem = new MenuItemViewModel("Save", null);
            m_saveMenuItem.AddMenuItem(m_saveNetworkModelMenuItem);
            m_saveMenuItem.AddMenuItem(m_saveNetworkSnapshotMenuItem);
            m_saveMenuItem.AddMenuItem(m_saveMeasurementSampleFileMenuItem);
            #endregion

            #region [ MENU BAR --> FILE --> EXIT ]
            m_exitMenuItem = new MenuItemViewModel("Exit", null);
            #endregion

            #region [ MENU BAR --> FILE ]
            m_fileMenuItem = new MenuItemViewModel("File", null);
            m_fileMenuItem.AddMenuItem(m_openMenuItem);
            m_fileMenuItem.AddMenuItem(m_saveMenuItem);
            m_fileMenuItem.AddMenuItem(m_exitMenuItem);
            #endregion

            #region [ MENU BAR ]
            m_menuBarItemViewModels = new List<MenuItemViewModel>();
            m_menuBarItemViewModels.Add(m_fileMenuItem);
            m_menuBarItemViewModels.Add(m_utilitiesMenuItem);
            m_menuBarItemViewModels.Add(m_offlineAnalysisMenuItem);
            #endregion

        }

        private void InitializeNetworkTree()
        {
            m_network = new Network();
            m_network.Initialize();
            m_network.Model = new NetworkModel();
            m_measurementSamples = new List<RawMeasurements>();
            m_networkTreeViewModel = new NetworkTreeViewModel(this, m_network, m_measurementSamples);
        }
        
        private void InitializeRecordDetail()
        {
            m_recordDetailViewModel = new RecordDetailViewModel(this);
        }

        #endregion

        #region [ Commands ]

        #region [ File Opening ] 

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "Network Model (.xml)|*.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    m_network = Network.DeserializeFromXml(openFileDialog.FileName);
                    ActionStatus = $"Opened network model from {openFileDialog.FileName}";

                    m_networkTreeViewModel.Network = m_network;
                    EnableControls();
                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to load selected file.");
                    }
                }
            }
        }

        private void OpenHdbExportFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "Hdb Export List File (.xml)|*.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    m_network = Network.FromHdbExport(openFileDialog.FileName, true);
                    ActionStatus = $"Opened network model from {openFileDialog.FileName}";

                    m_networkTreeViewModel.Network = m_network;
                    EnableControls();
                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to load selected file.");
                    }
                }
            }
        }

        private void OpenPsseRawFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".raw";
            openFileDialog.Filter = "PSSE (.raw)|*.raw";
            EnableControls();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    RawFile rawFile = RawFile.Read(openFileDialog.FileName);
                    m_network = Network.FromPsseRawFile(openFileDialog.FileName, rawFile.Version.ToString());
                    ActionStatus = $"Opened network model from {openFileDialog.FileName}";

                    m_networkTreeViewModel.Network = m_network;
                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to load selected file.");
                    }
                }
            }
        }

        private void OpenMeasurementSampleFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "Measurement Sample (.xml)|*.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    m_measurementSamples.Add(RawMeasurements.DeserializeFromXml(openFileDialog.FileName));

                    m_networkTreeViewModel.MeasurementSamples = m_measurementSamples;
                    ActionStatus = $"Opened measurement sample from {openFileDialog.FileName}";
                    EnableControls();

                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to load selected file.");
                    }
                }
            }
        }
        
        #endregion

        #region [ User Interface ]

        private void ChangeSelectedElement()
        {
            System.Windows.MessageBox.Show("Selection Changed");
        }

        private void ViewDetail()
        {
            m_recordDetailViewModel.ClearRecordDetailView();
            m_recordDetailViewModel.AddViewModel(m_networkTreeViewModel.SelectedElement.Value);
        }

        private void RefreshNetworkTree()
        {
            m_networkTreeViewModel.RefreshNetworkTree();
        }

        #endregion

        #region [ File Savings ] 

        private void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "Network Model (.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    m_network.Initialize();
                    m_network.SerializeToXml(saveFileDialog.FileName);
                    ActionStatus = $"Saved network model to {saveFileDialog.FileName}";
                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to save xml file.");
                    }
                }
            }
        }

        private void SaveMeasurementSampleFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "Measurement Sample (.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (m_networkTreeViewModel.SelectedElement.Value.Element is RawMeasurements)
                    {
                        // Create an XmlSerializer with the type of Network
                        XmlSerializer serializer = new XmlSerializer(typeof(RawMeasurements));

                        // Open a connection to the file and path.
                        TextWriter writer = new StreamWriter(saveFileDialog.FileName);

                        // Serialize this instance of NetworkMeasurements
                        serializer.Serialize(writer, (m_networkTreeViewModel.SelectedElement.Value.Element as RawMeasurements));

                        // Close the connection
                        writer.Close();
                        ActionStatus = $"Saved measurement sample to {saveFileDialog.FileName}";
                    }                    
                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to save xml file.");
                    }
                }
            }
        }

        private void SaveNetworkSnapshotFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "Network Model Snapshot (.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    m_network.SerializeData(true);
                    m_network.SerializeToXml(saveFileDialog.FileName);
                    ActionStatus = $"Saved network model snapshot to {saveFileDialog.FileName}";
                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to save xml file.");
                    }
                }
            }
        }

        #endregion
        
        private void UnkeyifyModel()
        {
            if (m_network != null && m_network.Model != null)
            {
                m_network.Model.Unkeyify();
                ActionStatus = "Set all keys to 'Undefined'";
            }
        }
        
        private void GenerateMeasurementSamplesFromCsv()
        {
            string columnMappingFile = null;
            string csvFile = null;

            OpenFileDialog columnMappingOpenFileDialog = new OpenFileDialog();

            columnMappingOpenFileDialog.DefaultExt = ".txt";
            columnMappingOpenFileDialog.Filter = "Column Mapping File (.txt)|*.txt";

            if (columnMappingOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    columnMappingFile = columnMappingOpenFileDialog.FileName;
                    ActionStatus = "Selected column mapping file.";

                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to load selected file.");
                    }
                }
            }

            OpenFileDialog csvOpenFileDialog = new OpenFileDialog();

            csvOpenFileDialog.DefaultExt = ".csv";
            csvOpenFileDialog.Filter = "Column Mapping File (.csv)|*.csv";

            if (csvOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    csvFile = csvOpenFileDialog.FileName;
                    ActionStatus = "Selected csv file.";

                }
                catch (Exception exception)
                {
                    if (exception != null)
                    {
                        System.Windows.MessageBox.Show(exception.ToString(), "Failed to load selected file.");
                    }
                }
            }


            if (csvFile != null && columnMappingFile != null)
            {
                List<RawMeasurements> measurementSamples = RawMeasurements.FromCsv(csvFile, columnMappingFile, true);

                foreach (RawMeasurements sample in measurementSamples)
                {
                    m_measurementSamples.Add(sample);
                }
                ActionStatus = $"Created {m_measurementSamples.Count} measurement samples from CSV file";
                m_networkTreeViewModel.MeasurementSamples = m_measurementSamples;
            }

        }
        
        private void PruneModel()
        {
            bool wasInPruningMode = m_network.Model.InPruningMode;
            m_network.Model.InPruningMode = true;

            m_network.Model.DetermineActiveCurrentFlows();
            m_network.Model.DetermineActiveCurrentInjections();
            m_network.Model.ResolveToObservedBuses();
            m_network.Model.ResolveToSingleFlowBranches();

            m_network.Model.Prune();
            m_network.Model.InPruningMode = wasInPruningMode;
        }

        private void EnableInferredStateAsActualProxy()
        {
            m_network.Model.InPruningMode = false;
            m_network.Model.EnableInferredStateAsActualProxy();
        }

        #region [ Setup ]

        private void SelectMeasurementSample()
        {
            SelectedMeasurementSample = m_networkTreeViewModel.SelectedElement.Value.Element as RawMeasurements;
            EnableControls();
            ActionStatus = "User selected a measurement sample.";
        }

        private void InitializeModel()
        {
            if (m_network != null && m_network.Model != null)
            {
                m_network.Initialize();
                m_networkIsInitialized = true;
                EnableInferredStateAsActualProxy();
                EnableControls();
                ActionStatus = "User initialized the network model";
            }
        }
        
        private void ClearMeasurementsFromModel()
        {
            if (m_network != null && m_network.Model != null)
            {
                m_network.Model.ClearValues();
                m_network.Model.InputKeyValuePairs = new Dictionary<string, double>();
                m_networkIsInitialized = false;
                m_measurementsAreMapped = false;
                m_activeCurrentFlowsHaveBeenDetermined = false;
                m_observedBussesHaveBeenResolved = false;
                m_stateWasComputed = false;
                DisableControls();
                ActionStatus = "User cleared measurement values from network model.";
            }
        }

        private void MapMeasurementsToModel()
        {
            try
            {
                foreach (RawMeasurementsMeasurement measurement in SelectedMeasurementSample.Items)
                {
                    m_network.Model.InputKeyValuePairs.Add(measurement.Key, Convert.ToDouble(measurement.Value));
                    CommunicationStatus = $"Mapping measurement {measurement.Key} with value {measurement.Value}";
                }
                CommunicationStatus = "";
                m_network.Model.OnNewMeasurements();

                m_measurementsAreMapped = true;
                EnableControls();
                ActionStatus = "Measurements succcessfully mapped to network model.";
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        #endregion

        #region [ Observability Analysis ] 

        private void DetermineActiveCurrentFlows()
        {
            try
            {
                m_network.Model.DetermineActiveCurrentFlows();
                m_activeCurrentFlowsHaveBeenDetermined = true;
                ActionStatus = "Active current flow phasors determined successfully.";
                EnableControls();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void DetermineActiveCurrentInjections()
        {
            try
            {
                m_network.Model.DetermineActiveCurrentInjections();
                m_activeCurrentInjectionsHaveBeenDetermined = true;
                ActionStatus = "Active current injection phasors determined successfully.";
                EnableControls();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ResolveToObservedBuses()
        {
            try
            {
                m_network.Model.ResolveToObservedBuses();
                m_observedBussesHaveBeenResolved = true;
                ActionStatus = "Observability analysis resolved nodes to observed nodes.";
                EnableControls();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ResolveToSingleFlowBranches()
        {
            try
            {
                m_network.Model.ResolveToSingleFlowBranches();
                m_singleFlowBranchesHaveBeenResolved = true;
                ActionStatus = "Observability analysis resolved transmission lines to single flow branches.";
                EnableControls();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        #endregion

        #region [ Matrices ]

        private void ViewAMatrix()
        {
            try
            {
                CurrentFlowMeasurementBusIncidenceMatrix A = new CurrentFlowMeasurementBusIncidenceMatrix(m_network);

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendFormat(A.ToCsvString());

                File.WriteAllText(Directory.GetCurrentDirectory() + "/A Matrix.csv", stringBuilder.ToString());
                ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/A Matrix.csv";
                Process.Start(Directory.GetCurrentDirectory() + "/A Matrix.csv");
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ViewIIMatrix()
        {
            try
            {
                VoltageMeasurementBusIncidenceMatrix II = new VoltageMeasurementBusIncidenceMatrix(m_network);

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendFormat(II.ToCsvString());

                File.WriteAllText(Directory.GetCurrentDirectory() + "/II Matrix.csv", stringBuilder.ToString());
                ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/II Matrix.csv";
                Process.Start(Directory.GetCurrentDirectory() + "/II Matrix.csv");
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ViewYMatrix()
        {
            try
            {
                SeriesAdmittanceMatrix Y = new SeriesAdmittanceMatrix(m_network);

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendFormat(Y.ToCsvString());

                File.WriteAllText(Directory.GetCurrentDirectory() + "/Y Matrix.csv", stringBuilder.ToString());
                ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/Y Matrix.csv";
                Process.Start(Directory.GetCurrentDirectory() + "/Y Matrix.csv");
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ViewYsMatrix()
        {
            try
            {
                LineShuntSusceptanceMatrix Ys = new LineShuntSusceptanceMatrix(m_network);

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendFormat(Ys.ToCsvString());

                File.WriteAllText(Directory.GetCurrentDirectory() + "/Ys Matrix.csv", stringBuilder.ToString());
                ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/Ys Matrix.csv";
                Process.Start(Directory.GetCurrentDirectory() + "/Ys Matrix.csv");
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ViewYshMatrix()
        {
            try
            {
                ShuntDeviceSusceptanceMatrix Ysh = new ShuntDeviceSusceptanceMatrix(m_network);

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendFormat(Ysh.ToCsvString());

                File.WriteAllText(Directory.GetCurrentDirectory() + "/Ysh Matrix.csv", stringBuilder.ToString());
                ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/Ysh Matrix.csv";
                Process.Start(Directory.GetCurrentDirectory() + "/Ysh Matrix.csv");
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }
        
        #endregion

        #region [ Computation ]

        private void ComputeSystemState()
        {
            try
            {
                m_network.ComputeSystemState();
                m_stateWasComputed = true;
                ActionStatus = "System state computed successfully.";
                EnableControls();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ComputeLineFlows()
        {
            try
            {
                m_network.Model.ComputeEstimatedCurrentFlows();
                ActionStatus = "Estimated line flows computed successfully.";
                EnableControls();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ComputeInjections()
        {
            try
            {
                m_network.Model.ReturnsCurrentInjection = true;
                m_network.Model.ComputeEstimatedCurrentInjections();
                ActionStatus = "Estimated injections computed successfully.";
                EnableControls();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        private void ComputePowerFlows()
        {
            System.Windows.MessageBox.Show("This button has not yet been implemented", "Oh, snap!");
        }

        private void ComputeSequenceComponents()
        {
            try
            {
                m_network.Model.ComputeSequenceValues();
                ActionStatus = "Sequence values computed successfully.";
                EnableControls();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.ToString(), "Error!");
            }
        }

        #endregion

        #region [ Inpsect ] 

        private void ViewComponents()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(m_network.Model.ComponentList());
            File.WriteAllText(Directory.GetCurrentDirectory() + "/ComponentList.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ComponentList.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ComponentList.txt", stringBuilder.ToString());
        }

        private void ViewReceivedMeasurements()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Received Measurements Key Value Pairs");
            Dictionary<string, double> receivedMeasurements = m_network.Model.GetReceivedMeasurements();
            foreach (KeyValuePair<string, double> keyValuePair in receivedMeasurements)
            {
                stringBuilder.AppendLine(keyValuePair.Key + ", " + keyValuePair.Value.ToString());
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/ReceivedMeasurements.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ReceivedMeasurements.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ReceivedMeasurements.txt");
        }

        private void ViewUnreceivedMeasurements()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Unreceived Measurements Key Value Pairs");
            Dictionary<string, double> receivedMeasurements = m_network.Model.GetReceivedMeasurements();
            foreach (KeyValuePair<string, double> keyValuePair in receivedMeasurements)
            {
                double value = 0;
                if (!m_network.Model.InputKeyValuePairs.TryGetValue(keyValuePair.Key, out value))
                {
                    stringBuilder.AppendLine(keyValuePair.Key + ", " + keyValuePair.Value.ToString());
                }
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/UnreceivedMeasurements.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/UnreceivedMeasurements.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/UnreceivedMeasurements.txt");
        }

        private void ViewObservableNodes()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.ObservedBusses.Count.ToString() + " Observed Busses");
            foreach (ObservedBus observedBus in m_network.Model.ObservedBusses)
            {
                stringBuilder.AppendFormat(observedBus.ToVerboseString() + "{0}", Environment.NewLine);
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "/ObservedBusses.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ObservedBusses.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ObservedBusses.txt");
        }
        
        private void ViewSubstationAdjacencyList()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Company company in m_network.Model.Companies)
            {
                foreach (Division division in company.Divisions)
                {
                    foreach (Substation substation in division.Substations)
                    {
                        stringBuilder.AppendFormat(substation.ToString() + "{0}", Environment.NewLine);
                        stringBuilder.AppendFormat(substation.Graph.AdjacencyList.ToString() + "{0}", Environment.NewLine);
                        stringBuilder.AppendLine();
                    }
                }
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "/SubstationAdjacencyLists.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/SubstationAdjacencyLists.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/SubstationAdjacencyLists.txt");
        }
        
        private void ViewTransmissionLineAdjacencyLists()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (TransmissionLine transmissionLine in m_network.Model.TransmissionLines)
            {
                stringBuilder.AppendFormat(transmissionLine.ToVerboseString() + "{0}", Environment.NewLine);
                stringBuilder.AppendFormat("Directly Connected Adjacency List{0}", Environment.NewLine);
                stringBuilder.AppendFormat(transmissionLine.Graph.DirectlyConnectedAdjacencyList.ToString() + "{0}", Environment.NewLine);
                stringBuilder.AppendFormat("Series Impedance Connected Adjacency List{0}", Environment.NewLine);
                stringBuilder.AppendFormat(transmissionLine.Graph.SeriesImpedanceConnectedAdjacencyList.ToString() + "{0}", Environment.NewLine);
                stringBuilder.AppendFormat("Transmission Line Tree{0}", Environment.NewLine);
                stringBuilder.AppendFormat(transmissionLine.Graph.RootNode.ToSubtreeString() + "{0}", Environment.NewLine);
                stringBuilder.AppendFormat("Single Flow Branch Resolution{0}", Environment.NewLine);
                stringBuilder.AppendFormat("Number of Possible Impedance States: " + transmissionLine.NumberOfPossibleSeriesImpedanceStates.ToString() + "{0}", Environment.NewLine);
                stringBuilder.AppendFormat("Possible Impedance States:{0}", Environment.NewLine);

                foreach (Impedance impedance in transmissionLine.PossibleImpedanceValues)
                {
                    stringBuilder.AppendFormat(impedance.ToVerboseString() + "{0}", Environment.NewLine);
                }
                stringBuilder.AppendFormat(transmissionLine.Graph.ResolveToSingleSeriesBranch().RawImpedanceParameters.ToVerboseString() + "{0}", Environment.NewLine);
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/TransmissionLineAdjacencyLists.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/TransmissionLineAdjacencyLists.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/TransmissionLineAdjacencyLists.txt");
        }
        
        private void ViewCalculatedImpedances()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (TransmissionLine transmissionLine in m_network.Model.TransmissionLines)
            {
                stringBuilder.AppendFormat(transmissionLine.ToVerboseString() + "{0}", Environment.NewLine);
                stringBuilder.AppendLine("Modeled Impedance Values");
                stringBuilder.AppendFormat(transmissionLine.Graph.ResolveToSingleSeriesBranch().RawImpedanceParameters.ToVerboseString() + "{0}", Environment.NewLine);
                stringBuilder.AppendLine("Calculated Impedance Values");
                stringBuilder.AppendFormat(transmissionLine.RealTimeCalculatedImpedance.ToVerboseString() + "{0}", Environment.NewLine);
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/CalculatedImpedances.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/CalculatedImpedances.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/CalculatedImpedances.txt");
        }
        
        private void ViewSeriesCompensatorInferenceData()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (TransmissionLine transmissionLine in m_network.Model.TransmissionLines)
            {
                if (transmissionLine.WillPerformSeriesCompensatorStatusInference)
                {
                    stringBuilder.AppendFormat(transmissionLine.ToVerboseString() + "{0}", Environment.NewLine);
                    stringBuilder.AppendFormat("Directly Connected Adjacency List{0}", Environment.NewLine);
                    stringBuilder.AppendFormat(transmissionLine.Graph.DirectlyConnectedAdjacencyList.ToString() + "{0}", Environment.NewLine);
                    stringBuilder.AppendFormat("Series Impedance Connected Adjacency List{0}", Environment.NewLine);
                    stringBuilder.AppendFormat(transmissionLine.Graph.SeriesImpedanceConnectedAdjacencyList.ToString() + "{0}", Environment.NewLine);
                    stringBuilder.AppendFormat("Transmission Line Tree{0}", Environment.NewLine);
                    stringBuilder.AppendFormat(transmissionLine.Graph.RootNode.ToSubtreeString() + "{0}", Environment.NewLine);
                    stringBuilder.AppendFormat("Number of Possible Impedance States: " + transmissionLine.NumberOfPossibleSeriesImpedanceStates.ToString() + "{0}", Environment.NewLine);
                    stringBuilder.AppendFormat("Possible Impedance States:{0}", Environment.NewLine);
                    foreach (Impedance impedance in transmissionLine.PossibleImpedanceValues)
                    {
                        stringBuilder.AppendFormat(impedance.ToVerboseString() + "{0}", Environment.NewLine);
                    }
                    stringBuilder.AppendFormat("Inferred Total Impedance:{0}", Environment.NewLine);
                    stringBuilder.AppendFormat(transmissionLine.InferredTotalImpedance.ToVerboseString());
                    stringBuilder.AppendFormat("Single Flow Branch Resolution:{0}", Environment.NewLine);
                    stringBuilder.AppendFormat(transmissionLine.Graph.ResolveToSingleSeriesBranch().RawImpedanceParameters.ToVerboseString() + "{0}", Environment.NewLine);
                    stringBuilder.AppendFormat("Asserted Total Branch Impedance:{0}", Environment.NewLine);
                    stringBuilder.AppendFormat(transmissionLine.FromSideImpedanceToDeepestObservability.ToVerboseString() + "{0}", Environment.NewLine);
                }

            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/SeriesCompensatorStatusInferencingData.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/SeriesCompensatorStatusInferencingData.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/SeriesCompensatorStatusInferencingData.txt");
        }
        
        private void ViewSeriesCompensators()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (TransmissionLine transmissionLine in m_network.Model.TransmissionLines)
            {
                if (transmissionLine.WillPerformSeriesCompensatorStatusInference)
                {
                    foreach (SeriesCompensator seriesCompensator in transmissionLine.SeriesCompensators)
                    {
                        stringBuilder.AppendFormat(seriesCompensator.ToVerboseString() + "{0}", Environment.NewLine);
                    }
                }
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/SeriesCompensators.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/SeriesCompensators.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/SeriesCompensators.txt");
        }
        
        private void ViewTransformers()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Transformer transformer in m_network.Model.Transformers)
            {
                stringBuilder.AppendFormat(transformer.ToVerboseString() + "{0}", Environment.NewLine);
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/Transformers.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/Transformers.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/Transformers.txt");
        }

        private void ViewOutputMeasurements()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.OutputKeyValuePairs.Count.ToString() + " Output Measurements");

            foreach (KeyValuePair<string, double> outputMeasurement in m_network.Model.OutputKeyValuePairs)
            {
                stringBuilder.AppendFormat(outputMeasurement.Key + "\t" + outputMeasurement.Value.ToString() + "{0}", Environment.NewLine);
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "/OutputMeasurements.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/OutputMeasurements.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/OutputMeasurements.txt");
        }

        private void ViewModeledVoltages()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.Voltages.Count.ToString() + " Modeled Voltage Phasors");
            foreach (VoltagePhasorGroup voltagePhasorGroup in m_network.Model.Voltages)
            {
                stringBuilder.AppendFormat(voltagePhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "/ModeledVoltages.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ModeledVoltages.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ModeledVoltages.txt");
        }

        private void ViewExpectedVoltages()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.ExpectedVoltages.Count.ToString() + " Expected Voltage Phasors");
            foreach (VoltagePhasorGroup voltagePhasorGroup in m_network.Model.ExpectedVoltages)
            {
                stringBuilder.AppendFormat(voltagePhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "/ExpectedVoltages.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ExpectedVoltages.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ExpectedVoltages.txt");
        }

        private void ViewActiveVoltages()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.ActiveVoltages.Count.ToString() + " Active Voltage Phasors");
            foreach (VoltagePhasorGroup voltagePhasorGroup in m_network.Model.ActiveVoltages)
            {
                stringBuilder.AppendFormat(voltagePhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "/ActiveVoltages.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ActiveVoltages.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ActiveVoltages.txt");
        }

        private void ViewInactiveVoltages()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Inactive Voltage Phasors");
            foreach (VoltagePhasorGroup voltagePhasorGroup in m_network.Model.ExpectedVoltages)
            {
                if (!m_network.Model.ActiveVoltages.Contains(voltagePhasorGroup))
                {
                    stringBuilder.AppendFormat(voltagePhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
                }
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/InactiveVoltages.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/InactiveVoltages.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/InactiveVoltages.txt");
        }
        
        private void ViewModeledCurrentFlows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.CurrentFlows.Count.ToString() + " Modeled Current Flow Phasors");
            foreach (CurrentFlowPhasorGroup currentFlowPhasorGroup in m_network.Model.CurrentFlows)
            {
                stringBuilder.AppendFormat(currentFlowPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/ModeledCurrentFlows.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ModeledCurrentFlows.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ModeledCurrentFlows.txt");
        }

        private void ViewExpectedCurrentFlows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.ExpectedCurrentFlows.Count.ToString() + " Expected Current Flow Phasors");
            foreach (CurrentFlowPhasorGroup currentFlowPhasorGroup in m_network.Model.ExpectedCurrentFlows)
            {
                stringBuilder.AppendFormat(currentFlowPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/ExpectedCurrentFlows.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ExpectedCurrentFlows.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ExpectedCurrentFlows.txt");
        }

        private void ViewInactiveCurrentFlows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Inactive Current Flow Phasors");
            foreach (CurrentFlowPhasorGroup currentFlowPhasorGroup in m_network.Model.ExpectedCurrentFlows)
            {
                if (!m_network.Model.ActiveCurrentFlows.Contains(currentFlowPhasorGroup))
                {
                    stringBuilder.AppendFormat(currentFlowPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
                }
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/InactiveCurrentFlows.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/InactiveCurrentFlows.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/InactiveCurrentFlows.txt");
        }

        private void ViewIncludedCurrentFlows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.IncludedCurrentFlows.Count.ToString() + " Included Current Flow Phasors");
            foreach (CurrentFlowPhasorGroup currentPhasorGroup in m_network.Model.IncludedCurrentFlows)
            {
                stringBuilder.AppendFormat(currentPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "/IncludedCurrentFlows.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/IncludedCurrentFlows.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/IncludedCurrentFlows.txt");
        }

        private void ViewExcludedCurrentFlows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Excluded Current Flow Phasors");
            foreach (CurrentFlowPhasorGroup currentFlowPhasorGroup in m_network.Model.IncludedCurrentFlows)
            {
                if (!m_network.Model.ActiveCurrentFlows.Contains(currentFlowPhasorGroup))
                {
                    stringBuilder.AppendFormat(currentFlowPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
                }
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/ExcludedCurrentFlows.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ExcludedCurrentFlows.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ExcludedCurrentFlows.txt");
        }
        
        private void ViewActiveCurrentFlows()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.ActiveCurrentFlows.Count.ToString() + " Active Current Flow Phasors");
            foreach (CurrentFlowPhasorGroup currentPhasorGroup in m_network.Model.ActiveCurrentFlows)
            {
                stringBuilder.AppendFormat(currentPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + "/ActiveCurrentFlows.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ActiveCurrentFlows.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ActiveCurrentFlows.txt");
        }

        private void ViewModeledCurrentInjections()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.CurrentInjections.Count.ToString() + " Modeled Current Injections Phasors");
            foreach (CurrentInjectionPhasorGroup currentInjectionPhasorGroup in m_network.Model.CurrentInjections)
            {
                stringBuilder.AppendFormat(currentInjectionPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/ModeledCurrentInjections.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ModeledCurrentInjections.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ModeledCurrentInjections.txt");
        }

        private void ViewExpectedCurrentInjections()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.ExpectedCurrentInjections.Count.ToString() + " Expected Current Injection Phasors");
            foreach (CurrentInjectionPhasorGroup currentInjectionPhasorGroup in m_network.Model.ExpectedCurrentInjections)
            {
                stringBuilder.AppendFormat(currentInjectionPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/ExpectedCurrentInjections.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ExpectedCurrentInjections.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ExpectedCurrentInjections.txt");
        }

        private void ViewActiveCurrentInjections()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(m_network.Model.ActiveCurrentInjections.Count.ToString() + " Active Current Injection Phasors");
            foreach (CurrentInjectionPhasorGroup currentInjectionPhasorGroup in m_network.Model.ActiveCurrentInjections)
            {
                stringBuilder.AppendFormat(currentInjectionPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/ActiveCurrentInjections.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/ActiveCurrentInjections.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/ActiveCurrentInjections.txt");
        }

        private void ViewInactiveCurrentInjections()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Inactive Current Injection Phasors");
            foreach (CurrentInjectionPhasorGroup currentInjectionPhasorGroup in m_network.Model.ExpectedCurrentInjections)
            {
                if (!m_network.Model.ActiveCurrentInjections.Contains(currentInjectionPhasorGroup))
                {
                    stringBuilder.AppendFormat(currentInjectionPhasorGroup.ToVerboseString() + "{0}", Environment.NewLine);
                }
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + "/InactiveCurrentInjections.txt", stringBuilder.ToString());
            ActionStatus = "Wrote to " + Directory.GetCurrentDirectory() + "/InactiveCurrentInjections.txt";
            Process.Start(Directory.GetCurrentDirectory() + "/InactiveCurrentInjections.txt");
        }


        #endregion

        #endregion
    }
}
