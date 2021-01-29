﻿// <copyright file="PreOperationmsdyn_workorderUpdate.cs" company="">PreOperationmsdyn_workorderUpdate
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Hong Liu</author>
// <date>9/20/2018 10:21:27 AM</date>
// <summary>Implements the PreOperationmsdyn_workorderUpdate Plugin.</summary>
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
// </auto-generated>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xrm.Sdk;
using TSIS2.Common;

namespace TSIS2.Plugins
{

    [CrmPluginRegistration(
    MessageNameEnum.Update,
    "msdyn_workorder",
    StageEnum.PreOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "PreOperationmsdyn_workorderUpdate Plugin",
    1,
    IsolationModeEnum.Sandbox)]
    /// <summary>
    /// PreOperationmsdyn_workorderUpdate Plugin.
    /// </summary>    
    public class PreOperationmsdyn_workorderUpdate : PluginBase
    {
        private readonly string preImageAlias = "PreImage";

        /// <summary>
        /// Initializes a new instance of the <see cref="PreOperationmsdyn_workorderUpdate"/> class.
        /// </summary>
        /// <param name="unsecure">Contains public (unsecured) configuration information.</param>
        /// <param name="secure">Contains non-public (secured) configuration information. 
        /// When using Microsoft Dynamics 365 for Outlook with Offline Access, 
        /// the secure string is not passed to a plug-in that executes while the client is offline.</param>
        public PreOperationmsdyn_workorderUpdate(string unsecure, string secure)
            : base(typeof(PreOperationmsdyn_workorderUpdate))
        {
            //if (secure != null &&!secure.Equals(string.Empty))
            //{

            //}
        }

        /// <summary>
        /// Main entry point for he business logic that the plug-in is to execute.
        /// </summary>
        /// <param name="localContext">The <see cref="LocalPluginContext"/> which contains the
        /// <see cref="IPluginExecutionContext"/>,
        /// <see cref="IOrganizationService"/>
        /// and <see cref="ITracingService"/>
        /// </param>
        /// <remarks>
        /// For improved performance, Microsoft Dynamics 365 caches plug-in instances.
        /// The plug-in's Execute method should be written to be stateless as the constructor
        /// is not called for every invocation of the plug-in. Also, multiple system threads
        /// could execute the plug-in at the same time. All per invocation state information
        /// is stored in the context. This means that you should not use global variables in plug-ins.
        /// </remarks>
        protected override void ExecuteCrmPlugin(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new InvalidPluginExecutionException("localContext");
            }

            IPluginExecutionContext context = localContext.PluginExecutionContext;
            Entity target = (Entity)context.InputParameters["Target"];
            //Entity postImageEntity = (context.PostEntityImages != null && context.PostEntityImages.Contains(this.postImageAlias)) ? context.PostEntityImages[this.postImageAlias] : null;
            Entity preImageEntity = (context.PreEntityImages != null && context.PreEntityImages.Contains(this.preImageAlias)) ? context.PreEntityImages[this.preImageAlias] : null;

            try
            {
                if (target.LogicalName.Equals(msdyn_workorder.EntityLogicalName))
                {
                    if (target.Attributes.Contains("ovs_operationid") && target.Attributes["ovs_operationid"] != null)
                    {
                        EntityReference operation = (EntityReference)target.Attributes["ovs_operationid"];
                        using (var servicecontext = new CrmServiceContext(localContext.OrganizationService))
                        {
                            var regulatedentity = (from tt in servicecontext.ovs_operationSet
                                                   where tt.Id == operation.Id
                                                   select new
                                                   {
                                                       tt.ovs_RegulatedEntityId
                                                   }).FirstOrDefault();
                            if (regulatedentity != null)
                            {
                                if (regulatedentity.ovs_RegulatedEntityId != null && regulatedentity.ovs_RegulatedEntityId.Id != null)
                                {
                                    target.Attributes["msdyn_billingaccount"] = regulatedentity.ovs_RegulatedEntityId;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}