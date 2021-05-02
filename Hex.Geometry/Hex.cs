using Hex.Geometry.Blerpables;
using Hex.Geometry.Interfaces;
using Hex.Geometry.Vectors;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Hex.Geometry
{
    public readonly struct Hex<T>:I3dIndexable, IEquatable<Hex<T>> where T:Blerpable
    {
        public HexIndex3d Index { get; }
        public T Payload { get; }

        public Hex(HexIndex3d index, T payload) : this()
        {
            Payload = payload;
            Index = index;
        }

        //public string DebugData;

        //

        //public bool IsBorder;

        //

        //private bool _notNull;

        public int XIndex => Index.XIndex;
        public int YIndex => Index.YIndex;
        public int ZIndex => Index.ZIndex;


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

        public override bool Equals(object obj) => obj is Hex<T> hex && Equals(hex);
        public bool Equals(Hex<T> other) => Index.Equals(other.Index);
        public override int GetHashCode() => -2134847229 + Index.GetHashCode();

        public static bool operator ==(Hex<T> left, Hex<T> right) => left.Equals(right);
        public static bool operator !=(Hex<T> left, Hex<T> right) => !(left == right);
    }


}