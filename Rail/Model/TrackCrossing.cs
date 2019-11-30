﻿using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    public class TrackCrossing : TrackBase
    {
        [XmlAttribute("Length1")]
        public double Length1 { get; set; }

        [XmlAttribute("Length2")]
        public double Length2 { get; set; }

        [XmlAttribute("Angle")]
        public double Angle { get; set; }

        public override void Update(double spacing)
        {
            base.Update(spacing);

            this.geometry = CreateCrossingTrackGeometry(this.Length1, this.Length2, this.Angle);
        }
    }
}