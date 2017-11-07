﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Com.CodeGame.CodeWars2017.DevKit.CSharpCgdk.Visualizer
{
    public partial class VisualizerForm : Form
    {
        public VisualizerForm()
        {
            InitializeComponent();
        }

        private double _step = 1.3;

        private void buttonPause_Click(object sender, EventArgs e)
        {
            Visualizer.Pause ^= true;
        }

        private void buttonUnZoom_Click(object sender, EventArgs e)
        {
            Visualizer.Zoom *= _step;
        }

        private void buttonZoom_Click(object sender, EventArgs e)
        {
            Visualizer.Zoom /= _step;
        }

        private void renderButton_Click(object sender, EventArgs e)
        {
            if (true)
            {
                Visualizer.DrawSince = 0;
            }
            else
            {
                Visualizer.DrawSince = int.MaxValue;
            }
        }

        private void VisualizerForm_KeyDown(object sender, KeyEventArgs e)
        {
            var lookAt = Visualizer.LookAt;
            var speed = 0.1;
            switch (e.KeyData)
            {
                case Keys.Right:
                    lookAt.X += Const.MapSize*Visualizer.Zoom * speed;
                    Visualizer.LookAt = lookAt;
                    break;
                case Keys.Left:
                    lookAt.X -= Const.MapSize * Visualizer.Zoom * speed;
                    Visualizer.LookAt = lookAt;
                    break;
                case Keys.Down:
                    lookAt.Y += Const.MapSize * Visualizer.Zoom * speed;
                    Visualizer.LookAt = lookAt;
                    break;
                case Keys.Up:
                    lookAt.Y -= Const.MapSize * Visualizer.Zoom * speed;
                    Visualizer.LookAt = lookAt;
                    break;
            }
        }
    }
}
