﻿using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackStraightUncoupler : TrackStraight
    {
        [XmlIgnore]
        public override string Name
        {
            get
            {
                return $"{Resources.TrackStraightUncoupler} {Length} mm";
            }
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return $"{this.Article} {Resources.TrackStraightUncoupler} {Length} mm";
            }
        }
    }
}