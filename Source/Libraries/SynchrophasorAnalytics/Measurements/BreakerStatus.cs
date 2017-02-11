﻿//******************************************************************************************************
//  BreakerStatus.cs
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
//  07/20/2011 - Kevin D. Jones
//       Generated original version of source code.
//  06/01/2013 - Kevin D. Jones
//       Modified to include INetworkDescribable and IClearable interfaces and Xml Serialization
//  06/10/2014 - Kevin D. Jones
//       Updated XML inline documentation.
//  06/23/2014 - Kevin D. Jones
//       Added compatibility for a 16 bit binary word.
//  07/09/2014 - Kevin D. Jones
//       Added Guid
//
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SynchrophasorAnalytics.Modeling;
using SynchrophasorAnalytics.Measurements;

namespace SynchrophasorAnalytics.Measurements
{
    /// <summary>
    /// The <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> class encapsulates the contents of a digital signal where one bit represents the status of a breaker that is monitored by a PMU and brought in via C37.118.
    /// </summary>
    /// <seealso cref="LinearStateEstimator.Measurements.BreakerStatusBit"/>
    [Serializable()]
    public class BreakerStatus : INetworkDescribable, IClearable
    {
        #region [ Private Members ]

        /// <summary>
        /// INetworkDescribable fields
        /// </summary>
        private Guid m_uniqueId;
        private int m_internalID;
        private int m_number;
        private string m_name;
        private string m_description;

        private bool m_enabled;
        private bool m_breakerStatus;
        private int m_binaryValue;
        private int m_previousBinaryValue;
        private string m_inputMeasurementKey;
        private BreakerStatusBit m_bitPosition;

        /// <summary>
        /// Parent
        /// </summary>
        private CircuitBreaker m_parentCircuitBreaker;
        private int m_parentCircuitBreakerID;


        #endregion

        #region [ Properties ]

        /// <summary>
        /// A statistically unique identifier for the instance of the class.
        /// </summary>
        [XmlAttribute("Uid")]
        public Guid UniqueId
        {
            get
            {
                if (m_uniqueId == Guid.Empty)
                {
                    m_uniqueId = Guid.NewGuid();
                }
                return m_uniqueId;
            }
            set
            {
                m_uniqueId = value;
            }
        }

        /// <summary>
        /// A unique integer identifier for each <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.
        /// </summary>
        [XmlAttribute("ID")]
        public int InternalID
        {
            get 
            { 
                return m_internalID; 
            }
            set 
            { 
                m_internalID = value; 
            }
        }

        /// <summary>
        /// A descriptive number for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.
        /// </summary>
        [XmlAttribute("Number")]
        public int Number
        {
            get 
            { 
                return m_number; 
            }
            set 
            { 
                m_number = value; 
            }
        }

        /// <summary>
        /// A descriptive acronym for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>. Will always return 'CBST'.
        /// </summary>
        [XmlAttribute("Acronym")]
        public string Acronym
        {
            get 
            { 
                return "CBST"; 
            }
            set 
            { 
            }
        }

        /// <summary>
        /// A descriptive name for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name
        {
            get 
            { 
                return m_name; 
            }
            set 
            { 
                m_name = value; 
            }
        }

        /// <summary>
        /// A description of the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.
        /// </summary>
        [XmlAttribute("Description")]
        public string Description
        {
            get 
            { 
                return m_description; 
            }
            set 
            { 
                m_description = value; 
            }
        }

        /// <summary>
        /// A flag that represents whether the measurement is enabled.
        /// </summary>
        [XmlAttribute("Enabled")]
        public bool IsEnabled
        {
            get
            {
                return m_enabled;
            }
            set
            {
                m_enabled = value;
            }
        }

        /// <summary>
        /// The status of the breaker.
        /// </summary>
        [XmlAttribute("Status")]
        public bool Value
        {
            get 
            { 
                return m_breakerStatus; 
            }
            set 
            { 
                m_breakerStatus = value; 
            }
        }

        /// <summary>
        /// The binary string of integers taken from the digital measurement.
        /// </summary>
        [XmlIgnore()]
        public int BinaryValue
        {
            get
            { 
                return m_binaryValue; 
            }
            set
            {
                m_previousBinaryValue = m_binaryValue;
                m_binaryValue = value;
                string binaryString = Convert.ToString(m_binaryValue, 2);
                binaryString = AddPrefixOfZerosIfNeeded(binaryString);
                int status = 0;
                switch (m_bitPosition)
                {
                    case BreakerStatusBit.PSV49: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV49]) - 48; break;
                    case BreakerStatusBit.PSV50: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV50]) - 48; break;
                    case BreakerStatusBit.PSV51: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV51]) - 48; break;
                    case BreakerStatusBit.PSV52: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV52]) - 48; break;
                    case BreakerStatusBit.PSV53: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV53]) - 48; break;
                    case BreakerStatusBit.PSV54: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV54]) - 48; break;
                    case BreakerStatusBit.PSV55: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV55]) - 48; break;
                    case BreakerStatusBit.PSV56: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV56]) - 48; break;
                    case BreakerStatusBit.PSV57: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV57]) - 48; break;
                    case BreakerStatusBit.PSV58: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV58]) - 48; break;
                    case BreakerStatusBit.PSV59: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV59]) - 48; break;
                    case BreakerStatusBit.PSV60: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV60]) - 48; break;
                    case BreakerStatusBit.PSV61: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV61]) - 48; break;
                    case BreakerStatusBit.PSV62: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV62]) - 48; break;
                    case BreakerStatusBit.PSV63: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV63]) - 48; break;
                    case BreakerStatusBit.PSV64: status = Convert.ToInt16(binaryString[(int)BreakerStatusBit.PSV64]) - 48; break;
                    default: break;
                }

                if (status == 1)
                { 
                    m_breakerStatus = true;
                    if (m_parentCircuitBreaker != null)
                    {
                        m_parentCircuitBreaker.ActualState = SwitchingDeviceActualState.Open;
                    }
                }
                else
                { 
                    m_breakerStatus = false;
                    if (m_parentCircuitBreaker != null)
                    {
                        m_parentCircuitBreaker.ActualState = SwitchingDeviceActualState.Closed;
                    }
                }
            }
        }

        /// <summary>
        /// Indicates whether the latest measurement is different than the previous once, indicating a change in the structure of the network representation.
        /// </summary>
        [XmlIgnore()]
        public bool HasReportedChange
        {
            get
            {
                return (m_binaryValue != m_previousBinaryValue);
            }
        }

        /// <summary>
        /// The openPDC input measurement key for the digital measurement containing the breaker statuses.
        /// </summary>
        [XmlAttribute("Key")]
        public string Key
        {
            get 
            { 
                return m_inputMeasurementKey; 
            }
            set 
            { 
                m_inputMeasurementKey = value; 
            }
        }

        /// <summary>
        /// The bit position of the breaker status inside the digital measurement 
        /// specified by the <see cref="LinearStateEstimator.Measurements.BreakerStatusBit"/> enumeration. This parameter 
        /// is specified in the network configuration files using '<b>PSV58</b>', 
        /// '<b>PSV59'</b>, '<b>PSV60'</b>, '<b>PSV61</b>', '<b>PSV62</b>', 
        /// '<b>PSV63</b>',  or '<b>PSV64</b>'.
        /// </summary>
        [XmlAttribute("Bit")]
        public BreakerStatusBit BitPosition
        {
            get 
            { 
                return m_bitPosition; 
            }
            set 
            { 
                m_bitPosition = value; 
            }
        }

        /// <summary>
        /// Gets the type of the object
        /// </summary>
        [XmlIgnore()]
        public string ElementType
        {
            get 
            { 
                return this.GetType().ToString(); 
            }
        }

        /// <summary>
        /// The <see cref="LinearStateEstimator.Modeling.CircuitBreaker"/> which is monitored by this <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> signal.
        /// </summary>
        [XmlIgnore()]
        public CircuitBreaker ParentCircuitBreaker
        {
            get 
            { 
                return m_parentCircuitBreaker; 
            }
            set
            {
                m_parentCircuitBreaker = value;
                m_parentCircuitBreakerID = value.InternalID;
            }
        }

        /// <summary>
        /// The <see cref="LinearStateEstimator.Modeling.SwitchingDeviceBase.InternalID"/> of the parent <see cref="LinearStateEstimator.Modeling.CircuitBreaker"/>. Used to link references between
        /// the <see cref="LinearStateEstimator.Modeling.NetworkModel"/>
        /// </summary>
        [XmlAttribute("ParentCircuitBreaker")]
        public int ParentCircuitBreakerID
        {
            get 
            { 
                return m_parentCircuitBreakerID; 
            }
            set 
            {
                m_parentCircuitBreakerID = value; 
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// A blank constructor that initializes with default values.
        /// </summary>
        public BreakerStatus()
            :this(0, 0, "Undefined Name", "Undefined Description", "Undefined Measurement Key", BreakerStatusBit.PSV58)
        {
        }

        /// <summary>
        /// A constructor for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> class which uses the properties required by the <see cref="LinearStateEstimator.Modeling.INetworkDescribable"/> interface as well as the <see cref="LinearStateEstimator.Measurements.BreakerStatus.Key"/> and the <see cref="LinearStateEstimator.Measurements.BreakerStatusBit"/>.
        /// </summary>
        /// <param name="internalID">A unique integer identifier for each <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.</param>
        /// <param name="number">A descriptive number for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.</param>
        /// <param name="name">A descriptive name for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.</param>
        /// <param name="description">A description of the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.</param>
        /// <param name="inputMeasurementKey">The openPDC input measurement key for the digital measurement containing the breaker statuses.</param>
        /// <param name="bitPosition">The bit position of the breaker status inside the digital measurement 
        /// specified by the <see cref="LinearStateEstimator.Measurements.BreakerStatusBit"/> enumeration. This parameter 
        /// is specified in the network configuration files using '<b>PSV58</b>', 
        /// '<b>PSV59'</b>, '<b>PSV60'</b>, '<b>PSV61</b>', '<b>PSV62</b>', 
        /// '<b>PSV63</b>',  or '<b>PSV64</b>'.</param>
        public BreakerStatus(int internalID, int number, string name, string description, string inputMeasurementKey, BreakerStatusBit bitPosition)
            :this(internalID, number, name, description, inputMeasurementKey, bitPosition, 0)
        {
        }

        /// <summary>
        /// The designated constructor for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> class which uses the properties required by the <see cref="LinearStateEstimator.Modeling.INetworkDescribable"/> interface as well as the <see cref="LinearStateEstimator.Measurements.BreakerStatus.Key"/>, the <see cref="LinearStateEstimator.Measurements.BreakerStatusBit"/>, and the <see cref="LinearStateEstimator.Modeling.SwitchingDeviceBase.InternalID"/> of the parent <see cref="LinearStateEstimator.Modeling.CircuitBreaker"/>.
        /// </summary>
        /// <param name="internalID">A unique integer identifier for each <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.</param>
        /// <param name="number">A descriptive number for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.</param>
        /// <param name="name">A descriptive name for the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.</param>
        /// <param name="description">A description of the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/>.</param>
        /// <param name="inputMeasurementKey">The openPDC input measurement key for the digital measurement containing the breaker statuses.</param>
        /// <param name="bitPosition">The bit position of the breaker status inside the digital measurement 
        /// specified by the <see cref="LinearStateEstimator.Measurements.BreakerStatusBit"/> enumeration. This parameter 
        /// is specified in the network configuration files using '<b>PSV58</b>', 
        /// '<b>PSV59'</b>, '<b>PSV60'</b>, '<b>PSV61</b>', '<b>PSV62</b>', 
        /// '<b>PSV63</b>',  or '<b>PSV64</b>'.</param>
        /// <param name="parentCircuitBreakerID">The <see cref="LinearStateEstimator.Modeling.SwitchingDeviceBase.InternalID"/> of the parent <see cref="CircuitBreaker"/>.</param>
        public BreakerStatus(int internalID, int number, string name, string description, string inputMeasurementKey, BreakerStatusBit bitPosition, int parentCircuitBreakerID)
        {
            m_internalID = internalID;
            m_number = number;
            m_name = name;
            m_description = description;
            m_inputMeasurementKey = inputMeasurementKey;
            m_bitPosition = bitPosition;
            m_parentCircuitBreakerID = parentCircuitBreakerID;
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Adds zeros to the beginning of a string representation of the 
        /// digital value measurement if the length is less than maximum.
        /// </summary>
        /// <param name="digitalValue">The raw string representation of the digital value measurement containing the breaker status</param>
        /// <returns>A string representation of the digital value measurement with zeros as a prefix up to the maximum length.</returns>
        private string AddPrefixOfZerosIfNeeded(string digitalValue)
        {
            // Add leading zeros to make it sixteen characters long
            string prefixOfZeros = "";
            for (int i = 0; i < (16 - digitalValue.Length); i++) { prefixOfZeros += "0"; }
            return (prefixOfZeros + digitalValue);
        }

        #endregion

        #region [ Class Methods ]

        /// <summary>
        /// Sets the <see cref="LinearStateEstimator.Measurements.BreakerStatus.BinaryValue"/> to zero, effectively clearing the state of the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> to default.
        /// </summary>
        public void ClearValues()
        {
            m_binaryValue = 0;
        }

        /// <summary>
        /// Inserts the specified value as the binary value based on the specified input measurement key. If the key does not match then the value is not inserted. 
        /// </summary>
        /// <param name="value">The value to be inserted.</param>
        /// <param name="key">The measurement key of the value to be inserted.</param>
        /// <returns>Returns a bool representing the success of the insertion.</returns>
        public bool InsertValueForKey(double value, string key)
        {
            bool keyIsValid;

            if (key == m_inputMeasurementKey)
            {
                keyIsValid = true;
                m_binaryValue = Convert.ToInt32(value);
            }
            else
            {
                keyIsValid = false;
            }

            return keyIsValid;
        }

        /// <summary>
        /// A descriptive string representation of the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> class instance. The format is <i>BreakerStatus,internalId,number,name,description,bitPosition,enabledFlag,measurementKey,parentCircuitBreakerInternalID</i> and can be used for a rudimentary momento design pattern.
        /// </summary>
        /// <returns>A string representation of the instance of the class.</returns>
        public override string ToString()
        {
            return "BreakerStatus," + m_internalID.ToString() + "," + m_number.ToString() + "," + m_name + "," + m_name + "," + m_description + "," + m_bitPosition.ToString() + "," + m_enabled.ToString() + "," + m_inputMeasurementKey + "," + m_parentCircuitBreakerID.ToString();
        }

        /// <summary>
        /// A verbose string representation of the instance of the class and can be used for descriptive text output in a console or text file.
        /// </summary>
        /// <returns>A verbose string representatin of the instance of the class.</returns>
        public string ToVerboseString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("----- Breaker Status -----------------------------------------------------------");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("      InternalID: " + m_internalID.ToString() + "{0}", Environment.NewLine);
            stringBuilder.AppendFormat("          Number: " + m_number.ToString() + "{0}", Environment.NewLine);
            stringBuilder.AppendFormat("            Name: " + m_name + "{0}", Environment.NewLine);
            stringBuilder.AppendFormat("     Description: " + m_description + "{0}", Environment.NewLine);
            stringBuilder.AppendFormat("          Status: " + this.Value.ToString() + "{0}", Environment.NewLine);
            stringBuilder.AppendFormat("    Binary Value: " + AddPrefixOfZerosIfNeeded(Convert.ToString(m_binaryValue, 2)) + "{0}", Environment.NewLine);
            stringBuilder.AppendFormat("             Bit: " + m_bitPosition.ToString() + "{0}", Environment.NewLine);
            stringBuilder.AppendFormat("             Key: " + m_inputMeasurementKey + "{0}", Environment.NewLine);
            if (m_parentCircuitBreaker != null)
            {
                stringBuilder.AppendFormat("       Parent CB: " + m_parentCircuitBreaker.ToString() + "{0}", Environment.NewLine);
            }
            else
            {
                stringBuilder.AppendFormat("       Parent CB: Unclaimed {0}", Environment.NewLine);
            }
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }

        /// <summary>
        /// A string representation special to the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> class.
        /// </summary>
        /// <returns>A string representation special to the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> class.</returns>
        public string ToStatusString()
        {
            return "-" + Acronym + "- " + "(" + this.Value.ToString() + ")" + " " + m_description + " " + m_bitPosition.ToString() + " for " + m_name;
        }

        /// <summary>
        /// Performs a deep copy of the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> object
        /// </summary>
        /// <returns>A deep copy of the <see cref="LinearStateEstimator.Measurements.BreakerStatus"/> object</returns>
        public BreakerStatus Copy()
        {
            BreakerStatus copy = (BreakerStatus)this.MemberwiseClone();
            copy.BitPosition = this.m_bitPosition;

            // To initialize the parsing of the value
            copy.BinaryValue = copy.BinaryValue;

            return copy;
        }

        public void Keyify(string rootKey)
        {
            Key = $"{rootKey}.Breaker.Bit{BitPosition}";
        }
        #endregion
    }
}
