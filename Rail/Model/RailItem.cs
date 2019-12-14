﻿using Rail.Controls;
using Rail.Misc;
using Rail.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rail.Model
{
    [DebuggerDisplay("RailItem Id={Id} X={Position.X} Y={Position.Y} A={Angle}")]

    public class RailItem
    {
        private static int globalDebugIndex = 0;
        private int debugIndex;
        
        public DelegateCommand OptionsCommand { get; private set; }

        public RailItem(TrackBase track, Point pos, ushort layer) 
        {
            this.OptionsCommand = new DelegateCommand(OnOptions);
            this.debugIndex = globalDebugIndex++;
            this.Id = track.Id;
            this.Track = track;
            this.Position = pos;
            this.Angle = 0.0;
            this.Layer = layer;
            this.DockPoints = track.DockPoints.Select(dp => new RailDockPoint(this, dp).Move(this.Position)).ToArray();
        }

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlIgnore]
        public TrackBase Track { get; set; }

        [XmlIgnore]
        public RailDockPoint[] DockPoints { get; private set; }

        [XmlIgnore]
        public Point Position;

        [XmlAttribute("X")]
        public double X 
        {
            get { return this.Position.X; } 
            set { this.Position.X = value; }
        }

        [XmlAttribute("Y")]
        public double Y 
        {
            get { return this.Position.Y; }
            set { this.Position.Y = value; }
        }

        [XmlAttribute("Angle")]
        public Angle Angle { get; set; }

        [XmlAttribute("Layer")]
        public ushort Layer { get; set; }

        //[XmlArray("Docks")]
        //[XmlArrayItem("Dock")]
        //public RailDock[] Docks { get; set; }

        public bool HasOnlyOneDock { get { return this.DockPoints.One(dp => dp.IsDocked);  } }

        public bool HasDocks { get { return this.DockPoints.Any(dp => dp.IsDocked); } }

        public void Move(Vector vec)
        {
            this.Position += vec;
            this.DockPoints.ToList().ForEach(dp => dp.Move(vec));
        }

        public void Rotate(Angle angle)
        {
            this.Angle += angle;
            this.DockPoints.ToList().ForEach(dp => dp.Rotate(angle, this.Position));
        }

        public void Rotate(Angle angle, Point center)
        {
            this.Angle += angle;
            this.Position = this.Position.Rotate(angle, center);
            this.DockPoints.ToList().ForEach(dp => dp.Rotate(angle, center));
        }

        public void Rotate(Angle angle, RailItem center)
        {
            Rotate(angle, center.Position);
        }

        public void DrawTrack(DrawingContext drawingContext, RailViewMode viewMode)
        {
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform (this.Angle));
            transformGroup.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            drawingContext.PushTransform(transformGroup);

            this.Track.Render(drawingContext, viewMode);

            drawingContext.Pop();
        }

        private static Pen dockPen = new Pen(Brushes.Blue, 1);
        private static Pen positionPen = new Pen(Brushes.Red, 2);
        
        public void DrawDockPoints(DrawingContext drawingContext)
        {
            foreach (var point in this.DockPoints)
            {
                drawingContext.DrawEllipse(null, dockPen, point.Position, this.Track.Spacing / 2, this.Track.Spacing / 2);
                if (!point.IsDocked)
                {
                    drawingContext.DrawLine(positionPen, point.Position, point.Position.Circle(point.Angle, this.Track.Spacing));
                }
            }
        }

        public bool IsInside(Point point)
        {
            TransformGroup grp = new TransformGroup();
            grp.Children.Add(new TranslateTransform(this.Position.X, this.Position.Y));
            grp.Children.Add(new RotateTransform(this.Angle, this.Position.X, this.Position.Y));

            Geometry geometry = this.Track.Geometry.Clone();
            geometry.Transform = grp;
            bool f = geometry.FillContains(point);
            return f;
        }

        public void OnOptions()
        {

        }
    }
}