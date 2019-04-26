/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Reflection;
using System.Collections.Generic;
using log4net;
using OpenMetaverse;
using OpenMetaverse.StructuredData;

namespace OpenSim.Framework
{
    /// <summary>
    /// Circuit data for an agent.  Connection information shared between
    /// regions that accept UDP connections from a client
    /// </summary>
    public class AgentCircuitData
    {
// DEBUG ON
        private static readonly ILog m_log =
                LogManager.GetLogger(
                MethodBase.GetCurrentMethod().DeclaringType);
// DEBUG OFF

        /// <summary>
        /// Avatar Unique Agent Identifier
        /// </summary>
        public UUID AgentID;

        /// <summary>
        /// Avatar's Appearance
        /// </summary>
        public AvatarAppearance Appearance;

        /// <summary>
        /// Agent's root inventory folder
        /// </summary>
        public UUID BaseFolder;

        /// <summary>
        /// Base Caps path for user
        /// </summary>
        public string CapsPath = String.Empty;

        /// <summary>
        /// Seed caps for neighbor regions that the user can see into
        /// </summary>
        public Dictionary<ulong, string> ChildrenCapSeeds;

        /// <summary>
        /// Root agent, or Child agent
        /// </summary>
        public bool child;

        /// <summary>
        /// Number given to the client when they log-in that they provide
        /// as credentials to the UDP server
        /// </summary>
        public uint circuitcode;

        /// <summary>
        /// How this agent got here
        /// </summary>
        public uint teleportFlags;

        /// <summary>
        /// Agent's account first name
        /// </summary>
        public string firstname;
        public UUID InventoryFolder;

        /// <summary>
        /// Agent's account last name
        /// </summary>
        public string lastname;

        /// <summary>
        /// Agent's full name.
        /// </summary>
        public string Name { get { return string.Format("{0} {1}", firstname, lastname); } }

        /// <summary>
        /// Random Unique GUID for this session.  Client gets this at login and it's
        /// only supposed to be disclosed over secure channels
        /// </summary>
        public UUID SecureSessionID;

        /// <summary>
        /// Non secure Session ID
        /// </summary>
        public UUID SessionID;

        /// <summary>
        /// Hypergrid service token; generated by the user domain, consumed by the receiving grid.
        /// There is one such unique token for each grid visited.
        /// </summary>
        public string ServiceSessionID = string.Empty;

        /// <summary>
        /// The client's IP address, as captured by the login service
        /// </summary>
        public string IPAddress;

        /// <summary>
        /// Viewer's version string as reported by the viewer at login
        /// </summary>
        private string m_viewerInternal;

        /// <summary>
        /// Viewer's version string
        /// </summary>
        public string Viewer
        {
            set { m_viewerInternal = value; }

            // Try to return consistent viewer string taking into account
            // that viewers have chaagned how version is reported
            // See http://opensimulator.org/mantis/view.php?id=6851
            get
            {
                // Old style version string contains viewer name followed by a space followed by a version number
                if (m_viewerInternal == null || m_viewerInternal.Contains(" "))
                {
                    return m_viewerInternal;
                }
                else // New style version contains no spaces, just version number
                {
                    return Channel + " " + m_viewerInternal;
                }
            }
        }

        /// <summary>
        /// The channel strinf sent by the viewer at login
        /// </summary>
        public string Channel;

        /// <summary>
        /// The Mac address as reported by the viewer at login
        /// </summary>
        public string Mac;

        /// <summary>
        /// The id0 as reported by the viewer at login
        /// </summary>
        public string Id0;

        /// <summary>
        /// Position the Agent's Avatar starts in the region
        /// </summary>
        public Vector3 startpos;
        public float startfar = -1.0f;

        public Dictionary<string, object> ServiceURLs;

        public AgentCircuitData()
        {
        }

        /// <summary>
        /// Pack AgentCircuitData into an OSDMap for transmission over LLSD XML or LLSD json
        /// </summary>
        /// <returns>map of the agent circuit data</returns>
        public OSDMap PackAgentCircuitData(EntityTransferContext ctx)
        {
            OSDMap args = new OSDMap();
            args["agent_id"] = OSD.FromUUID(AgentID);
            args["base_folder"] = OSD.FromUUID(BaseFolder);
            args["caps_path"] = OSD.FromString(CapsPath);

            if (ChildrenCapSeeds != null)
            {
                OSDArray childrenSeeds = new OSDArray(ChildrenCapSeeds.Count);
                foreach (KeyValuePair<ulong, string> kvp in ChildrenCapSeeds)
                {
                    OSDMap pair = new OSDMap();
                    pair["handle"] = OSD.FromString(kvp.Key.ToString());
                    pair["seed"] = OSD.FromString(kvp.Value);
                    childrenSeeds.Add(pair);
                }
                if (ChildrenCapSeeds.Count > 0)
                    args["children_seeds"] = childrenSeeds;
            }
            args["child"] = OSD.FromBoolean(child);
            args["circuit_code"] = OSD.FromString(circuitcode.ToString());
            args["first_name"] = OSD.FromString(firstname);
            args["last_name"] = OSD.FromString(lastname);
            args["inventory_folder"] = OSD.FromUUID(InventoryFolder);
            args["secure_session_id"] = OSD.FromUUID(SecureSessionID);
            args["session_id"] = OSD.FromUUID(SessionID);

            args["service_session_id"] = OSD.FromString(ServiceSessionID);
            args["start_pos"] = OSD.FromString(startpos.ToString());
            args["client_ip"] = OSD.FromString(IPAddress);
            args["viewer"] = OSD.FromString(Viewer);
            args["channel"] = OSD.FromString(Channel);
            args["mac"] = OSD.FromString(Mac);
            args["id0"] = OSD.FromString(Id0);
            if(startfar > 0)
                args["far"] = OSD.FromReal(startfar);

            if (Appearance != null)
            {
                args["appearance_serial"] = OSD.FromInteger(Appearance.Serial);

                OSDMap appmap = Appearance.Pack(ctx);
                args["packed_appearance"] = appmap;
            }

            // Old, bad  way. Keeping it fow now for backwards compatibility
            // OBSOLETE -- soon to be deleted
            if (ServiceURLs != null && ServiceURLs.Count > 0)
            {
                OSDArray urls = new OSDArray(ServiceURLs.Count * 2);
                foreach (KeyValuePair<string, object> kvp in ServiceURLs)
                {
                    //System.Console.WriteLine("XXX " + kvp.Key + "=" + kvp.Value);
                    urls.Add(OSD.FromString(kvp.Key));
                    urls.Add(OSD.FromString((kvp.Value == null) ? string.Empty : kvp.Value.ToString()));
                }
                args["service_urls"] = urls;
            }

            // again, this time the right way
            if (ServiceURLs != null && ServiceURLs.Count > 0)
            {
                OSDMap urls = new OSDMap();
                foreach (KeyValuePair<string, object> kvp in ServiceURLs)
                {
                    //System.Console.WriteLine("XXX " + kvp.Key + "=" + kvp.Value);
                    urls[kvp.Key] = OSD.FromString((kvp.Value == null) ? string.Empty : kvp.Value.ToString());
                }
                args["serviceurls"] = urls;
            }


            return args;
        }

        /// <summary>
        /// Unpack agent circuit data map into an AgentCiruitData object
        /// </summary>
        /// <param name="args"></param>
        public void UnpackAgentCircuitData(OSDMap args)
        {
            if (args["agent_id"] != null)
                AgentID = args["agent_id"].AsUUID();
            if (args["base_folder"] != null)
                BaseFolder = args["base_folder"].AsUUID();
            if (args["caps_path"] != null)
                CapsPath = args["caps_path"].AsString();

            if ((args["children_seeds"] != null) && (args["children_seeds"].Type == OSDType.Array))
            {
                OSDArray childrenSeeds = (OSDArray)(args["children_seeds"]);
                ChildrenCapSeeds = new Dictionary<ulong, string>();
                foreach (OSD o in childrenSeeds)
                {
                    if (o.Type == OSDType.Map)
                    {
                        ulong handle = 0;
                        string seed = "";
                        OSDMap pair = (OSDMap)o;
                        if (pair["handle"] != null)
                            if (!UInt64.TryParse(pair["handle"].AsString(), out handle))
                                continue;
                        if (pair["seed"] != null)
                            seed = pair["seed"].AsString();
                        if (!ChildrenCapSeeds.ContainsKey(handle))
                            ChildrenCapSeeds.Add(handle, seed);
                    }
                }
            }
            else
                ChildrenCapSeeds = new Dictionary<ulong, string>();

            if (args["child"] != null)
                child = args["child"].AsBoolean();
            if (args["circuit_code"] != null)
                UInt32.TryParse(args["circuit_code"].AsString(), out circuitcode);
            if (args["first_name"] != null)
                firstname = args["first_name"].AsString();
            if (args["last_name"] != null)
                lastname = args["last_name"].AsString();
            if (args["inventory_folder"] != null)
                InventoryFolder = args["inventory_folder"].AsUUID();
            if (args["secure_session_id"] != null)
                SecureSessionID = args["secure_session_id"].AsUUID();
            if (args["session_id"] != null)
                SessionID = args["session_id"].AsUUID();
            if (args["service_session_id"] != null)
                ServiceSessionID = args["service_session_id"].AsString();
            if (args["client_ip"] != null)
                IPAddress = args["client_ip"].AsString();
            if (args["viewer"] != null)
                Viewer = args["viewer"].AsString();
            if (args["channel"] != null)
                Channel = args["channel"].AsString();
            if (args["mac"] != null)
                Mac = args["mac"].AsString();
            if (args["id0"] != null)
                Id0 = args["id0"].AsString();
            if (args["teleport_flags"] != null)
                teleportFlags = args["teleport_flags"].AsUInteger();

            if (args["start_pos"] != null)
                Vector3.TryParse(args["start_pos"].AsString(), out startpos);

            if(args["far"] != null)
                startfar = (float)args["far"].AsReal();

            //m_log.InfoFormat("[AGENTCIRCUITDATA]: agentid={0}, child={1}, startpos={2}", AgentID, child, startpos);

            try
            {
                // Unpack various appearance elements
                Appearance = new AvatarAppearance();

                // Eventually this code should be deprecated, use full appearance
                // packing in packed_appearance
                if (args["appearance_serial"] != null)
                    Appearance.Serial = args["appearance_serial"].AsInteger();

                if (args.ContainsKey("packed_appearance") && (args["packed_appearance"].Type == OSDType.Map))
                {
                    Appearance.Unpack((OSDMap)args["packed_appearance"]);
//                    m_log.InfoFormat("[AGENTCIRCUITDATA] unpacked appearance");
                }
                else
                {
                    m_log.Warn("[AGENTCIRCUITDATA]: failed to find a valid packed_appearance");
                }
            }
            catch (Exception e)
            {
                m_log.ErrorFormat("[AGENTCIRCUITDATA] failed to unpack appearance; {0}",e.Message);
            }

            ServiceURLs = new Dictionary<string, object>();
            // Try parse the new way, OSDMap
            if (args.ContainsKey("serviceurls") && args["serviceurls"] != null && (args["serviceurls"]).Type == OSDType.Map)
            {
                OSDMap urls = (OSDMap)(args["serviceurls"]);
                foreach (KeyValuePair<String, OSD> kvp in urls)
                {
                    ServiceURLs[kvp.Key] = kvp.Value.AsString();
                    //System.Console.WriteLine("XXX " + kvp.Key + "=" + ServiceURLs[kvp.Key]);

                }
            }
            // else try the old way, OSDArray
            // OBSOLETE -- soon to be deleted
            else if (args.ContainsKey("service_urls") && args["service_urls"] != null && (args["service_urls"]).Type == OSDType.Array)
            {
                OSDArray urls = (OSDArray)(args["service_urls"]);
                for (int i = 0; i < urls.Count / 2; i++)
                {
                    ServiceURLs[urls[i * 2].AsString()] = urls[(i * 2) + 1].AsString();
                    //System.Console.WriteLine("XXX " + urls[i * 2].AsString() + "=" + urls[(i * 2) + 1].AsString());

                }
            }
        }

    }


}
