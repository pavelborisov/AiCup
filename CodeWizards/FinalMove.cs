﻿using System;
using Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk
{
    public class FinalMove
    {
        private Move _move;

        public FinalMove(Move move)
        {
            _move = move;
        }

        public double Turn
        {
            get { return _move.Turn; }
            set { _move.Turn = value; }
        }

        public double Speed
        {
            get { return _move.Speed; }
            set { _move.Speed = value; }
        }

        public double StrafeSpeed
        {
            get { return _move.StrafeSpeed; }
            set { _move.StrafeSpeed = value; }
        }

        public ActionType? Action
        {
            get { return _move.Action; }
            set { _move.Action = value; }
        }

        public double CastAngle
        {
            get { return _move.CastAngle; }
            set { _move.CastAngle = value; }
        }

        public double MinCastDistance
        {
            get { return _move.MinCastDistance; }
            set { _move.MinCastDistance = value; }
        }

        public double MaxCastDistance
        {
            get { return _move.MaxCastDistance; }
            set { _move.MaxCastDistance = value; }
        }

        public long StatusTargetId
        {
            get { return _move.StatusTargetId; }
            set { _move.StatusTargetId = value; }
        }

        public Message[] Messages
        {
            get { return _move.Messages; }
            set { _move.Messages = value; }
        }

        public void MoveTo(Point to, Point turnTo)
        {
            var self = MyStrategy.ASelf;
            if (turnTo != null)
            {
                _move.Turn = Utility.EnsureInterval(MyStrategy.Self.GetAngleTo(turnTo.X, turnTo.Y), self.MaxTurnAngle);
            }

            if (to != null && !Utility.PointsEqual(self, to))
            {
                var angle = self.GetAngleTo(to);
                var d = AWizard._getHalfEllipseDxDy(self.MaxStrafeSpeed, self.MaxForwardSpeed, self.MaxBackwardSpeed, angle);

                _move.Speed = d.Y;
                _move.StrafeSpeed = d.X;
            }
        }

        public void Apply(FinalMove move)
        {
            Turn = move.Turn;
            Speed = move.Speed;
            StrafeSpeed = move.StrafeSpeed;
            Action = move.Action;
            CastAngle = move.CastAngle;
            MinCastDistance = move.MinCastDistance;
            MaxCastDistance = move.MaxCastDistance;
            StatusTargetId = move.StatusTargetId;
            Messages = move.Messages;
        }

        public override string ToString()
        {
            // for DEBUG
            var res = "(" + Speed.ToString().Replace(',', '.') + ", " + StrafeSpeed.ToString().Replace(',', '.') + ")";
            if (Math.Abs(Turn) > Const.Eps)
                res += " " + Turn.ToString().Replace(',', '.');
            if (Action != null)
                res += " " + Action;
            return res;
        }
    }
}
