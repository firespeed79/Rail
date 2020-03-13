﻿using Rail.Tracks.Misc;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rail.Tracks
{
    public class TrackType
    {
        public TrackType()
        {
            this.Tracks = new List<TrackBase>();
            this.Name = new XmlMultilanguageString("New Track Type");
            this.Parameter = new TrackParameter();
            this.Radii = new List<TrackNamedValue>();
            this.Lengths = new List<TrackNamedValue>();
            this.Angles = new List<TrackNamedValue>();
        }

        [XmlElement("Name")]
        public XmlMultilanguageString Name { get; set; }

        [XmlElement("Parameter")]
        public TrackParameter Parameter { get; set; }

        [XmlArray("Radii")]
        [XmlArrayItem("Radius")]
        public List<TrackNamedValue> Radii { get; set; } 

        [XmlArray("Lengths")]
        [XmlArrayItem("Length")]
        public List<TrackNamedValue> Lengths { get; set; }

        [XmlArray("Angles")]
        [XmlArrayItem("Angle")]
        public List<TrackNamedValue> Angles { get; set; }

        [XmlArray("Tracks")]
        [XmlArrayItem(typeof(TrackStraight), ElementName = "Straight"),
         XmlArrayItem(typeof(TrackCurved), ElementName = "Curved"),
         XmlArrayItem(typeof(TrackEndPiece), ElementName = "EndPiece"),
         XmlArrayItem(typeof(TrackAdapter), ElementName = "Adapter"),
         XmlArrayItem(typeof(TrackAdjustment), ElementName = "StraightAdjustment"),
         XmlArrayItem(typeof(TrackFlex), ElementName = "Flex"),

         XmlArrayItem(typeof(TrackTurnout), ElementName = "Turnout"),
         XmlArrayItem(typeof(TrackCurvedTurnout), ElementName = "CurvedTurnout"),
         XmlArrayItem(typeof(TrackDoubleSlipSwitch), ElementName = "DoubleSlipSwitch"),
         XmlArrayItem(typeof(TrackDoubleCrossover), ElementName = "DoubleCrossover"),

         XmlArrayItem(typeof(TrackCrossing), ElementName = "Crossing"),
         XmlArrayItem(typeof(TrackStar), ElementName = "Star"),

         XmlArrayItem(typeof(TrackTurntable), ElementName = "Turntable"),
         XmlArrayItem(typeof(TrackTransferTable), ElementName = "TransferTable"),

         XmlArrayItem(typeof(TrackGroup), ElementName = "Group")]
        public List<TrackBase> Tracks { get; set; }

        [XmlArray("Groups")]
        [XmlArrayItem("Group")]
        public List<TrackGroup> Groups { get; set; }

        public void Update()
        {
            _ = this.Parameter ?? throw new Exception($"Parameter not set");
            this.Tracks.ForEach(track => track.Update(this));
        }
    }
}
