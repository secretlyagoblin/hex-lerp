using Hex.Geometry.Blerpables;
using Hex.Geometry.Interfaces;
using Hex.Geometry.Vectors;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Hex.Geometry
{
    public readonly struct Hex<T> : IHex<T> where T : IHexData<T>
    {    
        private readonly I3dIndexable _index;
        private readonly I2dPositionable _position;

        public int XIndex => _index.XIndex;
        public int YIndex => _index.YIndex;
        public int ZIndex => _index.ZIndex;
        public double XPos => _position.XPos;
        public double YPos => _position.YPos;

        public T Payload { get; }

        public Int3 Index3d => _index.Index3d;

        public Vector2 Position2d => _position.Position2d;

        public Hex(I3dIndexable index, T payload) : this()
        {
            Payload = payload;
            _index = index ?? throw new ArgumentNullException(nameof(index));
            _position = index.GetHexPosition2d();
        }

        //public string DebugData;

        //

        //public bool IsBorder;

        //

        //private bool _notNull;




        /*

        /// <summary>
        /// Create a new hex just from the XY. Will need to be expanded later.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Hex(HexIndex index, HexPayload payload, bool isBorder, string debugData = "")
        {
            Index = index;
            Payload = payload;
            DebugData = debugData;
            IsBorder = isBorder;

            _notNull = true;
        }

        public Hex(Hex hex, bool isBorder)
        {
            Index = hex.Index;
            Payload = hex.Payload;
            DebugData = hex.DebugData;
            IsBorder = isBorder;

            _notNull = true;
        }

        public static Hex InvalidHex
        {
            get {
                return new Hex()
                {
                    IsBorder = true,
                    _notNull = false
                };
            }
        }



        public static bool IsInvalid(Hex hex)
        {
            return !hex._notNull;
        }

        */


        public T Blerp(T b, T c, I3dPositionable weight)
        {
            return this.Payload.Blerp(b, c, weight);
        }

        public static bool operator ==(Hex<T> left, Hex<T> right) => left.Equals(right);
        public static bool operator !=(Hex<T> left, Hex<T> right) => !(left == right);
    }


}