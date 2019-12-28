﻿using Rail.Controls;
using Rail.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{

    public class TrackStraight : TrackBase
    {
        [XmlAttribute("Length")]
        public double Length { get; set; }

        [XmlIgnore]
        public override string Name 
        { 
            get 
            { 
                return $"{Resources.TrackStraight} {Length} mm"; 
            } 
        
        }
        protected override Geometry CreateGeometry(double spacing)
        {
            return StraitGeometry(this.Length, StraitOrientation.Center, spacing); 
        }

        protected override Drawing CreateRailDrawing(bool isSelected)
        {
            DrawingGroup drawingRail = new DrawingGroup();
            if (this.Ballast)
            {
                drawingRail.Children.Add(StraitBallast(this.Length, StraitOrientation.Center, 0, null));
            }
            drawingRail.Children.Add(StraitRail(isSelected, this.Length));
            return drawingRail;
        }

        protected override List<TrackDockPoint> CreateDockPoints()
        {
            return new List<TrackDockPoint>
            {
                new TrackDockPoint(0, new Point(-this.Length / 2.0, 0.0), 135, this.dockType),
                new TrackDockPoint(1, new Point(+this.Length / 2.0, 0.0), 315, this.dockType)
            };
        }
    }
}
