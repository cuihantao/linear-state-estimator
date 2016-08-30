﻿//******************************************************************************************************
//  ServiceClient.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
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
//  05/04/2009 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF;
using GSF.IO;
using GSF.PhasorProtocols;
using GSF.ServiceProcess;
using GSF.TimeSeries;
using PhasorProtocolAdapters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;

namespace LinearStateEstimator
{
    public class ServiceClient : ServiceClientBase
    {
        protected override void ClientHelper_ReceivedServiceResponse(object sender, EventArgs<ServiceResponse> e)
        {
            List<object> attachments = e.Argument.Attachments;

            // Handle any special attachments coming in from service
            if (attachments != null)
            {
                foreach (object attachment in attachments)
                {
                    if (attachment is ConfigurationErrorFrame)
                    {
                        Console.WriteLine("Received configuration error frame, invocation request for device configuration has failed. See common phasor services response for reason.\r\n");
                    }
                    else if (attachment is IConfigurationFrame)
                    {
                        // Attachment is a configuration frame, serialize it to XML and open it in a browser
                        IConfigurationFrame configurationFrame = attachment as IConfigurationFrame;
                        string fileName = string.Format("{0}\\DownloadedConfiguration-ID[{1}].xml", FilePath.GetAbsolutePath(""), configurationFrame.IDCode);
                        FileStream configFile = File.Create(fileName);
                        SoapFormatter xmlSerializer = new SoapFormatter();

                        xmlSerializer.AssemblyFormat = FormatterAssemblyStyle.Simple;
                        xmlSerializer.TypeFormat = FormatterTypeStyle.TypesWhenNeeded;

                        try 
                        {
                            // Attempt to serialize configuration frame as XML
                            xmlSerializer.Serialize(configFile, configurationFrame);
                        }
                        catch (Exception ex)
                        {                    		
                            byte[] errorMessage = Encoding.UTF8.GetBytes(ex.Message);
                            configFile.Write(errorMessage, 0, errorMessage.Length);
                            Console.Write("Failed to serialize configuration frame: {0}", ex.Message);
                        }

                        configFile.Close();

                        // Open captured XML sample file in explorer...
                        Process.Start("explorer.exe", fileName);
                    }
                }
            }

            // Allow base class to handle common response
            base.ClientHelper_ReceivedServiceResponse(sender, e);
        }
    }
}